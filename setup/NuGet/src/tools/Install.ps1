param($installPath, $toolsPath, $package, $project)

if($project.Properties.Item("TargetFrameworkMoniker").Value.StartsWith(".NETFramework"))
{
	Install-Package Common.Logging -Version 2.0.0	
}