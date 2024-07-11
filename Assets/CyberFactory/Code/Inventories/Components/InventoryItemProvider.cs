using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace CyberFactory.Inventories.Components {

    [AddComponentMenu("Providers/InventoryItem" + nameof(InventoryItem))]
    public sealed class InventoryItemProvider : MonoProvider<InventoryItem> { }

}