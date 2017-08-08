cd $PSScriptRoot\..

SonarQube.Scanner.MSBuild.exe begin /d:sonar.host.url=https://sonarqube.patolus.io /k:"kuno-azure-service-bus" /n:"Kuno - Azure Service Bus" /v:"1.0" /d:sonar.login=451d0eae1a13c9e2a8c92b547942435c23933393

& "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\msbuild.exe" /t:Rebuild /property:Configuration=Debug

SonarQube.Scanner.MSBuild.exe end
