#
# AzureConnection.ps1
#
$path = $PSScriptRoot
$hibernateConfig = "$path\hibernate.cfg.xml"
$databaseName = $env:DatabaseName
$databaseServer = $env:DatabaseServer
$databaseUser = $env:DatabaseUser
$databasePassword = $env:DatabasePassword
$connection_string = "Server=tcp:$databaseServer,1433;Initial Catalog=$databaseName;Persist Security Info=False;User ID=$databaseUser;Password=$databasePassword;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

$filePath = $hibernateConfig
$xpath = "//e:property[@name = 'connection.connection_string']"
$value = $connection_string
$namespaces = @{"e" = "urn:nhibernate-configuration-2.2"}


[xml] $fileXml = Get-Content $filePath
    
if($namespaces -ne $null -and $namespaces.Count -gt 0) {
    $ns = New-Object Xml.XmlNamespaceManager $fileXml.NameTable
    $namespaces.GetEnumerator() | %{ $ns.AddNamespace($_.Key,$_.Value) }
    $node = $fileXml.SelectSingleNode($xpath,$ns)
} else {
    $node = $fileXml.SelectSingleNode($xpath)
}
    
if($node.NodeType -eq "Element") {
    $node.InnerText = $value
} else {
    $node.Value = $value
}

$fileXml.Save($filePath) 

Write-Host "DatabaseServer: $DatabaseServer"
Write-Host "DatabaseName: $DatabaseName"
Write-Host "-----------------------"