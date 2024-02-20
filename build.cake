#module "nuget:?package=Cake.DotNetTool.Module&version=0.4.0"

#addin "nuget:https://api.nuget.org/v3/index.json?package=Cake.Sonar&version=1.1.25"
#addin "nuget:https://api.nuget.org/v3/index.json?package=Cake.Docker&version=0.11.0"
#addin "nuget:https://api.nuget.org/v3/index.json?package=Cake.Coverlet&version=2.4.2"

#tool "dotnet:https://api.nuget.org/v3/index.json?package=DotNet-SonarScanner&version=4.9.0"
#tool "dotnet:https://api.nuget.org/v3/index.json?package=GitVersion.Tool&version=5.1.2"

var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");

var sonarProjectName = "ProjectX.Cpl";
var sonarUrl = EnvironmentVariable("SONAR_URL");
var sonarUser = EnvironmentVariable("SONAR_USER");
var sonarPassword = EnvironmentVariable("SONAR_PASSWORD");

DotNetCoreMSBuildSettings msBuildSettings = null;
GitVersion gitVersions = null;

Setup(context => {
	gitVersions = context.GitVersion();
    msBuildSettings = GetMsBuildSettings();

	if(gitVersions.BranchName != "master")
		sonarProjectName += $":{gitVersions.BranchName}";

	if(target == "Inspect-Unit-Test")
		sonarProjectName += "-Unit";
});

Task("Sonar-Begin")		
	.Does(() => {
		SonarBegin(new SonarBeginSettings {
			Name = sonarProjectName,
			Key = sonarProjectName,
			Version = $"{gitVersions.AssemblySemVer}{gitVersions.PreReleaseTagWithDash}",
			Url = sonarUrl,
			Login = sonarUser,
			Password = sonarPassword,
			VsTestReportsPath = $"{Constants.VsTestReportsDirectoryPath}/*.{Constants.VsTestLogger}",
			OpenCoverReportsPath = $"{Constants.CoverageReportsDirectoryPath}/Coverage.xml",
			Exclusions = "**/Migrations/*.*"
		});
	});


Task("Sonar-End")	
	.Does(() => {
		SonarEnd(new SonarEndSettings { Login = sonarUser, Password = sonarPassword });
  });

Task("Restore")
	.Does(() => {
        DotNetCoreRestore(Constants.SolutionFilePath.FullPath, 
			new DotNetCoreRestoreSettings { ConfigFile = "Nuget.Config" });
    });

Task("Build")
	.IsDependentOn("Restore")
	.Does(() => {
		DotNetCoreBuild(
			Constants.SolutionFilePath.FullPath,
				new DotNetCoreBuildSettings() {
					Configuration = configuration,
					MSBuildSettings = msBuildSettings,
					NoRestore = true
				});
	});


Task("Unit-Test")
	.IsDependentOn("Clean-Artifacts")
	.IsDependentOn("Build")
	.Does(() => {
		var projects = GetFiles("./test/**/*UnitTests.csproj");
		if(projects.Count == 0)
		{
			Warning("No Unit test project found.");
			return;
		}

		if(projects.Count == 0)
		{
			Warning("No test project found.");
			return;
		}

		var testSettings = new DotNetCoreTestSettings() {
			Configuration = configuration,
			Logger = Constants.VsTestLogger,
			ResultsDirectory = Constants.VsTestReportsDirectoryPath,
			NoRestore = true,
			NoBuild = true
		};

		var coverletSettings = new CoverletSettings {
        	CollectCoverage = true,
        	CoverletOutputFormat = CoverletOutputFormat.opencover,
        	CoverletOutputDirectory = Constants.CoverageReportsDirectoryPath,
        	CoverletOutputName = "Coverage.xml"
    	};

		DotNetCoreTest(
			System.IO.Path.GetFullPath(Constants.UnitTestsSolutionFilePath.FullPath),
			testSettings,
			coverletSettings
		);
	});

Task("Inspect-Unit-Test")	
	.IsDependentOn("Sonar-Begin")
	.IsDependentOn("Unit-Test")
	.IsDependentOn("Sonar-End")	
	.Does(() => {});

Task("Package")
	.IsDependentOn("Clean-Artifacts")
    .IsDependentOn("Publish")
	.Does(() => {
		var filePath = $"{Constants.ArtifactsDirectoryPath}/{gitVersions.AssemblySemVer}{gitVersions.PreReleaseTagWithDash}.zip";

		Zip($"{Constants.PublishDirectoryPath}", filePath);
	});


Task("Publish")
	.Does(() => {
		DotNetCorePublish(
		  Constants.SolutionFilePath.FullPath, 
		  new DotNetCorePublishSettings
		  {
			Configuration = configuration,
			MSBuildSettings = msBuildSettings,
			OutputDirectory = $"{Constants.PublishDirectoryPath}"
		});
	});

Task("Clean-Artifacts")
	.Does(() => {
		if(DirectoryExists(Constants.ArtifactsDirectoryPath))
			DeleteDirectory(
			  Constants.ArtifactsDirectoryPath,
			  new DeleteDirectorySettings { Recursive = true}
			  );

		EnsureDirectoryExists(Constants.ArtifactsDirectoryPath);
	});

Task("Nuget-Package")
	.IsDependentOn("Clean-Artifacts")
	.IsDependentOn("Build")
	.Does(() => {	

		var dotNetCorePackSettings = new DotNetCorePackSettings
		{
			Configuration = configuration,
			MSBuildSettings = msBuildSettings,
			OutputDirectory = Constants.NugetPackagesDirectoryPath,
			NoRestore = true,
			NoBuild = true
		};

		var projects = GetFiles("./src/**/*.csproj");
        foreach(var project in projects)
			DotNetCorePack(project.FullPath, dotNetCorePackSettings);
	});

RunTarget(target);

// Helpers
public static class Constants
{
	public static DirectoryPath ArtifactsDirectoryPath => "artifacts";

	public static DirectoryPath PublishDirectoryPath => $"{ArtifactsDirectoryPath}/publish";

	public static DirectoryPath NugetPackagesDirectoryPath => $"{ArtifactsDirectoryPath}/NugetPackages";

	public static DirectoryPath VsTestReportsDirectoryPath => $"{Constants.ArtifactsDirectoryPath}/VsTestReports";

	public static DirectoryPath CoverageReportsDirectoryPath => $"{Constants.ArtifactsDirectoryPath}/Coverage";

	public static FilePath SolutionFilePath => "CoreInspect.Core.sln";

	public static FilePath UnitTestsSolutionFilePath => "ProjectX.Cpl.UnitTests.sln";

	public static string VsTestLogger = "trx";
}

private DotNetCoreMSBuildSettings GetMsBuildSettings()
{
    var settings = new DotNetCoreMSBuildSettings();

    settings.WithProperty("AssemblyVersion", gitVersions.AssemblySemVer);
    settings.WithProperty("VersionPrefix", gitVersions.AssemblySemVer);
    settings.WithProperty("FileVersion", gitVersions.AssemblySemVer);
    settings.WithProperty("InformationalVersion", gitVersions.AssemblySemVer + gitVersions.PreReleaseTagWithDash);
    settings.WithProperty("VersionSuffix", gitVersions.PreReleaseLabel + gitVersions.CommitsSinceVersionSourcePadded);

    return settings;
}
