using CyberFactory.Basics.Constants.Editor;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace CyberFactory.Common.States.Providers {
    [AddComponentMenu(AssetMenu.Plants.SYSTEM + nameof(PauseState))]
    public sealed class PauseStateProvider : MonoProvider<PauseState> { }

}