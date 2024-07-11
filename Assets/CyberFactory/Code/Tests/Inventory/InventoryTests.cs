using CyberFactory.Common;
using CyberFactory.Inventory;
using CyberFactory.Inventory.Services;
using CyberFactory.Products;
using NUnit.Framework;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.TestTools;

namespace CyberFactory.Tests.Inventory {

    public class InventoryTests : MorpehTestFixture {

        private Filter inventoryItems;
        private ProductVariant[] products;
        private InventoryService service;

        protected override void InitSystems(SystemsGroup systemsGroup) {
            AddSystem<InventorySystem>();
            AddSystem<InventoryPullSystem>();
        }

        [SetUp]
        public void SetUp() {
            inventoryItems = testWorld.Filter.With<Product>().With<InventoryItem>().Build();
            products = new ProductVariant[5];
            for (int i = 0; i < products.Length; i++) {
                var product = ScriptableObject.CreateInstance<ProductVariant>();
                product.price = i + 1;
                products[i] = product;
            }
        }


        [Test]
        [TestCase(100, 50, 50, ExpectedResult = 200)]
        [TestCase(1, 1, 1, ExpectedResult = 3)]
        [TestCase(5, 2, 0, ExpectedResult = 7)]
        public long TestItemsCount(long initCount, long pull1Count, long pull2Count) {
            service = testWorld.Filter.With<CyberFactory.Inventory.Inventory>().Build()
                .FirstOrDefault().GetComponent<CyberFactory.Inventory.Inventory>().service;

            // Process
            Debug.Log("----- Start -----");
            var inventoryItem = PullEntityToInventory(products[0], initCount);
            RunAllSystems(1, 3);
            Debug.Log("----- Pull 1 -----");
            var pull1Item = PullEntityToInventory(products[0], pull1Count);
            RunAllSystems(1, 3);
            Debug.Log("----- Pull 2 -----");
            var pull2Item = PullEntityToInventory(products[0], pull2Count);
            RunAllSystems(1, 3);
            Debug.Log("----- End -----");

            // Assert
            if (initCount <= 0 || pull1Count <= 0 || pull2Count <= 0) LogAssert.Expect(LogType.Error, "The 'Count' must be > '0'");
            Assert.That(inventoryItems.GetLengthSlow() == 1);
            Assert.That(service.Items.Count == 1);
            Assert.That(service.Items[products[0]].GetComponent<Count>().value == inventoryItem.GetComponent<Count>().value);
            return inventoryItem.GetComponent<Count>().value;
        }

        private Entity PullEntityToInventory(ProductVariant product, long count) {
            var entity = testWorld.CreateEntity();
            entity.SetComponent(new Product { product = product });
            entity.SetComponent(new Count { value = count });
            entity.AddComponent<PullToInventory>();
            return entity;
        }

    }

}