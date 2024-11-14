using System.Collections.Generic;
using System.Text;
using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Inventories.Components;
using CyberFactory.Inventories.Services;
using CyberFactory.Products.Models;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace CyberFactory.Utility.InfoPanel {

    [CreateAssetMenu(menuName = AssetMenu.Utility.SYSTEM + "InfoPanel", fileName = nameof(InfoPanelSystem), order = AssetMenu.Utility.ORDER)]
    public sealed class InfoPanelSystem : LateUpdateSystem {
        private InventoryService inventory;
        private Filter filter;

        private StringBuilder stringBuilder;

        public override void OnAwake() {
            filter = World.Filter.With<InfoPanelComponent>().Build();

            var inventories = World.Filter.With<Inventory>().Build();
            inventory = inventories.FirstOrDefault().GetComponent<Inventory>().service;

            stringBuilder = new StringBuilder();

        }

        // todo: use events instead update
        public override void OnUpdate(float deltaTime) {
            foreach (var entity in filter) {
                var infoPanel = entity.GetComponent<InfoPanelComponent>();

                string text = CollectProductsInfo(infoPanel.useProductFilter ? infoPanel.productFilter : null);
                infoPanel.text.SetText(text);
            }
        }

        private string CollectProductsInfo(IEnumerable<ProductModel> productsFilter) {
            stringBuilder.Clear();

            productsFilter ??= inventory.Items.Keys; // if product filter is null - get all inventory items
            foreach (var product in productsFilter) {
                if (product && !string.IsNullOrEmpty(product.name)) {
                    stringBuilder.Append($"<b>{product.name}:</b> {inventory.Count(product)}");
                }

                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
    }
}