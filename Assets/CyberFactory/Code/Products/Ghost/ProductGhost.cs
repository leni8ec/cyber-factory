using System;
using Scellecs.Morpeh;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CyberFactory.Products.Ghost {
    [Serializable]
    public struct ProductGhost : IComponent, IDisposable {

        public Transform transform;
        public SpriteRenderer spriteRenderer;
        public ProductGhostConfig config;

        /// position of the ghost source
        public Vector3 sourcePosition;


        public void Dispose() {
            Object.Destroy(transform.gameObject);
        }

    }
}