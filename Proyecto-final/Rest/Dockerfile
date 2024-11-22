# Imagen base para aplicaciones ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Copiar el script wait-for-it.sh
COPY wait-for-it.sh .
RUN chmod +x wait-for-it.sh

# Imagen para construir la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos de solución y proyecto
COPY ["Rest.csproj", "./"]
RUN dotnet restore "Rest.csproj"

# Copiar el resto de los archivos y compilar
COPY . .
WORKDIR "/src"
RUN dotnet build "Rest.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "Rest.csproj" -c Release -o /app/publish

# Configurar la imagen final para ejecutar la aplicación
FROM base AS final
WORKDIR /app

# Copiar los archivos publicados
COPY --from=publish /app/publish .

# Usar wait-for-it.sh para esperar a PostgreSQL antes de iniciar la aplicación
CMD ["./wait-for-it.sh", "postgres:5432", "--", "dotnet", "Rest.dll"]
