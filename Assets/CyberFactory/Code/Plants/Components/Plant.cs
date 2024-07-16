using System;
using CyberFactory.Plants.Models;
using Scellecs.Morpeh;

namespace CyberFactory.Plants.Components {

    [Serializable]
    public struct Plant : IComponent {

        public PlantModel model;

    }

}