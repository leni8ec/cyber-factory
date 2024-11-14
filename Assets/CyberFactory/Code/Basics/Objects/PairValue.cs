using System;
using TriInspector;
using UnityEngine;

namespace CyberFactory.Basics.Objects {
    /// <summary>
    /// Пара 'ключ-значение' для удобной сериализации в Unity инспекторе 
    /// </summary>
    [Serializable] [InlineProperty] [DeclareHorizontalGroup("Group")]
    public struct PairValue<TKey, TValue> {

        [SerializeField] [HideLabel] [Group("Group")]
        private TKey key;
        [SerializeField] [HideLabel] [Group("Group")]
        private TValue value;

        public TKey Key => key;
        public TValue Value => value;

        public PairValue(TKey key, TValue value) {
            this.key = key;
            this.value = value;
        }

        public void Deconstruct(out TKey key, out TValue value) {
            key = this.key;
            value = this.value;
        }

    }
}