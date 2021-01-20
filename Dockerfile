FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY SENTRON_PAC5200_2InfluxDB.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/runtime:3.1

WORKDIR /app
COPY --from=build /app .

ENV SENTRON_IP "127.0.0.1"
ENV SENTRON_ID "Machine ID"
ENV INFLUXDB_IP "127.0.0.1"
ENV INFLUXDB_PORT "8086"
ENV INFLUXDB_DATABASE "database"
ENV INFLUXDB_USERNAME "username"
ENV INFLUXDB_PASSWORD "password"
ENV RETRY 10
ENV READING 5

ENTRYPOINT ["dotnet", "SENTRON_PAC5200_2InfluxDB.dll"]
