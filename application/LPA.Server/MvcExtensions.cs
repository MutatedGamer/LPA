using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LPA.Server
{
    public static class MvcExtensions
    {
        // From https://stackoverflow.com/a/62792774
        public static IMvcBuilder AddControllersAsServices(
            this IMvcBuilder builder,
            ServiceLifetime lifetime)
        {
            var feature = new ControllerFeature();
            builder.PartManager.PopulateFeature(feature);

            foreach (var controller in feature.Controllers.Select(c => c.AsType()))
            {
                builder.Services.Add(
                    ServiceDescriptor.Describe(controller, controller, lifetime));
            }

            builder.Services.Replace(ServiceDescriptor
                .Transient<IControllerActivator, ServiceBasedControllerActivator>());

            return builder;
        }
    }
}
