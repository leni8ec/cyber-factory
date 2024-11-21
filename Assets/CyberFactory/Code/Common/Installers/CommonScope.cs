using CyberFactory.Basics.Installers;
using VContainer;

namespace CyberFactory.Common.Installers {
    public class CommonScope : ScopeInstaller {

        protected override void Install(IContainerBuilder builder) {

            // builder.Register<GameObjectsService>(Lifetime.Singleton);

        }

    }
}