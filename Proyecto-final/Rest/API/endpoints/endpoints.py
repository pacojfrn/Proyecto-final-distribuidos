from flask import Blueprint, request, jsonify
from API import db
from ..database.db import User
from API import cache
from ..cache.cache import redis_client, clear_product_quantity_cache
import json
from ..Soap.client import verify_persona
from flasgger import swag_from

product_blueprint = Blueprint('users', __name__)

@product_blueprint.route('/users', methods=['POST'])
@swag_from({
    'tags': ['Users'],
    'description': 'Create a new user',
    'parameters': [
        {
            'name': 'body',
            'in': 'body',
            'required': True,
            'schema': {
                'type': 'object',
                'properties': {
                    'name': {'type': 'string', 'example': 'John Doe'},
                    'persona': {'type': 'string', 'example': 'SoapAPI User'}
                },
                'required': ['name', 'persona']
            }
        }
    ],
    'responses': {
        201: {
            'description': 'User created successfully',
            'schema': {
                'type': 'object',
                'properties': {
                    'id': {'type': 'integer'},
                    'name': {'type': 'string'},
                    'persona': {'type': 'string'}
                }
            }
        },
        400: {'description': 'Invalid input'}
    }
})
def create_user():
    data = request.get_json()
    name = data.get('name')
    persona = data.get('persona')

    if not name or not persona:
        return jsonify({"error": "Name and Persona are required"}), 400

    # Verificar el ObjectId con la SOAP API
    if not verify_persona(persona):
        return jsonify({"error": "Persona ObjectId not found"}), 400

    new_user = User(name=name, persona=persona)
    db.session.add(new_user)
    db.session.commit()

    redis_client.set(f"user:{new_user.id}", json.dumps({"name": name, "persona": persona}))

    return jsonify({"id": new_user.id, "name": name, "persona": persona}), 201


@product_blueprint.route('/users/<int:user_id>', methods=['GET'])
@swag_from({
    'tags': ['Users'],
    'description': 'Retrieve a user by ID',
    'parameters': [
        {
            'name': 'user_id',
            'in': 'path',
            'type': 'integer',
            'required': True,
            'description': 'The ID of the user to retrieve'
        }
    ],
    'responses': {
        200: {
            'description': 'User retrieved successfully',
            'schema': {
                'type': 'object',
                'properties': {
                    'id': {'type': 'integer'},
                    'name': {'type': 'string'},
                    'persona': {'type': 'string'}
                }
            }
        },
        404: {'description': 'User not found'}
    }
})
def get_user(user_id):
    cached_user = redis_client.get(f"user:{user_id}")
    if cached_user:
        return jsonify(json.loads(cached_user)), 200

    user = User.query.get(user_id)
    if not user:
        return jsonify({"error": "User not found"}), 404

    redis_client.set(f"user:{user.id}", json.dumps({"name": user.name, "persona": user.persona}))
    return jsonify({"id": user.id, "name": user.name, "persona": user.persona}), 200

@product_blueprint.route('/users/<int:user_id>', methods=['DELETE'])
@swag_from({
    'tags': ['Users'],
    'description': 'Delete a user by ID',
    'parameters': [
        {
            'name': 'user_id',
            'in': 'path',
            'type': 'integer',
            'required': True,
            'description': 'The ID of the user to delete'
        }
    ],
    'responses': {
        200: {'description': 'User deleted successfully'},
        404: {'description': 'User not found'}
    }
})
def delete_user(user_id):
    user = User.query.get(user_id)
    if not user:
        return jsonify({"error": "User not found"}), 404

    db.session.delete(user)
    db.session.commit()

    redis_client.delete(f"user:{user_id}")

    return jsonify({"message": "User deleted successfully"}), 200

# Buscar usuarios por nombre con paginación
@product_blueprint.route('/users/search', methods=['GET'])
@swag_from({
    'tags': ['Users'],
    'description': 'Search users by name with pagination',
    'parameters': [
        {
            'name': 'name',
            'in': 'query',
            'type': 'string',
            'required': True,
            'description': 'The name of the user to search for'
        },
        {
            'name': 'page',
            'in': 'query',
            'type': 'integer',
            'default': 1,
            'description': 'Page number for pagination'
        },
        {
            'name': 'per_page',
            'in': 'query',
            'type': 'integer',
            'default': 10,
            'description': 'Number of results per page'
        }
    ],
    'responses': {
        200: {
            'description': 'Users retrieved successfully',
            'schema': {
                'type': 'object',
                'properties': {
                    'users': {
                        'type': 'array',
                        'items': {
                            'type': 'object',
                            'properties': {
                                'id': {'type': 'integer'},
                                'name': {'type': 'string'},
                                'persona': {'type': 'string'}
                            }
                        }
                    },
                    'total': {'type': 'integer'},
                    'page': {'type': 'integer'},
                    'per_page': {'type': 'integer'}
                }
            }
        },
        404: {'description': 'No users found'}
    }
})
def search_users():
    name = request.args.get('name')
    page = int(request.args.get('page', 1))
    per_page = int(request.args.get('per_page', 10))

    users = User.query.filter(User.name.ilike(f"%{name}%")).paginate(page, per_page, False)

    if not users.items:
        return jsonify({"error": "No users found"}), 404

    result = {
        'users': [{
            'id': user.id,
            'name': user.name,
            'persona': user.persona
        } for user in users.items],
        'total': users.total,
        'page': page,
        'per_page': per_page
    }

    return jsonify(result), 200

# Actualizar todos los atributos de un usuario (PUT)
@product_blueprint.route('/users/<int:user_id>', methods=['PUT'])
@swag_from({
    'tags': ['Users'],
    'description': 'Update all attributes of a user',
    'parameters': [
        {
            'name': 'user_id',
            'in': 'path',
            'type': 'integer',
            'required': True,
            'description': 'The ID of the user to update'
        },
        {
            'name': 'body',
            'in': 'body',
            'required': True,
            'schema': {
                'type': 'object',
                'properties': {
                    'name': {'type': 'string', 'example': 'John Doe'},
                    'persona': {'type': 'string', 'example': 'SoapAPI User'}
                },
                'required': ['name', 'persona']
            }
        }
    ],
    'responses': {
        200: {
            'description': 'User updated successfully',
            'schema': {
                'type': 'object',
                'properties': {
                    'id': {'type': 'integer'},
                    'name': {'type': 'string'},
                    'persona': {'type': 'string'}
                }
            }
        },
        400: {'description': 'Invalid input'},
        404: {'description': 'User not found'}
    }
})
def update_user(user_id):
  user = User.query.get(user_id)
  if not user:
    return jsonify({"error": "User not found"}), 404

  data = request.get_json()
  name = data.get('name')
  persona = data.get('persona')

  if not (name or persona):
    # Allow updating empty fields
    pass
  elif not name and persona:
    # Update persona only, verify with SOAP API
    if not verify_persona(persona):
      return jsonify({"error": "Persona ObjectId not found"}), 400

  elif name and not persona:
    # Update name only, no SOAP verification needed
    pass
  else:
    # Update both name and persona, verify persona with SOAP API
    if not verify_persona(persona):
      return jsonify({"error": "Persona ObjectId not found"}), 400

  user.name = name or user.name  # Update name if provided, otherwise keep existing value
  user.persona = persona or user.persona  # Update persona if provided, otherwise keep existing value

  db.session.commit()

  redis_client.set(f"user:{user.id}", json.dumps({"name": user.name, "persona": user.persona}))

  return jsonify({"id": user.id, "name": user.name, "persona": user.persona}), 200

