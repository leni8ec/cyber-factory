using Scellecs.Morpeh;
using UnityEngine;
using VContainer;

namespace CyberFactory.Basics.Extensions {
    public static class InstallerExtensions {

        public static void ResolveInjection(this Installer installer, IObjectResolver resolver) {
            // Debug.Log("Inject Installer: " + installer.name);
            foreach (var initializer in installer.initializers) InjectSystem(resolver, initializer);
            foreach (var pair in installer.fixedUpdateSystems) InjectSystem(resolver, pair.System);
            foreach (var pair in installer.updateSystems) InjectSystem(resolver, pair.System);
            foreach (var pair in installer.lateUpdateSystems) InjectSystem(resolver, pair.System);
            foreach (var pair in installer.cleanupSystems) InjectSystem(resolver, pair.System);
        }

        private static void InjectSystem<T>(IObjectResolver resolver, T system) where T : ScriptableObject, IInitializer {
            // Debug.Log($"Inject system: {system.name}");
            resolver.Inject(system);
        }

    }
}