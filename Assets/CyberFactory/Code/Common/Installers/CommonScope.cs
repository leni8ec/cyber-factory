using CyberFactory.Basics.Installers;
using CyberFactory.Common.Services.GameObjects;
using VContainer;

namespace CyberFactory.Common.Installers {
    public class CommonScope : ScopeInstaller {

        protected override void Install(IContainerBuilder builder) {
            
            // builder.Register<GameObjectsService>(Lifetime.Singleton);
 
        }
        
    }
}