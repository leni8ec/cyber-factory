using System;
using CyberFactory.Plants.Models;
using Scellecs.Morpeh;
using TriInspector;

namespace CyberFactory.Plants.Components {
    [Serializable]
    public struct Plant : IComponent {

        public PlantModel model;

        // [Range(1, 99)] - not work for struct
        // [OnValueChanged(nameof(OnLevelValueChangedInInspector))] - not work for struct
        // [ValidateInput(nameof(ValidateNumber))] - not work for struct
        public int level;

        [ShowInInspector] [ReadOnly] [PropertyOrder(3)]
        private int MaxLevel => model ? model.productionRateLevels.Count : 0;

        [ShowInInspector] [ReadOnly] [PropertyOrder(2)]
        private int ValidLevel {
            get {
                if (level < 1) return 1;
                if (level > MaxLevel) return MaxLevel;
                return level;
            }
        }

        [PropertySpace]
        [ShowInInspector] [ReadOnly] [PropertyOrder(3)]
        public float ProductionRate => model ? model.productionRateLevels[ValidLevel - 1] : 0; // use index value

    }
}