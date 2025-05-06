using SimpleExec;
using System.IO;
using System.IO.Compression;
using System.Net.Http;

namespace Rick.Cli.Services;

public class YoloInstaller
{
    public static async Task UserInstall()
    {
        await Command.RunAsync("bash", "-c \"git clone https://aur.archlinux.org/paru.git /tmp/paru && cd /tmp/paru && makepkg -si --noconfirm\"");
        await Command.RunAsync("paru", "-S  grimblast-git --noconfirm");

        Console.WriteLine("Copying configs to ~/.config...");

        var homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var configDir = Path.Combine(homeDir, ".config");
        var localConfigDir = Path.Combine(homeDir, "dotfiles", "config");

        if (Directory.Exists(localConfigDir))
        {
            Console.WriteLine($"Using {localConfigDir} as source.");
            ConfigCopier.CopyConfigs(localConfigDir, configDir);
        }
        else
        {
            Console.WriteLine("We'll download the thang... but not today.");
        }

        var sourceIdeavimrc = Path.Combine(configDir, "vim", ".ideavimrc");
        var targetIdeavimrc = Path.Combine(homeDir, ".ideavimrc");
        CreateSymlink(sourceIdeavimrc, targetIdeavimrc);

        var sourceBashrc = Path.Combine(configDir, "bash", ".bashrc");
        var targetBashrc = Path.Combine(homeDir, ".bashrc");
        CreateSymlink(sourceBashrc, targetBashrc);
    }

    public static async Task SudoInstall()
    {
        Console.WriteLine("Starting YOLO install");

        await Command.RunAsync("pacman", "-S --needed base-devel git --noconfirm");
        await Command.RunAsync("pacman", "-S libnotify hyprpicker wl-clipboard grim slurp rofi-wayland noto-fonts-emoji ttf-jetbrains-mono-nerd waybar --noconfirm");
        await Command.RunAsync("pacman", "-S ghostty yazi ffmpeg p7zip jq poppler fd ripgrep fzf zoxide imagemagick --noconfirm");
        await Command.RunAsync("pacman", "-S dotnet-sdk --noconfirm");
        await Command.RunAsync("pacman", "-S gnome-keyring libsecret --noconfirm");
        await Command.RunAsync("pacman", "-S discord flatpak --noconfirm");
    }

    private static void CreateSymlink(string sourcePath, string targetPath)
    {
        try
        {
            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"Source file {sourcePath} does not exist. Skipping symlink creation.");
                return;
            }

            // Check if target exists as a file or symlink
            if (File.Exists(targetPath) || Directory.Exists(targetPath))
            {
                try
                {
                    if (File.GetAttributes(targetPath).HasFlag(FileAttributes.ReparsePoint))
                    {
                        File.Delete(targetPath); // Delete if it's a symlink
                        Console.WriteLine($"Removed existing symlink at {targetPath}");
                    }
                    else
                    {
                        File.Delete(targetPath); // Delete if it's a regular file
                        Console.WriteLine($"Removed existing file at {targetPath}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to delete existing target at {targetPath}: {ex.Message}");
                    return;
                }
            }

            File.CreateSymbolicLink(targetPath, sourcePath);
            Console.WriteLine($"Created symlink: {targetPath} -> {sourcePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to create symlink for {Path.GetFileName(targetPath)}: {ex.Message}");
        }
    }

    public static class ConfigCopier
    {
        public static void CopyConfigs(string sourceDir, string destDir)
        {
            Console.WriteLine($"Copying configs from {sourceDir} to {destDir}...");

            if (!Directory.Exists(sourceDir))
            {
                Console.WriteLine($"Source directory {sourceDir} does not exist. Skipping config copy.");
                return;
            }

            CopyDirectory(sourceDir, destDir, true);
            Console.WriteLine($"Configs copied to {destDir}");
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
}
