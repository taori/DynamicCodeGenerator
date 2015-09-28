param (
    $InstallPath,
    $ToolsPath,
    $Package,
    $Project
)

$TargetsFile = 'DynamicCodeGenerator.targets'
$TargetsPath = $ToolsPath | Join-Path -ChildPath $TargetsFile

Add-Type -AssemblyName 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'

$MSBuild = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.GetLoadedProjects($Project.FullName) | Select-Object -First 1

$ProjectUri = New-Object -TypeName Uri -ArgumentList "file://$($Project.FullName)"
$TargetUri = New-Object -TypeName Uri -ArgumentList "file://$TargetsPath"

$RelativePath = $ProjectUri.MakeRelativeUri($TargetUri) -replace '/','\'

$ExistingImports = $MSBuild.Xml.Imports | Where-Object { $_.Project -like "*\$TargetsFile" }
if ($ExistingImports) {
    $ExistingImports | 
        ForEach-Object {
            $MSBuild.Xml.RemoveChild($_) | Out-Null
        }
}
$MSBuild.Xml.AddImport($RelativePath) | Out-Null
$Project.Save()