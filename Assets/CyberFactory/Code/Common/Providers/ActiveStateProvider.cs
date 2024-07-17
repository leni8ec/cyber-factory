using CyberFactory.Common.Components;
using Scellecs.Morpeh.Providers;
using TriInspector;
using UnityEngine;

namespace CyberFactory.Common.Providers {

    [AddComponentMenu("ECS/" + nameof(ActiveState))] [HideMonoScript]
    public sealed class ActiveStateProvider : MonoProvider<ActiveState> { }

}