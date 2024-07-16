using System;
using Scellecs.Morpeh;

namespace CyberFactory.Common.Components {

    [Serializable]
    public struct ChangedCount : IComponent {

        public int oldValue;
        public int newValue;

        public int Delta => newValue - oldValue;
        public bool IsIncrease => newValue > oldValue;

    }
}