using System;
using Scellecs.Morpeh;

namespace CyberFactory.Common.Components {

    [Serializable]
    public struct Progress : IComponent {

        public float value;

        public bool IsComplete => value >= 1;

        public void Clamp() {
            if (value < 0) value = 0;
            else if (value > 1) value = 1;
        }

    }

}