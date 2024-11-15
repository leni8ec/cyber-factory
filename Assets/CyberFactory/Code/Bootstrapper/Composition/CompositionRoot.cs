using CyberFactory.Inventories.Services;
using VContainer;
using VContainer.Unity;

namespace CyberFactory.Bootstrapper.Composition {
    public class CompositionRoot : LifetimeScope {

        protected override void Configure(IContainerBuilder builder) {
            builder.Register<InventoryService>(Lifetime.Singleton);
        }

    }
}