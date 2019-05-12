#addin nuget:?package=Cake.Coverlet&version=2.1.2

using System.Xml.Linq;

var target = Argument("target", "default");
var configuration = Argument("configuration", "Release");
var output = Argument("output", "./artifacts");
var versionSuffix = Argument<string>("versionSuffix", null);

Information("Target: " + target);
Information("Configuration: " + configuration + ", tests always run under Debug");
Information("Output path: " + output);
Information("Version suffix: " + versionSuffix);

Task("clean")
    .Does(() => {
        CleanDirectories("./src/**/bin/" + configuration);
        CleanDirectories("./src/**/obj/" + configuration);
        CleanDirectories("./test/**/bin/Debug");
        CleanDirectories("./test/**/obj/Debug");
        CleanDirectory("./artifacts");
        CleanDirectory("./coverage");
    });

Task("build")
    .IsDependentOn("clean")
    .Does(() => {
        var buildSettings = new DotNetCoreBuildSettings {
            VersionSuffix = versionSuffix,
            Configuration = configuration
        };

        DotNetCoreBuild("./src/Shorthand.ImageSharp.WebP/Shorthand.ImageSharp.WebP.csproj", buildSettings);
    });

Task("pack")
    .IsDependentOn("clean")
    .Does(() => {
        var buildSettings = new DotNetCorePackSettings {
            VersionSuffix = versionSuffix,
            Configuration = configuration,
            OutputDirectory = output
        };

        DotNetCorePack("./src/Shorthand.ImageSharp.WebP/Shorthand.ImageSharp.WebP.csproj", buildSettings);
    });

Task("test")
    .IsDependentOn("clean")
    .Does(() => {
        var settings = new DotNetCoreTestSettings {
            Logger = "console;verbosity=normal",
            TestAdapterPath = "."
         };

        var coverletSettings = new CoverletSettings {
            CollectCoverage = true,
            CoverletOutputFormat = CoverletOutputFormat.cobertura | CoverletOutputFormat.opencover,
            CoverletOutputDirectory = Directory(@".\coverage\"),
            CoverletOutputName = $"results-{DateTime.UtcNow:dd-MM-yyyy-HH-mm-ss-FFF}"
        };

        DotNetCoreTest("./test/Shorthand.ImageSharp.WebP.Tests/Shorthand.ImageSharp.WebP.Tests.csproj", settings, coverletSettings);
    });

Task("azure-pipelines")
    .IsDependentOn("clean")
    .Does(() => {
        var settings = new DotNetCoreTestSettings {
            Logger = "trx;LogFileName=TestResults.trx"
         };

        var coverletSettings = new CoverletSettings {
            CollectCoverage = true,
            CoverletOutputFormat = CoverletOutputFormat.cobertura,
            CoverletOutputDirectory = Directory(@".\coverage\"),
            CoverletOutputName = "results"
        };

        DotNetCoreTest("./test/Shorthand.ImageSharp.WebP.Tests/Shorthand.ImageSharp.WebP.Tests.csproj", settings, coverletSettings);
    });

Task("default")
    .IsDependentOn("pack");

RunTarget(target);