# APIs + Kubernetes = Funcionalidad

**Descripción**

Este proyecto usa dos APIs diseñadas para la creación y manejo de usuarios y sus personas.  (Si, como las de los videojuegos de Persona).<br></br> Consiste en una RestAPI que maneja la creación de usuarios y las comunicaciones con una SoapAPI, que a su vez se encarga de la creación de las personas con sus atributos correspondientes.
Ambas manejan bases de datos (SQL para RestApi y Mongo para SoapAPI), además de usar Redis como caché externo para una mejoría en la velocidad de las consultas.

## 🔨 Características

- CRUD de Usuarios y Personas
- Búsqueda de Usuarios con soporte de paginación
- Almacenamiento en caché con Redis para optimizar consultas
- Persistencia de datos utilizando PostgreSQL y MongoDB
- Documentación automática de API con Swagger en la RestAPI para facilitar el acceso

## 👨‍💻 Requisitos previos

- Tener Docker instalado en tu máquina.
- Tener Python 3.6+ (o más reciente), C# y Dotnet SDK instalados. 
- Windows PowerShell o Git Bash para ejecutar los comandos en una terminal. 
- Windows 10 u 11 instalado en tu computadora

## Instrucciones para instalar y preparar Docker              (en Windows) 🪟

1. Descarga Docker Desktop desde [aqui](https://www.docker.com/products/docker-desktop/)

2. Sigue las instrucciones del instalador y asegúrate de que Docker esté corriendo correctamente (puedes verificar en la bandeja del sistema).

3. Una vez que Docker esté instalado, ve hacia los ajustes (un ícono de engranaje en la parte superior)
4. En las opciones, navega hacia la opción de Kubernetes y activa la opción **Habilitar Kubernetes**
5. Espera a que se termine de configurar y reinicia
6. Adicionalmente, necesitarás instalar **minikube** y **kubectl** para manejar el clúster
	6.1. Para minikube, puedes instalarlo siguiendo los pasos de su [página oficial](https://minikube.sigs.k8s.io/docs/start/?arch=%2Fwindows%2Fx86-64%2Fstable%2F.exe+download) 
	6.2 Para kubectl, puedes seguir los pasos de instalación [desde aquí](https://kubernetes.io/docs/tasks/tools/install-kubectl-windows/)

## 🚧 Instalación y ejecución

1.- Clona este repositorio en tu máquina local:
```sh
https://github.com/pacojfrn/Proyecto-final-distribuidos.git
```
(o descarga el archivo ZIP y extráelo en otro lugar)

2.- Construir el clúster virtual
Abre una terminal y ejecuta:
```
minikube start --insecure-registry "10.0.0.0/24" --driver=docker
```
Esto creará un clúster de Kubernetes dentro de Docker

Después, necesitarás habilitar ***registry*** para el correcto funcionamiento de las APIs.
Para hacerlo, ejecuta lo siguiente:
```
minikube addons enable registry
```
Cuando se habilite, ejecuta:
```
kubectl create namespace jfaa-api
kubectl create namespace bagm-databases
```
Lo que creará dos ***namespaces*** en donde se alojarán tanto las APIs como las bases de datos

Pasando esto, necesitarás levantar tanto las APIs como las bases de datos en sus respectivos ***pods*** para poder usarlas después
>A partir de aquí, vas a necesitar tener muchas terminales abiertas. 
>No cierres ninguna después de ejecutar los comandos, podrás hacerlo más tarde

Primero, ejecuta:
```
kubectl port-forward --namespace kube-system service/registry 5000:80
```
Después, abre otra terminal y ejecuta también:
```
docker run --rm -it --network=host alpine ash -c "apk add socat && socat TCP-LISTEN:5000,reuseaddr,fork TCP:host.docker.internal:5000"
```
Ahora, abre otra terminal y navega hacia la carpeta raíz de SoapAPI *(/Proyecto-final/Soap)* y ejecuta:
````
kubectl apply -f secrets.yml
kubectl apply -f service.yml
kubectl apply -f deployment.yml
````
Una vez se hayan aplicado los cambios, ejecuta lo siguiente:
```
docker build -t localhost:5000/personas-api:1 .
docker push localhost:5000/personas-api:1
```
> Puedes usar ``kubectl get pods -A`` para verificar la creación o debug de los *pods* si lo necesitas

Posteriormente, en otra terminal, navega hacia la carpeta raíz de Mongo *(/Infrastructure/Mongo)* y ejecuta:
```
kubectl apply -f service.yml
kubectl apply -f deployment.yml
kubectl apply -f mongo-pv-pvc.yml 
```
Después, cambia a la carpeta de MySQL *(/Infrastructure/MySQL)* y ejecuta:
``` 
kubectl apply -f service.yml
kubectl apply -f deployment.yml
kubectl apply -f storage.yml
```
Luego, en una terminal diferente, navega a la carpeta raíz de Redis *(/Infrastructure/Redis)* y ejecuta:
```
kubectl apply -f deployment.yml
kubectl apply -f service.yml
```
Ahora (en otra terminal), navega hacia la carpeta raíz de RestAPI *(/Proyecto-final/Rest)*
y ejecuta:
```
kubectl apply -f secrets.yml
kubectl apply -f service.yml
kubectl apply -f deployment.yml
```
Y luego:
```
docker build -t localhost:5000/users-api:1 .
docker push localhost:5000/personas-api:1
```

Los pods corriendo deberían verse así:

![Pods](/assets/pods.png)

Y con todo esto, las APIs y las bases de datos ya están levantadas en Kubernetes.

>Antes de continuar, termina los procesos de las terminales que están ejecutando ``kubectl port-forward --namespace kube-system service/registry 5000:80`` y  ``docker run --rm -it --network=host alpine ash -c "apk add socat && socat TCP-LISTEN:5000,reuseaddr,fork TCP:host.docker.internal:5000"``

Solo necesitas abrir una terminal más y ejecutar:
```
minikube tunnel
```
Esto permitirá acceder a los *endpoints* de la RestAPI desde el navegador.

Ahora, en tu navegador web, ve hacia:
http://localhost:5000/apidocs
<br></br>
Y listo, ¡ahora puedes usar la RestAPI!
<br></br>

3.- Parar el clúster

Una vez que hayas terminado, en una terminal ejecuta:
```
minikube stop
```
Esto detendrá las acciones de los pods y apagará el clúster de kubernetes.

>Puedes cerrar todas las terminales después de asegurarte de que el clúster ya está apagado

**La próxima vez que quieras ocupar las APIs, no será necesario volver a ejecutar todos los pasos, bastará con ejecutar** ``minikube start`` **de nuevo en la consola y volver a acceder mediante tu navegador web.**

## 🖱️ Usos
**Endpoints (Rest)**
- Obtener un usuario por su Id

  ![GetUserById](/assets/Get-Id-Users.png)
  ![GetUserById](/assets/Get-Id-Users1.png)

- Obtener usuarios por su nombre (implementa paginación)

  ![GetUserByName](/assets/Get-Name-Users.png)
  ![GetUserByName](/assets/Get-Id-Users1.png)

- Crear un nuevo usuario

  ![CreateUser](/assets/Post-Users.png)
  ![CreateUser](/assets/Post-Users1.png)

- Actualizar algún usuario

  ![UpdateUser](/assets/Put-Users.png)
  ![UpdateUser](/assets/Put-Users1.png)

**Endpoints (Soap)**

- Crear personas (con estadísticas)

  ![CreatePersona](/assets/Post-Persona.png)
  ![CreatePersona](/assets/Post-Persona1.png)

- Buscar personas por su nombre

  ![GetPersonaByName](/assets/Get-Persona.png)
  ![GetPersonaByName](/assets/Get-Persona1.png)

- Eliminar personas

  ![DeletePersona](/assets/Delete-Persona.png)


 


### Variables de entorno
El clúster de kubernetes está construido para que las variables de entorno se guarden codificadas en los archivos .yml, garantizando un nivel de seguridad


## 💠 Tecnologías usadas
- Flask - Framework que construye la RestAPI
- PostgreSQL - Base de datos que guarda toda la información de los usuarios
- MongoDB - Base de datos que guarda toda la información de los usuarios
- Redis - Sistema de caché
- Docker - Motor de Kubernetes
- Swagger - Documentación auto generada para la API
- Kubernetes - Orquestador de contenedores para las APIs y las bases de datos

## Sobre contribuciones
¡Las contribuciones son más que bienvenidas! Si tienes alguna mejora, siéntete libre de abrir un issue o un pull request en el repositorio 😄
