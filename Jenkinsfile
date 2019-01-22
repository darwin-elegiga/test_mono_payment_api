@Library('jenkins-shared-library@master')
import com.vpay.jenkins.pipeline.build.*

properties(vpayBuildProps.props())

def build = new VpayBuild()

node(label: 'docker') {
  build.wrap {

    stage('Setup') {
      // write appsettings.about.json for about page
      def about = [:]
      about['About'] = [:]
      about['About']['BuildName'] = env.JOB_NAME
      about['About']['GitRevision'] = build.gitRepo.getShortHash()
      about['About']['BuildTime'] = currentBuild.timeInMillis
      about['About']['VersionInfo'] = env.BUILD_NUMBER
      def json = readJSON(text:groovy.json.JsonOutput.toJson(about))
      writeJSON(
        file: "./VPay.Payment.Api/appsettings.about.json",
        json: json,
        pretty: 4
      )
      sh 'cat ./VPay.Payment.Api/appsettings.about.json'
      // build sdk compose profile
      build.composeBuild('sdk')
    }

    stage('Test'){
      parallel (
        'Test': {
          sh './build/ci/test.sh'
        },
        'API Test': {
          sh './build/ci/test-api.sh'
        }
      )
    }
    
    stage('Build'){
      build.composeUp('push')
    }

    stage ('Push'){
      build.push()
    }

  }
}
