using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;

namespace CyberFactory.Coins {

    public class CoinsProvider : MonoProvider<Coins> { }


    /// <summary>
    /// Предполагаемый компонент для введения денег в игру 
    /// </summary>
    [Serializable]
    public struct Coins : IComponent {
        public int count;
    }

}