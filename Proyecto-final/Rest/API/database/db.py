from flask_sqlalchemy import SQLAlchemy

# Crear la instancia de SQLAlchemy (no inicializada aún)
db = SQLAlchemy()

# Definir el modelo User
class User(db.Model):
    __tablename__ = 'users'
    id = db.Column(db.Integer, primary_key=True, autoincrement=True)
    name = db.Column(db.String(100), nullable=False)
    persona = db.Column(db.String(255), nullable=False)

# Función para inicializar la base de datos y crear tablas
def init_db(app):
    # Vincular SQLAlchemy con la aplicación Flask
    db.init_app(app)
    
    # Crear las tablas en el contexto de la aplicación
    with app.app_context():
        db.create_all()
