using Guardian.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guardian.Common.Interfaces;

namespace Guardian.Common.Interfaces
{
    public interface ISoftwareScanner
    {
        Task<List<InstalledApplication>> ScanAsync();
    }
}
