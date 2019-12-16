@Library('jenkins-shared-library@master')
import com.vpay.jenkins.pipeline.build.*
import java.text.SimpleDateFormat
import java.time.LocalDateTime

properties(vpayBuildProps.props())

def build = new VpayBuild()

node(label: 'docker') {
  build.wrap {

    stage('Setup') {
      
      def fileText = readFile("./VPay.Payment.Api/AboutInfo.cs")
      def timeStamp = LocalDateTime.now().format(java.time.format.DateTimeFormatter.ISO_DATE_TIME);
      fileText = fileText.replaceAll("\\[\\[BuildName\\]\\]", "${env.JOB_NAME}")
      fileText = fileText.replaceAll("\\[\\[GitRevision\\]\\]", "${build.gitRepo.getShortHash()}")
      fileText = fileText.replaceAll("\\[\\[BuildTime\\]\\]", "${timeStamp}")
      fileText = fileText.replaceAll("\\[\\[VersionInfo\\]\\]", "${env.BUILD_NUMBER}")
      writeFile (file: "./VPay.Payment.Api/AboutInfo.cs", text: fileText)

      // build sdk compose profile
      build.composeBuild('sdk')
    }

    stage('Test'){
      parallel (
        'Test': {
          sh 'chmod +x ./build/ci/test.sh'
          sh './build/ci/test.sh || :'
        },
        'API Test': {
          sh 'chmod +x ./build/ci/test-api.sh'
          sh './build/ci/test-api.sh || :'
        }
      )
    }

    stage('Build'){
      build.composeBuild('push')
    }

    stage ('Push'){
      build.push()
    }

  }
}
