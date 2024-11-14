using System;
using Scellecs.Morpeh;

namespace CyberFactory.Common.Components {

    [Serializable]
    public struct ChangedCount : IComponent {

        public int oldValue;
        public int newValue;

        public int Delta => newValue - oldValue;
        public bool IsIncrease => newValue > oldValue;

        /// <summary>
        /// Бывает что за один кадр - происходит два и более изменения, тогда нам надо объединить их в одно,
        /// сложив математически 
        /// </summary>
        /// <param name="last">Последнее изменение</param>
        public void Merge(ChangedCount last) {
            newValue = last.newValue;
        }

        public override string ToString() {
            return $"ChangedCount: {{ old: {oldValue}, new: {newValue} | delta: {Delta:+0} }}";
        }
    }
}