using ETicaretAPI.SignalR.Hubs;
using Microsoft.AspNetCore.Builder;

namespace ETicaretAPI.SignalR
{
    public static class HubRegistration
    {
        /// SignalR endpointleri burada kaydedilir
        public static void MapHubs(this WebApplication webApplication)
        {
            webApplication.MapHub<ProductHub>("/products-hub");
            webApplication.MapHub<OrderHub>("/orders-hub");
        }
    }
}
