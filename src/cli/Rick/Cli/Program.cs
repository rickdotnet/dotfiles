using Microsoft.Extensions.Hosting;
using Cli;
using SimpleExec;

// will launch into the CLI using various flags
// there are a few options, looking at Terminal.Gui
// first, but then also looking at the one from nuecc
// and team.

// only some of them will run the host, and at that,
// we'll wrap it and make sure we're smart about how
// hosts are created
Console.WriteLine("Hello World!");

Console.WriteLine("Waiting for 5 seconds...");
await Task.Delay(5000);

await Command.RunAsync("pacman", "-S --needed base-devel --noconfirm");

await Command.RunAsync("pacman", "-S libnotify hyprpicker wl-clipboard grim slurp rofi-wayland noto-fonts-emoji ttf-jetbrains-mono-nerd --noconfirm");

await Command.RunAsync("paru", "-S ghostty --noconfirm");

await Command.RunAsync("pacman", "-S yazi ffmpeg p7zip jq poppler fd ripgrep fzf zoxide imagemagick --noconfirm");

await Command.RunAsync("pacman", "-S dotnet-sdk --noconfirm");

await Command.RunAsync("pacman", "-S gnome-keyring libsecret --noconfirm");

await Command.RunAsync("pacman", "-S discord flatpak --noconfirm");
// var host = Host.CreateApplicationBuilder(args).ConfigureHost();
// host.Run();