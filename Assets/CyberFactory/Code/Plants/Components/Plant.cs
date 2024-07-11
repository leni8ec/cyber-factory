using System;
using CyberFactory.Plants.Configs;
using Scellecs.Morpeh;

namespace CyberFactory.Plants.Components {

    [Serializable]
    public struct Plant : IComponent {

        public PlantVariant plant;

    }

}