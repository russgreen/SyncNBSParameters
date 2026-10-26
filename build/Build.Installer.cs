using Fallout.Common;
using Fallout.Common.Git;
using Fallout.Common.Tools.DotNet;
using Fallout.Solutions;
using Octokit;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using static Fallout.Common.Tools.DotNet.DotNetTasks;

partial class Build
{
    Target Installer => _ => _
    .TriggeredBy(Sign)
    .OnlyWhenStatic(() => GitRepository.IsOnMainOrMasterBranch())
    .Executes(() =>
    {
        var wixProjectPath = Path.Combine(RootDirectory, @"installer\SyncNBSParameters.Setup.wixproj");
        var version = GetProjectVersion(Path.Combine(RootDirectory, @"SyncNBSParameters\SyncNBSParameters.csproj"));


        Log.Information("WiX project : {wixProjectPath}", wixProjectPath);
        Log.Information("Version : {version}", version);

        DotNetBuild(settings => settings
            .SetProjectFile(wixProjectPath)
            .SetConfiguration("Release")
            .SetProperty("VersionPrefix", version)
            .SetVerbosity(DotNetVerbosity.minimal));

        SignMSI(version);
    });

    static string GetProjectVersion(string projectFilePath)
    {
        var doc = XDocument.Load(projectFilePath);
        var ns = doc.Root?.Name.NamespaceName;

        // First PropertyGroup usually contains the base Version
        var versionElement = ns != null
            ? doc.Descendants(XName.Get("Version", ns)).FirstOrDefault()
            : doc.Descendants("Version").FirstOrDefault();

        return versionElement?.Value ?? "1.0.0";
    }

    static void SignMSI(string version)
    {
        var aipOutputPath = Path.Combine(RootDirectory, @"installer\SyncNBSParameters-SetupFiles");
        Log.Information(aipOutputPath);

        var msiPath = Directory.GetFiles(aipOutputPath, $"*{version}.msi").FirstOrDefault();
        Log.Information(msiPath);

        if (File.Exists(msiPath))
        {
            SignFiles(new System.Collections.Generic.List<string> { msiPath });
        }
    }
}
