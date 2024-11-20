using CyberFactory.Basics.Installers;
using CyberFactory.Products.Ghost;
using VContainer;

namespace CyberFactory.Products.Installers {
    public class ProductsScope : ScopeInstaller {

        protected override void Install(IContainerBuilder builder) {

            builder.Register<ProductGhostFactory>(Lifetime.Singleton);

        }

    }
}