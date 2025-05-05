using System.Reflection;
using ConsoleAppFramework;
using Rick.Cli.Services;

namespace Rick.Cli.Commands;

/// <summary>
/// We know one thing... YOLO.
/// </summary>
public class Architect
{
    /// <summary>
    /// Caution.
    /// </summary>
    /// <param name="yolo">Chad mode</param>
    [Command(" ")]
    public async Task Run(bool yolo = false)
    {
        if (yolo)
        {
            Console.WriteLine("YOLO!!");
            await YoloInstaller.FreshInstall();
        }
        else
        {
            Console.WriteLine("Do you even yolo, bro?");
        }
    }
}
public static class ConfigCopier
{
    public static async Task CopyConfigs(string sourceDir, string destDir)
    {
        Console.WriteLine($"Copying configs from {sourceDir} to {destDir}...");

        // Resolve tilde to home directory
        destDir = destDir.Replace("~", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

        // Check if source directory exists
        if (!Directory.Exists(sourceDir))
        {
            Console.WriteLine($"Source directory {sourceDir} does not exist. Skipping config copy.");
            return;
        }

        // Copy directory recursively, overwriting existing files
        CopyDirectory(sourceDir, destDir, true);
        Console.WriteLine($"Configs copied to {destDir}");

        await Task.CompletedTask; // For async compatibility
    }

    private static void CopyDirectory(string sourceDir, string destDir, bool overwrite)
    {
        // Create destination directory if it doesn't exist
        Directory.CreateDirectory(destDir);

        // Copy all files
        foreach (var file in Directory.GetFiles(sourceDir))
        {
            var destFile = Path.Combine(destDir, Path.GetFileName(file));
            File.Copy(file, destFile, overwrite);
        }

        // Copy all subdirectories
        foreach (var subDir in Directory.GetDirectories(sourceDir))
        {
            var destSubDir = Path.Combine(destDir, Path.GetFileName(subDir));
            CopyDirectory(subDir, destSubDir, overwrite);
        }
    }
}
