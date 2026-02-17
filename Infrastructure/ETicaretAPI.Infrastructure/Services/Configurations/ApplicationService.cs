using ETicaretAPI.Application.Abstractions.Services.Configurations;
using ETicaretAPI.Application.CustomAttributes;
using ETicaretAPI.Application.DTOs.Configuration;
using ETicaretAPI.Application.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;

namespace ETicaretAPI.Infrastructure.Services.Configurations
{
    public class ApplicationService : IApplicationService
    {
        public List<Menu> GetAuthorizeDefinitionEndpoints(Type type)
        {
            // Verilen type'ın bulunduğu assembly'yi al
            var assembly = Assembly.GetAssembly(type);

            // Assembly içindeki tüm tipleri alıp ControllerBase'den türeyenleri filtrele
            var controllers = assembly
                .GetTypes()
                .Where(t => t.IsAssignableTo(typeof(ControllerBase)));

            // Menü listesini oluştur
            List<Menu> menus = new();

            // Her controller'ı dolaş
            foreach (var controller in controllers)
            {
                // Controller içindeki AuthorizeDefinitionAttribute ile işaretlenmiş metodları al
                var actions = controller
                    .GetMethods()
                    .Where(m => m.IsDefined(typeof(AuthorizeDefinitionAttribute), true));

                // Her action metodunu dolaş
                foreach (var method in actions)
                {
                    // Metodun üzerindeki AuthorizeDefinitionAttribute'ü al
                    var authorizeAttr = method.GetCustomAttribute<AuthorizeDefinitionAttribute>(true);

                    // Attribute yoksa bir sonraki metoda geç
                    if (authorizeAttr is null)
                        continue;

                    // Bu action'ın hangi menüye ait olduğunu kontrol et
                    var menu = menus.FirstOrDefault(m => m.Name == authorizeAttr.Menu);

                    // Menü yoksa yeni menü oluştur ve listeye ekle
                    if (menu is null)
                    {
                        menu = new Menu { Name = authorizeAttr.Menu };
                        menus.Add(menu);
                    }

                    // Metodun HTTP tipini belirle (GET, POST, PUT, DELETE vb.)
                    var httpAttr = method.GetCustomAttributes(true)
                                         .OfType<HttpMethodAttribute>()
                                         .FirstOrDefault();

                    // Action nesnesini oluştur ve özelliklerini doldur
                    var action = new Application.DTOs.Configuration.Action
                    {
                        ActionType = Enum.GetName(typeof(ActionType), authorizeAttr.ActionType),
                        Definition = authorizeAttr.Definition,
                        HttpType = httpAttr?.HttpMethods.First() ?? HttpMethods.Get
                    };

                    // Action için benzersiz bir kod oluştur
                    action.Code = $"{action.HttpType}.{action.ActionType}.{action.Definition.Replace(" ", "")}";

                    // Oluşturulan action'ı ilgili menünün action listesine ekle
                    menu.Actions.Add(action);
                }
            }

            // Tüm menüleri ve içindeki action'ları döndür
            return menus;
        }
    }
}
