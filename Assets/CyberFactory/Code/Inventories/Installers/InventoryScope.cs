using CyberFactory.Basics.Installers;
using CyberFactory.Inventories.Services;
using VContainer;

namespace CyberFactory.Inventories.Installers {
    public class InventoryScope : ScopeInstaller {

        protected override void Install(IContainerBuilder builder) {

                // builder.Register<InventoryService>(Lifetime.Singleton);

        }

    }
}