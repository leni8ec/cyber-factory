using System;
using Scellecs.Morpeh.Providers;

namespace CyberFactory.Products.Deprecated {

    public class StoneProvider : MonoProvider<Stone> { }

    [Serializable]
    public struct Stone : IStuffComponent {
        public int count;
    }

}