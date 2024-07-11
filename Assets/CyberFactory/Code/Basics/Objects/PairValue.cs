using System;
using TriInspector;
using UnityEngine;

namespace CyberFactory.Basics.Objects {

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
    }

}