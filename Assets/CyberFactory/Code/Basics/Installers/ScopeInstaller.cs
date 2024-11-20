using CyberFactory.Basics.Extensions;
using Scellecs.Morpeh;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CyberFactory.Basics.Installers {
    /// <summary>
    /// Scope installer wrapper <br/><br/>
    /// Must be placed to a same game object with a "Morpeh Installer" to inject it. <br/>
    /// Auto added self gameobject to "autoInjectGameObjects" field  
    /// </summary>
    public abstract class ScopeInstaller : LifetimeScope {

        [Inject] private IObjectResolver Resolver { get; init; }

        protected override void Awake() {
            // add self gameobject to auto-injection
            if (!autoInjectGameObjects.Contains(gameObject))
                autoInjectGameObjects.Add(gameObject);

            base.Awake();
        }

        private void Start() {
            InjectInstaller();
        }

        private void InjectInstaller() {
            var installer = GetComponent<Installer>();
            if (!installer) return;

            installer.ResolveInjection(Resolver);
        }

        protected override void Configure(IContainerBuilder builder) {
            base.Configure(builder);

            Debug.Log($"Configuring scope installer: {name}");
            Install(builder);
        }

        protected abstract void Install(IContainerBuilder builder);

    }
}