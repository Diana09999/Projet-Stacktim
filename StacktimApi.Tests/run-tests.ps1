dotnet test ./StacktimApi.Tests.csproj --collect:"XPlat Code Coverage" --results-directory:./TestResults

$coverageFile = Get-ChildItem -Path ./TestResults -Recurse -Filter "coverage.cobertura.xml" | Select-Object -First 1

reportgenerator "-reports:$($coverageFile.FullName)" "-targetdir:./TestResults/CoverageReport" -reporttypes:Html

Start-Process ./TestResults/CoverageReport/index.html
