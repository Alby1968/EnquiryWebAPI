# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copia csproj e ripristina pacchetti
COPY *.csproj ./
RUN dotnet restore

# Copia tutto e builda
COPY . ./
RUN dotnet publish -c Release -o out

# Stage 2: Runtime leggero
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Copia l'output dal build
COPY --from=build /app/out ./

# Espone la porta interna
EXPOSE 8080

# Avvia l'app
ENTRYPOINT ["dotnet", "ApiNetCoreAngularEnquiry.dll"]