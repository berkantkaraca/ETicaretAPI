using ETicaretAPI.Application.DTOs.Configuration;

namespace ETicaretAPI.Application.Abstractions.Services.Configurations
{
    public interface IApplicationService
    {
        /// <summary>
        /// Uygulamada AuthorizeDefinition ile işaretlenmiş tüm endpoint'leri döner. (Authorize gerektiren endpointler)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        List<Menu> GetAuthorizeDefinitionEndpoints(Type type);
    }
}
