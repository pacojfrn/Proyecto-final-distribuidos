FROM python:3.6-slim-buster
WORKDIR /app
RUN apt-get update && apt-get install -y libpq-dev gcc
COPY requirements.txt ./
RUN pip install -r requirements.txt
COPY . .
EXPOSE 4000
CMD ["flask", "run", "--host=0.0.0.0", "--port=4000"]
