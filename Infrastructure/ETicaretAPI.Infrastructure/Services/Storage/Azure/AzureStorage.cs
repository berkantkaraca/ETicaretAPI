using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ETicaretAPI.Application.Abstractions.Storage.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ETicaretAPI.Infrastructure.Services.Storage.Azure
{
    public class AzureStorage : Storage, IAzureStorage
    {
        readonly BlobServiceClient _blobServiceClient; //İlgili azure servisine bağlanmak için kullanılır.
        BlobContainerClient _blobContainerClient; //Account içindeki container'da dosya işlemleri yapmak için kullanılır.

        public AzureStorage(IConfiguration configuration)
        {
            _blobServiceClient = new(configuration["Storage:Azure"]);
        }

        public async Task DeleteAsync(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }

        public List<string> GetFiles(string containerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Select(b => b.Name).ToList();
        }

        public bool HasFile(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Any(b => b.Name == fileName);
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string containerName, IFormFileCollection files)
        {
            //Çalışılacak container'ı belirleme
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            await _blobContainerClient.CreateIfNotExistsAsync();
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

            List<(string fileName, string pathOrContainerName)> datas = new();

            foreach (IFormFile file in files)
            {
                string fileNewName = await FileRenameAsync(containerName, file.Name, HasFile);

                BlobClient blobClient = _blobContainerClient.GetBlobClient(fileNewName);
                await blobClient.UploadAsync(file.OpenReadStream());
                datas.Add((fileNewName, $"{containerName}/{fileNewName}"));
            }
            //todo
            /*
             https://bketicaret.blob.core.windows.net/photo/favicon-16x16.png
             
            upload edilmiş dosyasının veritabanında tutulması şekline

            sürekli dosya/resim ihtiyaçlarını local'de tutmak (örneğin api sahibi firma logo görseli), son kullanıcıların dosya/resim ihtiyaçlarını ise azure gibi 3ncü parti bir sunucu'da tutmak (örneğin ürün fotoğrafları) gibi bir senaryo'da, veritabanında domain(https:/azure.com) ve querystring(/photos/a.jpg) kısımlarını tabloda ayrı ayrı sütunlarda tutmak, 

            fonksiyonel olarak'ta domain hücresi boş ise, api'nin veya storage servisinin default domain tanımının kullanılması, değilise, özel domainin veritabanındaki ilgili hücreden combine kullanılması gibi bir zenginlik hayal ettim. 
            
            Bu sayede farklı azure containerları dahi aynı api'de kullanılabilir. Domain sütunu içinde, bu özelleştirme yapılabilir.
             */

            return datas;
        }
    }
}
