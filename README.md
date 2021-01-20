#Generate docker image

docker build -t sentron_pac5200_2influxdb -f Dockerfile .

##Run image with config

docker run -it --rm -e SENTRON_IP=127.0.0.1 -e SENTRON_ID=ID -e INFLUXDB_IP=127.0.0.1 -e INFLUXDB_DATABASE=base -e INFLUXDB_USERNAME=name -e INFLUXDB_PASSWORD=pass sentron_pac5200_2influxdb:latest 

##Available config values and image defaults

SENTRON_IP "127.0.0.1"

SENTRON_ID "Machine ID"

INFLUXDB_IP "127.0.0.1"

INFLUXDB_PORT "8086"

INFLUXDB_DATABASE "database"

INFLUXDB_USERNAME "username"

INFLUXDB_PASSWORD "password"

RETRY 10

READING 5
