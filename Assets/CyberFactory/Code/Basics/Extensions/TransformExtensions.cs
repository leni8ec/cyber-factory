using UnityEngine;

namespace CyberFactory.Basics.Extensions {
    public static class TransformExtensions {

        public static void Reset(this Transform transform) {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public static void Reset(this Transform transform, Vector3 localPosition) {
            transform.localPosition = localPosition;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public static void Reset(this Transform transform, Vector3 localPosition, Vector3 localRotation) {
            transform.localPosition = localPosition;
            transform.localRotation = Quaternion.Euler(localRotation);
            transform.localScale = Vector3.one;
        }


        public static Transform ResetPosition(this Transform transform) {
            transform.localPosition = Vector3.zero;
            return transform;
        }

        public static Transform ResetRotation(this Transform transform) {
            transform.localRotation = Quaternion.identity;
            return transform;
        }

        public static Transform ResetScale(this Transform transform) {
            transform.localScale = Vector3.one;
            return transform;
        }

    }
}