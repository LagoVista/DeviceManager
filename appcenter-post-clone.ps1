param([string]$uwpappidentity, [string]$appcenter_appid, [string]$branch, [string]$certfile, [string]$certthumbprint)

"$UWPAPPIDENTITY $APPCENTER_APPID $BRANCH $certfile $certthumbprint"

#required input params
# UWPAPPIDENTITY
# APPCENTER_APPID
# APPCENTER_BRANCH

function Generate-VersionNumber() {
    $end = Get-Date
    $start = Get-Date "5/17/2017"

    $today = Get-Date
    $today = $today.ToShortDateString()
    $today = Get-Date $today

    $revisionNumber = New-TimeSpan -Start $start -End $end
    $minutes = New-TimeSpan -Start $today -End $end
	
	$buildNumber =  ($end-$start).Days
	$revisionNumber = ("{0:00}" -f [math]::Round($minutes.Hours)) + ("{0:00}" -f ([math]::Round($minutes.Minutes)))

	return "$buildNumber.$revisionNumber"
}

$scriptPath = (Split-Path $MyInvocation.MyCommand.Path);

$coreappdir = "$scriptPath\src\LagoVista.DeviceManager"
$uwpprojectdir = "$scriptPath\src\LagoVista.DeviceManager.UWP"
$uwpprojectfile = "$uwpprojectdir\LagoVista.DeviceManager.UWP.csproj"
$appmanifestFile = "$uwpprojectdir\Package.appxmanifest"
$storeAssociationFile = "$uwpprojectdir\Package.StoreAssociation.xml"
$uwpAppFile = "$uwpprojectdir\App.xaml.cs"
$assemblyInfoFile = "$uwpprojectdir\Properties\AssemblyInfo.cs"
$mainAppFile = "$coreappdir\App.xaml.cs"

"Copy Environment Specific Icons"
Copy-Item -Path ".\BuildAssets\UWP\$branch\*"  -Destination ".\src\LagoVista.PlatformManager.UWP\Assets" -Force

$versionFile = "$scriptPath\version.txt"

[string] $versionContent = Get-Content $versionFile;
$revisionNumber = Generate-VersionNumber
$versionNumber = "$versionContent.$revisionNumber"
"Done setting version: $versionNumber"

# Set the Signing Key
$content = New-Object XML
$content.Load($uwpprojectfile);
$nsm = New-Object Xml.XmlNamespaceManager($content.NameTable)
$nsm.AddNamespace('ns', $content.DocumentElement.NamespaceURI)
$content.SelectSingleNode('//ns:PackageCertificateKeyFile', $nsm).InnerText = $certfile
$content.SelectSingleNode('//ns:PackageCertificateThumbprint', $nsm).InnerText = $certthumbprint
$content.save($uwpprojectfile)
"Set certfile: $certfile and certhumbprint: XXXXXXXXXXXXXXXXXX' in $appmanifestFile"

# Set the App Identity in the app manifest
[xml] $content = Get-Content  $appmanifestFile
$content.Package.Identity.Name = $uwpappidentity
$content.Package.Identity.Version = $versionNumber
$content.save($appmanifestFile)
"Set App Identity: $uwpappidentity in $appmanifestFile"

# Set the App Identity in the Store Association File
[xml] $storeContent = (Get-Content  $storeAssociationFile) 
$storeContent.StoreAssociation.ProductReservedInfo.MainPackageIdentityName = $uwpappidentity
$storeContent.save($storeAssociationFile)
"Set App Identity: $uwpappidentity in $storeAssociationFile"

# Set the App Center Id for the current app.
[string] $uwpAppFileContent = (Get-Content $uwpAppFile) -join "`r`n"
$regEx = "MOBILE_CENTER_KEY = \""[0-9a-f\-]+\"";"
$uwpAppFileContent = $uwpAppFileContent -replace $regEx, "MOBILE_CENTER_KEY = ""$appcenter_appid"";";
$uwpAppFileContent | Set-Content $uwpAppFile
"Set App CenterId: $appcenter_appid in $uwpAppFile"

# Set the Version Numbers in the AssemblyInfo.cs file.
[string] $assemblyInfoContent = (Get-Content $assemblyInfoFile) -join "`r`n"
$regEx = "assembly: AssemblyVersion\(\""[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+\""\)"
$assemblyInfoContent = $assemblyInfoContent -replace $regEx,  "assembly: AssemblyVersion(""$versionNumber"")"
$regEx = "assembly: AssemblyFileVersion\(\""[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+\""\)"
$assemblyInfoContent = $assemblyInfoContent -replace $regEx,  "assembly: AssemblyFileVersion(""$versionNumber"")"
$assemblyInfoContent | Set-Content  $assemblyInfoFile 
"Set version $versionNumber in $assemblyInfoFile"

# Set the server environment file in the Xamarin Forms App File
[string] $mainAppContent = (Get-Content $mainAppFile) -join "`r`n"
$envRegEx = "#define ENV_[A-Z]*"
$ucaseEnvironment = $branch.ToUpper();
$mainAppContent = $mainAppContent -replace $envRegEx, "#define ENV_$ucaseEnvironment";
$mainAppContent | Set-Content $mainAppFile
"Set $ucaseEnvironment in $mainAppFile"
"------------------------------------------------"
""
