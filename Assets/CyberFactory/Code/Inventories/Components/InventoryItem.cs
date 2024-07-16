using System;
using Scellecs.Morpeh;

namespace CyberFactory.Inventories.Components {

    /// <summary>
    /// Inventory item is 'stackable' - if it has a 'Count' component on entity
    /// </summary>
    [Serializable]
    public struct InventoryItem : IComponent { }

}