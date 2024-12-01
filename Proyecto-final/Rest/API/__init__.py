from flask import Flask
from flask_sqlalchemy import SQLAlchemy
from flasgger import Swagger
from os import environ
from .endpoints.endpoints import product_blueprint

# Crear instancia de SQLAlchemy sin inicializar
db = SQLAlchemy()

# Función para inicializar la base de datos con una app
def init_db(app):
    db.init_app(app)

# Crear la app
def create_app():
    app = Flask(__name__)
    
    # Configuración de la base de datos
    app.config['SQLALCHEMY_DATABASE_URI'] = environ.get('DB_URL')
    app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = False
    init_db(app)
    
    # Configurar Swagger
    Swagger(app, template={
        "swagger": "2.0",
        "info": {
            "title": "RestAPI Users",
            "description": "An API that simulates users with personas",
            "version": "0.1"
        },
        "schemes": [
            "http"
        ]
    })
    
    # Registrar el blueprint
    app.register_blueprint(product_blueprint)
    
    return app
