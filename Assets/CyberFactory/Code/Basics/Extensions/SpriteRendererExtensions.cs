using UnityEngine;

namespace CyberFactory.Basics.Extensions {
    public static class SpriteRendererExtensions {

        public static float GetAlpha(this SpriteRenderer spriteRenderer) {
            return spriteRenderer.color.a;
        }

        public static void SetColorAlpha(this SpriteRenderer spriteRenderer, float alpha) {
            var color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }

    }
}