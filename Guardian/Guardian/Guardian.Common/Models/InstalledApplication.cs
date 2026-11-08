namespace Guardian.Common.Models
{
    public class InstalledApplication
    {
        public string Name { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public string? InstallLocation { get; set; }
        public DateTime ScanDate { get; set; }
    }
}
