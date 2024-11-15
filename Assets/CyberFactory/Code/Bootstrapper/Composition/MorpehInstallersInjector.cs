using Scellecs.Morpeh;
using UnityEngine;
using VContainer;

namespace CyberFactory.Bootstrapper.Composition {
    [DefaultExecutionOrder(-4999)]
    public class MorpehInstallersInjector : MonoBehaviour {

        [Inject] private IObjectResolver Resolver { get; init; }

        [SerializeField] private Transform installersRoot;


        private void Awake() {
            InjectInstallers();
        }

        private void InjectInstallers() {
            Installer[] installers = installersRoot.GetComponentsInChildren<Installer>();
            foreach (var installer in installers) {
                // Debug.Log("Installer: " + installer.name);
                foreach (var initializer in installer.initializers) InjectSystem(initializer);
                foreach (var pair in installer.fixedUpdateSystems) InjectSystem(pair.System);
                foreach (var pair in installer.updateSystems) InjectSystem(pair.System);
                foreach (var pair in installer.lateUpdateSystems) InjectSystem(pair.System);
                foreach (var pair in installer.cleanupSystems) InjectSystem(pair.System);
            }
        }


        private void InjectSystem<T>(T system) where T : ScriptableObject, IInitializer {
            // Debug.Log($"Inject: {system.name}");
            Resolver.Inject(system);
        }

    }
}