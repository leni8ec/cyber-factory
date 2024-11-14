using CyberFactory.Basics.Constants.Editor;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace CyberFactory.Common.Components.Providers {
    [AddComponentMenu(AssetMenu.Common.PROVIDER + nameof(Progress))]
    public sealed class ProgressProvider : MonoProvider<Progress> { }
}