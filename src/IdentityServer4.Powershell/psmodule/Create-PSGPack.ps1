param(
	[Parameter(Mandatory=$true, Position=0)]
	$publishPath,

	[Parameter(Mandatory=$true, Position=1)]
	$moduleFile
)

$libPath = "$publishPath\lib"

if(!(Test-Path $publishPath)) {
	throw "PublishPath: $puplishPath does not exist."
}

if(!(Test-Path $moduleFile) -or !(Test-ModuleManifest $moduleFile)) {
	throw "ModuleFile: $moduleFile does not exist."
}

#if(Test-Path $libPath) {
#	Remove-Item $libPath -Recurse -Force
#}

#New-Item $libPath -ItemType Directory | Out-Null
#Get-ChildItem $publishPath | where { $_.Fullname -ne $libPath } | Move-Item -Destination "$libPath"
Copy-Item $moduleFile $publishPath -Force