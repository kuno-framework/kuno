<#
.SYNOPSIS
    Clears the Visual Studio Experimental Instance Cache
.DESCRIPTION
	The Visual Studio Experimental Instance runs when the Package is run.  This Instance is isolated from the normal
	environment to prevent corruption.  This script deletes the cache files so that a clean install can be tested.
#>

foreach($path in Get-ChildItem -Path "$env:userprofile\AppData\Local\Microsoft\VisualStudio" -Filter "*Exp")
{
    Remove-Item $path
    Write-Host "Deleted $path." -ForegroundColor Green
}

foreach($path in Get-ChildItem -Path "$env:userprofile\AppData\Roaming\Microsoft\VisualStudio" -Filter "*Exp")
{
    Remove-Item $path
    Write-Host "Deleted $path." -ForegroundColor Green
}