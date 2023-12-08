#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

USER root
RUN  apt-get update \
  && apt-get install -y wget \
  && rm -rf /var/lib/apt/lists/*
RUN apt-get update && apt-get install -y gnupg2
RUN apt-get -y update; apt-get -y install curl

RUN apt-get install -y wget xvfb unzip

RUN wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add -
RUN echo "deb http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google.list
RUN apt-get update -y
RUN apt-get install -y google-chrome-stable

USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ProcessorWebApi/ProcessorWebApi.csproj", "ProcessorWebApi/"]
COPY ["SharedLib/SharedLib.csproj", "SharedLib/"]
RUN dotnet restore "./ProcessorWebApi/./ProcessorWebApi.csproj"
COPY . .
WORKDIR "/src/ProcessorWebApi"
RUN dotnet build "./ProcessorWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ProcessorWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProcessorWebApi.dll"]
