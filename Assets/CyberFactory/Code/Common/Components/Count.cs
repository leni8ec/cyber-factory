using System;
using Scellecs.Morpeh;

namespace CyberFactory.Common.Components {

    /// <summary>
    /// If added to 'InventoryItem' - means that item is stackable  
    /// </summary>
    [Serializable]
    public struct Count : IComponent, IComparable<Count> {

        public int value;

        public void Change(Count delta, out ChangedCount changedCount) {
            Change(delta.value, out changedCount);
        }

        public void Change(int delta, out ChangedCount changedCount) {
            changedCount = new ChangedCount { oldValue = value, newValue = value += delta };
        }


        #region Operators

        public static int operator -(Count count) {
            return -count.value;
        }

        public static int operator +(Count count) {
            return +count.value;
        }

        public static int operator -(Count count1, Count count2) {
            return count1.value - count2.value;
        }

        public static int operator +(Count count1, Count count2) {
            return count1.value + count2.value;
        }

        public static bool operator ==(Count count1, Count count2) {
            return count1.value == count2.value;
        }

        public static bool operator !=(Count count1, Count count2) {
            return count1.value != count2.value;
        }

        public static bool operator ==(Count count1, int countValue2) {
            return count1.value == countValue2;
        }

        public static bool operator !=(Count count1, int countValue2) {
            return count1.value != countValue2;
        }

        public static bool operator <(Count count1, Count count2) {
            return count1.value < count2.value;
        }

        public static bool operator >(Count count1, Count count2) {
            return count1.value > count2.value;
        }

        public static bool operator <(Count count1, int countValue2) {
            return count1.value < countValue2;
        }

        public static bool operator >(Count count1, int countValue2) {
            return count1.value > countValue2;
        }

        public static bool operator <=(Count count1, Count count2) {
            return count1.value <= count2.value;
        }

        public static bool operator >=(Count count1, Count count2) {
            return count1.value >= count2.value;
        }

        public static bool operator <=(Count count1, int countValue2) {
            return count1.value <= countValue2;
        }

        public static bool operator >=(Count count1, int countValue2) {
            return count1.value >= countValue2;
        }

        #endregion


        #region Equality members

        public int CompareTo(Count other) {
            return value.CompareTo(other.value);
        }

        public bool Equals(Count other) {
            return value == other.value;
        }

        public override bool Equals(object obj) {
            return obj is Count other && Equals(other);
        }

        public override int GetHashCode() {
            return value.GetHashCode();
        }

        #endregion


    }

}