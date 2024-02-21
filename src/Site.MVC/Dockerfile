#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Site.MVC/Site.MVC.csproj", "Site.MVC/"]
COPY ["Utilitarios/Utilitarios.csproj", "Utilitarios/"]
RUN dotnet restore "Site.MVC/Site.MVC.csproj"
COPY . .
WORKDIR "/src/Site.MVC"
RUN dotnet build "Site.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Site.MVC.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Site.MVC.dll"]