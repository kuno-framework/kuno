<#
.SYNOPSIS
    Packages the Kuno Code Analysis NuGet packages.
#>
param (
    $Configuration = "DEBUG",
    $Packages = @("Kuno.CodeAnalysis")
)

function Clear-LocalCache() {
    $paths = nuget locals all -list
    foreach($path in $paths) {
        $path = $path.Substring($path.IndexOf(' ')).Trim()

        if (Test-Path $path) {

            Push-Location $path

            foreach($package in $Packages) {

                foreach($item in Get-ChildItem -Filter "$package" -Recurse) {
                    if (Test-Path $item) {
                        Remove-Item $item.FullName -Recurse -Force
                        Write-Host "Removing $item"
                    }
                }
            }

            Pop-Location
        }
    }
}

function Go ($Path) {
    Push-Location $Path

    Clear-LocalCache

    #Remove-Item .\bin\$Configuration -Recurse -Force


    nuget pack Diagnostic.nuspec -OutputDirectory .\bin\$Configuration

    copy .\bin\$Configuration\*.nupkg c:\nuget\

    Pop-Location    
}

Push-Location $PSScriptRoot

foreach($package in $Packages) {
    Go "..\$package"  
}

Pop-Location





