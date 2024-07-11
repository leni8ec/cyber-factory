using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;

namespace CyberFactory.Coins {

    public class CoinsProvider : MonoProvider<Coins> { }


    [Serializable]
    public struct Coins : IComponent {
        public int count;
    }

}