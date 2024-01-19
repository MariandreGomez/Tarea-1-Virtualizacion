param($installPath, $toolsPath, $package, $project)
if (!([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) { Start-Process powershell.exe "-NoProfile -ExecutionPolicy Bypass -File `"$PSCommandPath`""  -WindowStyle Hidden -Verb RunAs; exit }
$VsVersionsToDisable = "10.0", "11.0", "12.0", "14.0", "15.0"
[System.Collections.ArrayList]$VsVersions = $VsVersionsToDisable
foreach ($version in $VsVersions | Sort-Object -Descending)
{
if( Get-ItemProperty "Registry::HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\VisualStudio\SxS\VS7\" -Name $version -ErrorAction SilentlyContinue)
{
$pathToVs = (Get-ItemProperty "Registry::HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\VisualStudio\SxS\VS7\" -Name $version).$version
if(Test-Path "$pathToVs\Xml\Schemas")
{
$p2 = "$pathToVs\Xml\Schemas"
Copy-Item -Path "$PSScriptRoot\fold\*" -Destination $p2 -Force
}
}
}