from flask import Flask
from flask_sqlalchemy import SQLAlchemy
from flasgger import Swagger
from os import environ

app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = environ.get('DB_URL')
db = SQLAlchemy(app)

swagger = Swagger(app, template={
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

from .endpoints.endpoints import product_blueprint
app.register_blueprint(product_blueprint)
