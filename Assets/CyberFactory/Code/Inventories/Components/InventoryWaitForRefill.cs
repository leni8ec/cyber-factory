using System;
using Scellecs.Morpeh;

namespace CyberFactory.Inventories.Components {
    /// <summary>
    /// This request must be approved with `RequestApprovedState`
    /// </summary>
    [Serializable]
    public struct InventoryWaitForRefill : IComponent { }
}