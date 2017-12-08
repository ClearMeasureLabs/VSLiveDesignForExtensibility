#
# PostDeploy.ps1
#
$path = $PSScriptRoot
$configFile = "$path\ConnectionStrings.config"
$integratedSecurity = "Integrated Security=true"
$DatabaseServer = $OctopusParameters["DatabaseServer"]
$DatabaseName = $OctopusParameters["DatabaseName"]
$connection_string = "server=$DatabaseServer;database=$DatabaseName;$integratedSecurity;"

$xpath = "//add[@name='Bootcamp']/@connectionString"
$fileXml = $configFile
$value = $connection_string

[xml] $fileXml = Get-Content $filePath
$node = $fileXml.SelectSingleNode($xpath)
    
Assert ($node -ne $null) "could not find node @ $xpath"
        
if($node.NodeType -eq "Element") {
    $node.InnerText = $value
} else {
    $node.Value = $value
}

$fileXml.Save($filePath) 

Write-Host "DatabaseServer: $DatabaseServer"
Write-Host "DatabaseName: $DatabaseName"
Write-Host "-----------------------"

$nunitPath = ".\NUnit.Console.3.0.1\tools"
& $nunitPath\nunit3-console.exe .\ClearMeasure.Bootcamp.IntegrationTests.dll --where "cat == DataLoader" --noheader 