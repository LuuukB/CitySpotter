using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitySpotter.Domain.Services.FileServices
{
    public interface IFileService
    {
        Task<string> ReadFileAsync(string filePath);
    }
}
