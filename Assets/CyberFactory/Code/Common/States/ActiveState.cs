using System;
using Scellecs.Morpeh;

namespace CyberFactory.Common.States {
    /// The entity is active (enabled)
    /// <para/>
    /// Plant: may be waiting for resources ('InventoryProductsRequest') or produce a product ('ProductionState')
    [Serializable] public struct ActiveState : IComponent { }
}