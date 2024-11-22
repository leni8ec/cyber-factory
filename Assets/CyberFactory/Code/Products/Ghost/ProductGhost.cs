using System;
using Scellecs.Morpeh;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CyberFactory.Products.Ghost {
    [Serializable]
    public struct ProductGhost : IComponent, IDisposable {

        public ProductGhostConfig config;
        public SpriteRenderer spriteRenderer;
        public Transform transform;

        /// position of the ghost source
        public Vector3 sourcePosition;


        public void Dispose() {
            if (transform) Object.Destroy(transform.gameObject);
        }

    }
}