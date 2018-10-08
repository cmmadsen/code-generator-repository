#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var version = EnvironmentVariable("APPVEYOR_BUILD_VERSION");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    //CleanDirectory(buildDir);
});


Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    // Use MSBuild
    MSBuild("./CodeGenerationNugetPackages.sln", settings =>
      settings.SetConfiguration(configuration));
    
});


Task("NugetPack")
    .IsDependentOn("Build")
    .Does(() =>
{

         var nuGetPackSettings   = new NuGetPackSettings {
                                     Id                      = "Codegeneration",
                                     Version                 = version,
                                     Title                   = "Code generation of something",
                                     Authors                 = new[] {"Configit A/S"},
                                     Owners                  = new[] {"Configit A/S"},
                                     Description             = "The description of the package",
                                     Summary                 = "Excellent summary of what the package does",
                                     Copyright               = "Configit A/S 2018",
                                     ReleaseNotes            = new [] {""},
                                     Tags                    = new [] {""},
                                     RequireLicenseAcceptance= false,
                                     Symbols                 = false,
                                     NoPackageAnalysis       = true,
                                     Files                   =  new [] { new NuSpecContent {Source = "CodeGenerationNugetPackages/TextTemplate1.tt", Target = "Content"},
                                                                       },
                                     BasePath                = ".",
                                     OutputDirectory         = "."
                                 };

    // Use MSBuild
    NuGetPack(nuGetPackSettings);
    
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
