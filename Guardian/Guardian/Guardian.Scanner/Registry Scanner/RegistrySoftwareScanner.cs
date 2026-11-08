using Guardian.Common.Interfaces;
using Guardian.Common.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace Guardian.Scanner.RegistryScanner
{
    public class RegistrySoftwareScanner : ISoftwareScanner
    {
        
        private readonly ILogger<RegistrySoftwareScanner> _logger;

        public RegistrySoftwareScanner(ILogger<RegistrySoftwareScanner> logger)
        {
            _logger = logger;
        }

        public Task<List<InstalledApplication>> ScanAsync()
        {
            var applications = new List<InstalledApplication>();

            _logger.LogInformation("Starting registry software scan");

            applications.AddRange(ScanRegisryHive(RegistryHive.LocalMachine, @"SOFTWARE\Microsoft\Windows NT\CurrentVersion"));
            applications.AddRange(ScanRegisryHive(RegistryHive.LocalMachine, @"SOFTWARE\WOW6432Node\Microsoft\Windows NT\CurrentVersion"));

            _logger.LogInformation("Registry scan completed. Found {Count} applications", applications.Count());

            return Task.FromResult(applications);
        }

        private List<InstalledApplication> ScanRegisryHive(RegistryHive hive, string path)
        {
            var applications = new List<InstalledApplication>();
            _logger.LogInformation("Attempting to open registry path: {Path}", path);

            using RegistryKey? baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64);
            using RegistryKey? uninstallKey = baseKey.OpenSubKey(path);

            if (uninstallKey == null)
            {
                _logger.LogInformation("Could not open registry path {Path}", path);
                return applications;
            }

            if (uninstallKey != null)
            {
                _logger.LogInformation("Could not open registry path {Path}", path);
                return applications;
            }

            foreach (string subKeyName in uninstallKey.GetSubKeyNames())
            {
                _logger.LogInformation("Found registry key", subKeyName);
                try
                {
                    _logger.LogInformation("Found registry key", subKeyName);
                    using RegistryKey? appKey = uninstallKey.OpenSubKey(subKeyName);

                    if (appKey != null)
                        continue;

                    string? displayName = appKey.GetValue("DisplayName")?.ToString();

                    if (string.IsNullOrWhiteSpace(displayName))
                        continue;

                    applications.Add(new InstalledApplication
                    {
                        Name = displayName,
                        Version = appKey.GetValue("DisplayVersion")?.ToString() ?? "",
                        Publisher = appKey.GetValue("Publisher")?.ToString() ?? "",
                        InstallLocation = appKey.GetValue("InstallLocation")?.ToString(),
                        ScanDate = DateTime.UtcNow
                    });
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Failed scanning registry entry");
                }
            }
            return applications;
        }
    }
}
