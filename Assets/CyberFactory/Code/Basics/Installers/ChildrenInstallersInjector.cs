using CyberFactory.Basics.Extensions;
using Scellecs.Morpeh;
using UnityEngine;
using VContainer;

namespace CyberFactory.Basics.Installers {
    [DefaultExecutionOrder(-4999)]
    public class ChildrenInstallersInjector : MonoBehaviour {

        [Inject] private IObjectResolver Resolver { get; init; }

        [SerializeField] private Transform installersRoot;


        private void Awake() {
            InjectInstallers();
        }

        private void InjectInstallers() {
            Installer[] installers = installersRoot.GetComponentsInChildren<Installer>();
            foreach (var installer in installers) installer.ResolveInjection(Resolver);
        }

    }
}