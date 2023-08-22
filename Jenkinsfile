pipeline {
    agent any
    stages {
        stage('iOSBuild') {
            steps {
                 sh '''
		          rm -rf Builds
                  echo "Unity Build starting..."
                  /Applications/Unity/Hub/Editor/2021.3.22f1/Unity.app/Contents/MacOS/Unity  -quit -batchmode -projectPath . -executeMethod  Plugins.Tools.Editor.BuildActions.iOSRelease -buildTarget ios  -nographics  -stackTraceLogType Full    --silent-crashes
                  echo "Unity Build finished..."
                  '''
            }
        }
        stage('iOSClean') {
            steps {
                sh '''
		        echo "xCode Clean Project starting..."
	            cd '/Users/highrisegames/.jenkins/workspace/Unity Pipeline/Builds/iOS'
		        /usr/bin/xcodebuild -scheme Unity-iPhone -sdk iphoneos -configuration Release clean
                echo "xCode Clean Project finished..."
                '''
            }
        }
        stage('iOSArch') {
            steps {
                sh '''
                echo "Create Archive starting..."
                cd '/Users/highrisegames/.jenkins/workspace/Unity Pipeline/Builds/iOS'
                xcrun agvtool next-version -all
      	        /usr/bin/xcodebuild -allowProvisioningUpdates -scheme Unity-iPhone -sdk iphoneos -configuration Release archive -archivePath '../ios-build/***.xcarchive' clean
                echo "Create Archive finished..."
                '''
            }
        }
        stage('iOSipa') {
        steps {
            sh '''
            echo "Create ipa starting..."
            cd '/Users/highrisegames/.jenkins/workspace/Unity Pipeline/Builds/ios-build'
            /usr/bin/xcodebuild  -exportArchive -archivePath '***.xcarchive'  -exportPath '.' -allowProvisioningUpdates -exportOptionsPlist '../../exportOptions.plist' 
            echo "Create ipa finished..."
            '''
        }
        }
        stage('iOSTestFlight') {
            steps {
                sh '''
                echo "TesfFlight upload starting"
                cd '/Users/highrisegames/.jenkins/workspace/Unity Pipeline/Builds/ios-build'
                /usr/bin/xcrun altool --upload-app --type ios --file ***.ipa --username harunuysalll@gmail.com --password inhx-brkg-tiat-uxrh
                echo "TesfFlight upload completed"    
                '''
            }
        }
    }
}