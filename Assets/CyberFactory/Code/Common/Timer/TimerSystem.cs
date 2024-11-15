using CyberFactory.Basics.Constants.Editor;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Common.Timer {

    [CreateAssetMenu(menuName = AssetMenu.Common.SYSTEM + "Timer")]
    public sealed class TimerSystem : UpdateSystem {
        private Filter filter;

        public override void OnAwake() {
            filter = World.Filter.With<Timer>().Without<TimerComplete>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (var entity in filter) {
                ref var timer = ref entity.GetComponent<Timer>();
                timer.time += deltaTime;
                if (!timer.IsComplete) continue;

                entity.AddComponent<TimerComplete>();
            }
        }
    }
}