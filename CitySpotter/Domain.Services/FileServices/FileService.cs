namespace CitySpotter.Domain.Services.FileServices;

public class FileService : IFileService
{
    public async Task<string> ReadFileAsync(string filePath)
    {
        using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync(filePath);
        using StreamReader reader = new StreamReader(fileStream);
        //Dont foget if doesnt work uncommend and remove current return

        /*var temporaryString = "";
        for (int i = 0; i < reader.Read(); i++)
        {
            temporaryString += reader.ReadLine();
        }
        return temporaryString;*/

        return await reader.ReadToEndAsync();
    }
}
