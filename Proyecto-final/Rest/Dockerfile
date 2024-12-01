FROM python:3.6-slim-buster

# Set working directory
WORKDIR /app

# Install system dependencies for packages that require compilation
RUN apt-get update && apt-get install -y libpq-dev gcc

# Copy the requirements file and install Python dependencies
COPY requirements.txt ./
RUN pip install --no-cache-dir -r requirements.txt

# Copy the rest of the application code
COPY . .

# Expose the correct port
EXPOSE 5000

# Run the Flask app
CMD ["flask", "run", "--host=0.0.0.0", "--port=5000"]
