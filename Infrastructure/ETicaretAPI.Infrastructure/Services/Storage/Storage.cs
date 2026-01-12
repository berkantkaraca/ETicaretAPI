using ETicaretAPI.Infrastructure.Operations;

namespace ETicaretAPI.Infrastructure.Services.Storage
{
    /// <summary>
    /// Tüm storage servislerde ortak kullanılacak metotların bulunduğu sınıf.
    /// </summary>
    public class Storage
    {
        protected delegate bool HasFile(string pathOrContainerName, string fileName);

        protected Task<string> FileRenameAsync(string pathOrContainerName, string fileName, HasFile hasFileMethod)
        {
            string extension = Path.GetExtension(fileName);
            string baseName = NameOperation.CharacterRegulatory(
                Path.GetFileNameWithoutExtension(fileName)
            );

            string newFileName = baseName + extension;
            int counter = 1;

            while (hasFileMethod(pathOrContainerName, newFileName))
            {
                counter++;
                newFileName = $"{baseName}-{counter}{extension}";
            }

            return Task.FromResult(newFileName);
        }
    }
}
