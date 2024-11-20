using UnityEngine;
using VContainer.Unity;

namespace CyberFactory.Bootstrapper {
    public class EntryPoint : IInitializable, IStartable {

        public void Initialize() {
            Debug.Log("EntryPoint - initialized");
        }

        public void Start() {
            Debug.Log("EntryPoint - started");
        }

    }
}