using CyberFactory.Common.Components;
using Scellecs.Morpeh;

namespace CyberFactory.Basics.Extensions {
    public static class ChangeCountExtensions {

        /// <summary>
        /// <inheritdoc cref="ChangeSmart(CyberFactory.Common.Components.Count,int,in Scellecs.Morpeh.Entity)"/>
        /// </summary>
        public static void ChangeSmart(this ref Count count, Count delta, in Entity entity) {
            ChangeSmart(ref count, delta.value, in entity);
        }

        /// <summary>
        /// Обработку компонентов <see cref="ChangedCount"/> вынесена сюда, что-бы постоянно не дублировать один и тот-же код.
        /// </summary>
        public static void ChangeSmart(this ref Count count, int delta, in Entity entity) {
            count.Change(delta, out var changedCount);

            if (entity.Has<ChangedCount>()) { // if item is already changed in this frame
                entity.GetComponent<ChangedCount>().Merge(changedCount);
            } else {
                entity.AddComponent<ChangedCount>() = changedCount;
            }
        }

    }
}