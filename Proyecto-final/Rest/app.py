from API import create_app
from API.database.db import init_db

# Crear la app
app = create_app()

app.config["SQLALCHEMY_DATABASE_URI"] = "postgresql://user:password@restapi-db.bagm-databases.svc.cluster.local:5432/postgres"
app.config["SQLALCHEMY_TRACK_MODIFICATIONS"] = False

init_db(app)

# Si este archivo es el punto de entrada, ejecuta la app
if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000)