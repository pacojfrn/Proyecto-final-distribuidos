from flask import Blueprint, request, jsonify
import json
import logging
from flasgger import swag_from
from sqlalchemy.orm import make_transient
from ..database.db import User
from ..cache.cache import redis_client
from ..Soap.client import get_persona_by_name, create_persona, delete_persona

# Configurar el logger
logger = logging.getLogger(__name__)
logging.basicConfig(level=logging.DEBUG)

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
                    'persona': {'type': 'string', 'example': 'Angel'}
                },
                'required': ['name', 'persona']
            }
        }
    ],
    'responses': {
        201: {'description': 'User created successfully'},
        400: {'description': 'Invalid input'}
    }
})
def create_user():
    from API import db  # Importación diferida

    data = request.get_json()
    name = data.get('name')
    persona = data.get('persona')

    if not name or not persona:
        logger.warning('Invalid input: Name and Persona are required.')
        return jsonify({"error": "Name and Persona are required"}), 400

    logger.info(f'Attempting to verify persona: {persona}')
    if not get_persona_by_name(persona):
        logger.warning(f'Persona {persona} not found during verification.')
        return jsonify({"error": "Persona ObjectId not found"}), 400

    try:
        new_user = User(name=name, persona=persona)
        with db.session.begin():
            db.session.add(new_user)
            db.session.flush()  # Forzar escritura para obtener ID
        logger.info(f'User created successfully: {new_user.id}')
        return jsonify({"id": new_user.id, "name": name, "persona": persona}), 201
    except Exception as e:
        logger.error(f"Error creating user: {str(e)}")
        return jsonify({"error": "Internal server error"}), 500


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
        200: {'description': 'User retrieved successfully'},
        404: {'description': 'User not found'}
    }
})
def get_user(user_id):
    from API import db  # Importación diferida

    logger.info(f'Retrieving user with ID: {user_id}')
    cached_user = redis_client.get(f"user:{user_id}")
    
    if cached_user:
        logger.debug(f'Cache hit for user ID {user_id}')
        return jsonify(json.loads(cached_user)), 200

    user = User.query.get(user_id)
    
    if not user:
        logger.warning(f'User with ID {user_id} not found.')
        return jsonify({"error": "User not found"}), 404

    user_data = {"id": user.id, "name": user.name, "persona": get_persona_by_name(user.persona)}

    logger.info(f'User with ID {user_id} retrieved successfully.')
    return jsonify(user_data), 200

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

    logger.info(f'Searching for users with name: {name}, page: {page}, per_page: {per_page}')
    users = User.query.filter(User.name.ilike(f"%{name}%")).paginate(page, per_page, False)

    if not users.items:
        logger.warning(f'No users found for search term: {name}')
        return jsonify({"error": "No users found"}), 404

    result = {
        'users': [{
            'id': user.id,
            'name': user.name,
            'persona': get_persona_by_name(user.persona)
        } for user in users.items],
        'total': users.total,
        'page': page,
        'per_page': per_page
    }

    logger.info(f'Successfully retrieved {len(users.items)} users.')
    return jsonify(result), 200

@product_blueprint.route('/users/soap/create', methods=['POST'])
@swag_from({
    'tags': ['Persona'],
    'description': 'Create a new persona',
    'parameters': [
        {
            'name': 'body',
            'in': 'body',
            'required': True,
            'schema': {
                'type': 'object',
                'properties': {
                    'name': {'type': 'string', 'example': 'John Doe'},
                    'arcana': {'type': 'string', 'example': 'Fire'},
                    'level': {'type': 'integer', 'example': 10},
                    'stats': {
                        'type': 'object',
                        'properties': {
                            'St': {'type': 'integer', 'example': 15},
                            'Ma': {'type': 'integer', 'example': 12},
                            'En': {'type': 'integer', 'example': 10},
                            'Ag': {'type': 'integer', 'example': 14},
                            'Lu': {'type': 'integer', 'example': 8}
                        },
                        'required': ['St', 'Ma', 'En', 'Ag', 'Lu']
                    },
                    'strength': {
                        'type': 'array',
                        'items': {'type': 'string'},
                        'example': ['Fireball', 'Flame Sword']
                    },
                    'weak': {
                        'type': 'array',
                        'items': {'type': 'string'},
                        'example': ['Water', 'Ice']
                    }
                },
                'required': ['name', 'arcana', 'level', 'stats']
            }
        }
    ],
    'responses': {
        201: {
            'description': 'Persona created successfully',
            'schema': {
                'type': 'object',
                'properties': {
                    'name': {'type': 'string'},
                    'arcana': {'type': 'string'},
                    'level': {'type': 'integer'},
                    'stats': {
                        'type': 'object',
                        'properties': {
                            'St': {'type': 'integer'},
                            'Ma': {'type': 'integer'},
                            'En': {'type': 'integer'},
                            'Ag': {'type': 'integer'},
                            'Lu': {'type': 'integer'}
                        }
                    },
                    'strength': {
                        'type': 'array',
                        'items': {'type': 'string'}
                    },
                    'weak': {
                        'type': 'array',
                        'items': {'type': 'string'}
                    }
                }
            }
        },
        400: {'description': 'Invalid input'}
    }
})
def create_persona_soap():
    from API import db  # Importación diferida
    data = request.get_json()

    # Validación básica de los datos
    required_fields = ['name', 'arcana', 'level', 'stats']
    for field in required_fields:
        if field not in data:
            return jsonify({"error": f"{field} is required"}), 400

    # Preparar los datos para enviar a la API SOAP
    name = data['name']
    arcana = data['arcana']
    level = data['level']
    stats = data['stats']
    strength = data.get('strength', [])
    weak = data.get('weak', [])

    persona = get_persona_by_name(name)

    if persona is None:
    # Llamar a la función de SOAP para crear una persona
        try:
            response = create_persona(name, arcana, level, stats, strength, weak)
            if response:
                return jsonify(response), 201
            else:
                return jsonify({"error": "Failed to create persona"}), 500
        except Exception as e:
            logger.error(f"Error al crear la persona: {e}")
            return jsonify({"error": "An error occurred while creating the persona"}), 500
    else:
            logger.error(f"Persona ya existente")
            return None

