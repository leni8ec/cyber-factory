using System;
using CyberFactory.Inventories.Services;
using Scellecs.Morpeh;

namespace CyberFactory.Inventories.Components {

    [Serializable]
    public struct Inventory : IComponent {

        public InventoryService service;

    }

}