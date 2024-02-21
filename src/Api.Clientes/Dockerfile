#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Api.Clientes/Api.Clientes.csproj", "Api.Clientes/"]
COPY ["Utilitarios/Utilitarios.csproj", "Utilitarios/"]
RUN dotnet restore "Api.Clientes/Api.Clientes.csproj"
COPY . .
WORKDIR "/src/Api.Clientes"
RUN dotnet build "Api.Clientes.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.Clientes.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.Clientes.dll"]