using System;
using CyberFactory.Inventories.Services;
using Scellecs.Morpeh;

namespace CyberFactory.Inventories.Components {

    // todo: remove it after implement DI 'VContainer'
    [Serializable]
    public struct Inventory : IComponent {

        public InventoryService service;

    }

}