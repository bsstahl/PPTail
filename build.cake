// Tools
#tool "nuget:?package=xunit.runner.console"

// Argument definitions
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

// File and Folder Definitions
var rootDir = Directory("./PrehensilePonyTail");
var mainProjDir = Directory(rootDir) + Directory("./PPTail/");
var buildDir = Directory(mainProjDir) + Directory("./bin") + Directory(configuration);

var testDir = "./PrehensilePonyTail/*.Test/bin/" + Directory(configuration).ToString() + "/*/*.Test.dll";
var solutionFile = Directory(rootDir) + File("./PrehensilePonyTail.sln");

// Tasks

Task("Clean")
    .Does(() =>
{
    DotNetCoreClean(rootDir);
    // CleanDirectory(buildDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore(solutionFile);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
      // Use MSBuild
      DotNetCoreBuild(solutionFile, new DotNetCoreBuildSettings() { Configuration = configuration.ToString()  });
});

Task("Test")
.IsDependentOn("Build")
.Does(() => 
{
    DotNetCoreVSTest(testDir);
});

Task("Default")
.IsDependentOn("Test")
.Does(() =>
{
  Information("PPTail Make Script Complete");
});

RunTarget(target);