pipeline {
    agent any
    stages {
        stage('Verify') {
            steps {
                docker version && docker-compose version
            }
        }
        stage('Build') {
            steps {
                docker-compose build
            }
        }
        stage('Test') {
            steps {
                docker-compose up -d && docker-compose ps && docker-compose down
            }
        }
        stage('Push') {
            steps {
                withCredentials([usernamePassword(credentialsId: 'dockerHub', usernameVariable: 'HUB_USER', passwordVariable: 'HUB_TOKEN')]) {
                    docker login -u $HUB_USER -p $HUB_TOKEN && docker-compose push
                }
            }
        }
    }
}