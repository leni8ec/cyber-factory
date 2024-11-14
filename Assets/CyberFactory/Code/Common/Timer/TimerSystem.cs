using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Common.States;
using Scellecs.Morpeh;
using UnityEngine;

namespace CyberFactory.Common.Timer {

    [CreateAssetMenu(menuName = AssetMenu.Common.SYSTEM + "Timer")]
    public sealed class TimerSystem : Scellecs.Morpeh.Systems.UpdateSystem {
        private Filter filter;

        public override void OnAwake() {
            filter = World.Filter.With<Timer>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (var entity in filter) {
                ref var timer = ref entity.GetComponent<Timer>();
                timer.duration += deltaTime;
                if (!timer.IsComplete) continue;

                entity.AddComponent<CompleteState>();
                entity.RemoveComponent<Timer>();
            }
        }
    }
}