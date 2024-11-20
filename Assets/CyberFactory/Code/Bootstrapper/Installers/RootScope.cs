using CyberFactory.Basics.Installers;
using CyberFactory.Common.Services.GameObjects;
using CyberFactory.Inventories.Services;
using VContainer;
using VContainer.Unity;

namespace CyberFactory.Bootstrapper.Installers {
    public class RootScope : ScopeInstaller {

        protected override void Install(IContainerBuilder builder) {

            builder.RegisterEntryPoint<EntryPoint>();

            builder.Register<InventoryService>(Lifetime.Singleton);
            builder.Register<GameObjectsService>(Lifetime.Singleton);

        }

    }
}