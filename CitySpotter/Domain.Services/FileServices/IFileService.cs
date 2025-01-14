namespace CitySpotter.Domain.Services.FileServices
{
    public interface IFileService
    {
        Task<string> ReadFileAsync(string filePath);
    }
}
