using System.Threading;
using CyberFactory.Basics.Extensions;
using CyberFactory.Common.Pool;
using CyberFactory.Common.Timer;
using Cysharp.Threading.Tasks;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using Timer = CyberFactory.Common.Timer.Timer;

namespace CyberFactory.Products.Ghost {
    // todo: add disposable to destroy and clear all objects
    public class ProductGhostSpawner {

        [Inject] private ProductGhostFactory Factory { get; init; }
        private readonly EntityPoolAsync pool;

        public ProductGhostSpawner() {
            pool = new EntityPoolAsync(async token => await Factory.CreateAsync(token));
        }

        public async UniTask PrepareAsync(int poolSize, CancellationToken token) {
            await pool.PrepareAsync(poolSize, token);
        }

        public async UniTask<Entity> SpawnAsync(ProductGhostConfig config, AssetReferenceSprite spriteReference, Vector3 sourcePosition, CancellationToken token) {
            var entity = await pool.GetAsync(token);
            var ghost = entity.GetComponent<ProductGhost>();

            // Set variables
            ghost.sourcePosition = sourcePosition;
            ghost.config = config; // todo: move config to DI

            // Load resources
            var sprite = await Addressables.LoadAssetAsync<Sprite>(spriteReference).WithCancellation(token);

            // Configure
            ghost.spriteRenderer.sprite = sprite;
            ghost.spriteRenderer.SetColorAlpha(0); // predefine color value to avoid glitches
            ghost.transform.position = sourcePosition + config.startOffset;
            ghost.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f); // todo: scale (implement in prefab usage)
            ghost.transform.gameObject.SetActive(true);

            entity.SetComponent(ghost); // because 'ref' don't allowed in async methods

            // Add timer
            entity.AddComponent<Timer>() = new Timer {
                interval = config.duration
            };

            return entity;
        }

        public void Despawn(Entity entity) {
            // Reset 
            var ghost = entity.GetComponent<ProductGhost>();
            ghost.transform.gameObject.SetActive(false);
            ghost.spriteRenderer.sprite = null;
            // ghost.transform.ResetPosition(); // it's is not necessary

            // Remove components
            entity.RemoveComponent<Timer>();
            entity.RemoveComponent<TimerComplete>();

            pool.Return(entity);
        }

    }
}