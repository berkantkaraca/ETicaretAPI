namespace ETicaretAPI.Application.Abstractions.Storage
{
    /// <summary>
    /// Client'ın kullanacağı storage servisini belirler
    /// </summary>
    public interface IStorageService : IStorage
    {
        public string StorageName { get; }
    }
}
