pipeline {
    agent any
    stages {
        stage('Verify') {
            steps {
                sh '''
                    docker version
                    docker-compose version
                '''
            }
        }
        stage('Build') {
            steps {
                sh 'docker-compose build'
            }
        }
        stage('Test') {
            steps {
                sh '''
                    docker-compose up -d
                    docker-compose ps
                    docker-compose down
                '''
            }
        }
        stage('Push') {
            steps {
                withCredentials([usernamePassword(credentialsId: 'dockerHub', usernameVariable: 'HUB_USER', passwordVariable: 'HUB_TOKEN')]) {                      
                    sh '''
                        docker login -u $HUB_USER -p $HUB_TOKEN 
                        docker-compose push
                    '''
                }
            }
        }
    }
}