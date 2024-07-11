using System;
using Scellecs.Morpeh.Providers;

namespace CyberFactory.Products.Deprecated {

    public class WoodProvider : MonoProvider<Wood> { }

    [Serializable]
    public struct Wood : IStuffComponent {
        public int count;
    }

}