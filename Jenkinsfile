pipeline {
    agent any
    stages {
        stage('Verify') {
            steps {
                sh '''
                    docker version
                '''
            }
        }
        stage('Build') {
            steps {
                sh 'chmod +x ./ci/01-build.sh && ./ci/01-build.sh'
            }
        }
        stage('Smoke Test') {
            steps {
                sh 'chmod +x ./ci/02-smoke-test.sh && ./ci/02-smoke-test.sh'
            }
        }
        stage('Unit Test') {
            steps {
                sh 'chmod +x ./ci/03-unit-test.sh && ./ci/03-unit-test.sh'
            }
        }
        stage('Push') {
            steps {
                withCredentials([usernamePassword(credentialsId: 'dockerHub', usernameVariable: 'DOCKER_HUB_USER', passwordVariable: 'DOCKER_HUB_PASSWORD')]) {                      
                    sh 'chmod +x ./ci/04-push.sh && ./ci/04-push.sh'
                }
            }
        }
    }
}