using System;
using CyberFactory.Inventory.Services;
using Scellecs.Morpeh;

namespace CyberFactory.Inventory {

    [Serializable]
    public struct Inventory : IComponent {

        public InventoryService service;

    }

}