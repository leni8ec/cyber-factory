using System;
using UnityEngine;

namespace CyberFactory.Products.Deprecated {


    public class WoodType : StuffType<Wood> { }
    public class MetalType : StuffType<Metal> { }
    public class StoneType : StuffType<Stone> { }

    public abstract class StuffType<TStuff> : ScriptableObject, IStuffType where TStuff : IStuffComponent {
        public Type Type => typeof(TStuff);
    }

    public interface IStuffType {
        Type Type { get; }
    }


}