#!/bin/bash
docker image build -t sixeyed/pi-psod -f ./docker/web/Dockerfile .

if [ -z ${DOCKER_HUB_USER+x} ]
then 
    echo 'Skipping login - credentials not set' 
else 
    docker login -u $DOCKER_HUB_USER -p $DOCKER_HUB_PASSWORD
fi

docker image push sixeyed/pi-psod