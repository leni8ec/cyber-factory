using System;
using CyberFactory.Common.Queries;

namespace CyberFactory.Inventories.Queries {
    /// <summary>
    /// This request must be approved with `RequestApprovedState`
    /// </summary>
    [Serializable]
    public struct InventoryRefillPending : IQueryComponent { }
}