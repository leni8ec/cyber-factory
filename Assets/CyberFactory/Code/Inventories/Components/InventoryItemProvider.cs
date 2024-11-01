using Scellecs.Morpeh.Providers;
using TriInspector;
using UnityEngine;

namespace CyberFactory.Inventories.Components {

    [AddComponentMenu("Cyber Factory/Inventory/" + nameof(InventoryItem))] [HideMonoScript]
    public sealed class InventoryItemProvider : MonoProvider<InventoryItem> { }

}