using System.Collections.Generic;
using System.Text;
using CyberFactory.Basics.Constants.Editor;
using CyberFactory.Inventories.Services;
using CyberFactory.Products.Models;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using VContainer;

namespace CyberFactory.Utility.InfoPanel {

    [CreateAssetMenu(menuName = AssetMenu.Utility.SYSTEM + "InfoPanel", fileName = nameof(InfoPanelSystem), order = AssetMenu.Utility.ORDER)]
    public sealed class InfoPanelSystem : LateUpdateSystem {

        [Inject] private InventoryService Inventory { get; init; }

        private Filter filter;
        private StringBuilder stringBuilder;

        public override void OnAwake() {
            filter = World.Filter.With<InfoPanelComponent>().Build();
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

            productsFilter ??= Inventory.Items.Keys; // if product filter is null - get all inventory items
            foreach (var product in productsFilter) {
                if (product && !string.IsNullOrEmpty(product.name)) {
                    stringBuilder.Append($"<b>{product.name}:</b> {Inventory.Count(product)}");
                }

                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
    }
}