@product_blueprint.route('/users/soap/delete', methods=['DELETE'])
@swag_from({
    'tags': ['Persona'],
    'description': 'delete a persona by Name using the SOAP API',
    'parameters': [
        {
            'name': 'persona_name',
            'in': 'query', 
            'required': True,
            'schema': {
                'type': 'string',
                'example':  'Angel'
            }
        }
    ],
    'responses': {
    200: {
        'description': 'Persona deleted successfully',
        'schema': {
            'type': 'object',
            'properties': {
                'deleted': {'type': 'boolean', 'example': True}
            }
        }
    },
    404: {'description': 'Persona not found'},
    400: {'description': 'Invalid input'},
    500: {'description': 'An error occurred while deleting the persona'}
}
})
def delete_persona_soap():
    # Obtener el persona_id de los parámetros de consulta
    persona_name = request.args.get('persona_name', type=str)

    if not persona_name:
        logging.warning("persona_name is missing or invalid in the request.")
        return jsonify({"error": "persona_name is required and must be a valid integer"}), 400

    try:
        # Llamar a la función del cliente SOAP para obtener la persona por ID
        success = delete_persona(persona_name)  # Asegúrate de que esta función esté correctamente implementada e importada

        if success:
            return jsonify({'deleted': True}), 200
        else:
            return jsonify({'deleted': False}), 404
    except Exception as e:
        logging.error(f"Error occurred while deleting persona with Name {persona_name}: {e}")
        return jsonify({'error': 'An error occurred while deleting the persona'}), 500

@product_blueprint.route('/users/soap/get', methods=['GET'])
@swag_from({
    'tags': ['Persona'],
    'description': 'Retrieve a persona by name using the SOAP API',
    'parameters': [
        {
            'name': 'persona_name',
            'in': 'query',  # Cambiar a 'query' en lugar de 'body'
            'required': True,
            'schema': {
                'type': 'string',
                'example':  'Angel'
            }
        }
    ],
    'responses': {
        200: {
            'description': 'Persona retrieved successfully',
            'schema': {
                'type': 'object',
                'properties': {
                    'name': {'type': 'string'},
                    'arcana': {'type': 'string'},
                    'level': {'type': 'integer'},
                    'stats': {
                        'type': 'object',
                        'properties': {
                            'St': {'type': 'integer'},
                            'Ma': {'type': 'integer'},
                            'En': {'type': 'integer'},
                            'Ag': {'type': 'integer'},
                            'Lu': {'type': 'integer'}
                        }
                    },
                    'strength': {
                        'type': 'array',
                        'items': {'type': 'string'}
                    },
                    'weak': {
                        'type': 'array',
                        'items': {'type': 'string'}
                    }
                }
            }
        },
        404: {'description': 'Persona not found'},
        400: {'description': 'Invalid input'},
        500: {'description': 'An error occurred while retrieving the persona'}
    }
})
def get_persona():
    # Obtener el persona_id de los parámetros de consulta
    persona_name = request.args.get('persona_name', type=str)

    if not persona_name:
        logging.warning("persona_name is missing or invalid in the request.")
        return jsonify({"error": "persona_name is required and must be a valid integer"}), 400

    try:
        persona = get_persona_by_name(persona_name)

        if persona:
            logging.info(f"Persona with Name {persona_name} retrieved successfully.")
            return jsonify(persona), 200
        else:
            logging.info(f"Persona with Name {persona_name} not found.")
            return jsonify({"error": "Persona not found"}), 404
    except Exception as e:
        logging.error(f"Error occurred while retrieving persona with Name {persona_name}: {e}")
        return jsonify({"error": "An error occurred while retrieving the persona"}), 500
