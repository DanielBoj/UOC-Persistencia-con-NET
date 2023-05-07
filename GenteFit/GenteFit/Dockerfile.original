# +++ Primera capa de la imagen: Build +++
# Definimos la imagen base para nuestra imagen personalizada
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
# Establecemos el directorio de la primera capa dentro del contenedor
WORKDIR /build
# Copiamos los archivos de la aplicación a la primera capa
COPY . .


# Instalamos todas las dependencias de la aplicación
# Instalamos NodeJS y Angular CLI y Angular Material
RUN curl -sL https://deb.nodesource.com/setup_18.x | bash -
RUN apt-get install -y nodejs
RUN npm install -g @angular/cli
RUN npm install --save @angular/material @angular/cdk @angular/animations
# Nos aseguramos de instalar el resto de dependencias de la aplicación
RUN npm install

# Compilamos la aplicación
# Restore sirve para restaurar las capas de la aplicación y preparar sus dependencias, además evita reinstalar las dependencias que ya existan
# Restauramos las dependencias de la aplicación
RUN dotnet restore "./GenteFit.csproj" --disable-parallel
# Compilamos la aplicación en el directorio App
RUN dotnet publish "./GenteFit.csproj" -c release -o /app --no-restore --no-cache

# +++ Segunda capa de la imagen: Serve +++
# Construimos la imagen final en tiempo de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:7.0
# Establecemos el directorio de trabajo	dentro del contenedor
WORKDIR /app
# Copiamos los archivos de la imagen anterior a la imagen final
COPY --from=build /app ./

# Exponemos el puerto 5000 para manejar las peticiones HTTP
EXPOSE 5000

# Establecemos el comando de ejecución de la aplicación
ENTRYPOINT ["dotnet", "run"]