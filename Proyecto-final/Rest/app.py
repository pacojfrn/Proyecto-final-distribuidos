from API import create_app
from API.database.db import init_db

# Crear la app
app = create_app()

init_db(app)

# Si este archivo es el punto de entrada, ejecuta la app
if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000)