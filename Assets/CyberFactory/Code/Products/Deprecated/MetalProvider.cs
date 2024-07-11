using System;
using Scellecs.Morpeh.Providers;

namespace CyberFactory.Products.Deprecated {

    public class MetalProvider : MonoProvider<Metal> { }

    [Serializable]
    public struct Metal : IStuffComponent {
        public int count;
    }

}