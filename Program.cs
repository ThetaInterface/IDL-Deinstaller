using System.Diagnostics;
using System.Runtime.Versioning;

using static IDL.SoftwareManager;

namespace Deinstaller;

[SupportedOSPlatform("windows")]
public static class Program
{
    private static readonly string _appName = "WSP";

    public static void Main()
    {
        if (GetCurrentSoftwareVersion(_appName) != null)
        {
            string? installPath = GetSoftwareInstallationPath(_appName);

            DeleteShortcut(_appName, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)));
            DeleteShortcut(_appName);

            DeleteSubKey(_appName);

            if (installPath != null)
                Annihilate(installPath);
        }
    }
    
    private static void Annihilate(string path)
    {
        string batFile = Path.Combine(Path.GetTempPath(), "uninstall.bat");
        string batContent = $@"
            @echo off
            timeout /t 2 /nobreak > NUL
            rmdir /s /q ""{path}""
            del ""%~f0""
            ";

        File.WriteAllText(batFile, batContent);
        
        ProcessStartInfo psi = new ()
        {
            FileName = batFile,
            CreateNoWindow = true,
            UseShellExecute = false
        };
        Process.Start(psi);

        Environment.Exit(0);
    }
}