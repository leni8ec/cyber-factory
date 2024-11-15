using System.Collections.Generic;
using System.Linq;
using CyberFactory.Basics.Objects;
using CyberFactory.Common.Components;
using CyberFactory.Common.States;
using CyberFactory.Inventories.Queries;
using CyberFactory.Inventories.Services;
using CyberFactory.Inventories.Systems;
using CyberFactory.Plants.Core.Components;
using CyberFactory.Plants.Core.Models;
using CyberFactory.Plants.Production.Systems;
using CyberFactory.Products.Components;
using CyberFactory.Products.Models;
using CyberFactory.Products.Objects;
using NUnit.Framework;
using Scellecs.Morpeh;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace CyberFactory.Tests.Plants {
    public class PlantTests : MorpehTestFixture {

        private InventoryService inventory;
        private ProductModel[] products;
        private Entity[] plants;

        protected override void InitSystems(SystemsGroup systemsGroup) {
            // AddSystem<PlantInitializerSystem>(); - not required for test (plant level is set manual)
            AddSystem<PlantResourceRequestSystem>();
            AddSystem<PlantProductionStartSystem>();
            AddSystem<PlantProductionSystem>();
            AddSystem<PlantProductionCompleteSystem>();

            AddSystem<InventoryOrderSystem>();
            AddSystem<InventoryPullSystem>();
            AddSystem<InventoryServiceSyncSystem>();
        }


        [SetUp]
        public void SetUp() {
            // init products
            products = new ProductModel[5];
            for (int i = 0; i < products.Length; i++) {
                var product = ScriptableObject.CreateInstance<ProductModel>();
                product.price = i + 1;
                product.name = $"Product_{i}";
                products[i] = product;
            }
            // init plants
            plants = new Entity[5];
            for (int i = 0; i < plants.Length; i++) {
                // create plant model
                var plantModel = ScriptableObject.CreateInstance<PlantModel>();
                plantModel.name = $"Plant_{i}";
                plantModel.productionRateLevels = new List<float> { 1 };
                plantModel.product = products[i];
                // create plant entity
                var plantEntity = testWorld.CreateEntity();
                ref var plant = ref plantEntity.AddComponent<Plant>();
                plant.model = plantModel;
                plant.level = 1;
                plants[i] = plantEntity;
            }
            // inventory service
            inventory = new InventoryService();

        }

        [Test]
        public void Production_ResourceRequest() {
            // Process
            foreach (var plant in plants) {
                plant.AddComponent<ActiveState>();
            }
            RunAllSystems(0.1f, 1);

            // Assert
            foreach (var plant in plants) {
                Assert.IsTrue(plant.Has<InventoryProductsOrder>()); // request added
                Assert.IsTrue(plant.Has<OrderApprovedState>()); // request approved
                Assert.IsFalse(plant.Has<Progress>()); // but not started to production
            }
        }

        [Test]
        public void Production_Start() {
            // Process
            foreach (var plant in plants) {
                plant.AddComponent<ActiveState>();
            }
            RunAllSystems(0.1f, 2);

            // Assert
            foreach (var plant in plants) {
                Assert.IsFalse(plant.Has<InventoryProductsOrder>()); // request - removed
                Assert.IsFalse(plant.Has<OrderApprovedState>()); // request approved flag - removed
                Assert.IsTrue(plant.Has<Progress>()); // production started
                Assert.AreApproximatelyEqual(0.1f, plant.GetComponent<Progress>());
            }
        }

        [Test]
        public void Production_Complete_and_GetProduct() {
            // Process
            foreach (var plant in plants) {
                plant.AddComponent<ProductionComplete>();
            }
            RunAllSystemsMultipleTimes();

            // Assert
            foreach (var product in products) {
                Assert.IsTrue(inventory.Has(product));
                Assert.AreEqual(1, inventory.Count(product));
            }
            foreach (var plant in plants) {
                Assert.IsTrue(!plant.Has<ProductionComplete>());
            }
        }

        [Test]
        public void Production_FullCycle() {
            // Process
            foreach (var plant in plants) {
                plant.AddComponent<ActiveState>();
            }
            RunAllSystems(0.1f, 2);

            // Assert production result
            RunAllSystems(1f, 1);
            foreach (var plant in plants) {
                Assert.IsFalse(plant.Has<InventoryProductsOrder>()); // request - removed
                Assert.IsFalse(plant.Has<OrderApprovedState>()); // request approved flag - removed
                Assert.IsFalse(plant.Has<Progress>()); // production not started yed (2nd time)
                Assert.IsTrue(plant.Has<ActiveState>()); // plant is active
            }
            foreach (var product in products) {
                Assert.AreEqual(1, inventory.Count(product));
            }

            // Assert new request (2nd time)
            RunAllSystems(0.1f, 1);
            foreach (var plant in plants) {
                Assert.IsTrue(plant.Has<InventoryProductsOrder>()); // request - removed
                Assert.IsTrue(plant.Has<OrderApprovedState>()); // request approved flag - removed
                Assert.IsFalse(plant.Has<Progress>()); // production started (2nd time)
                Assert.IsTrue(plant.Has<ActiveState>()); // plant is active
            }

            // Assert new production cycle (2nd time)
            RunAllSystems(0.1f, 1);
            foreach (var plant in plants) {
                Assert.IsFalse(plant.Has<InventoryProductsOrder>()); // request - removed
                Assert.IsFalse(plant.Has<OrderApprovedState>()); // request approved flag - removed
                Assert.IsTrue(plant.Has<Progress>()); // production started (2nd time)
                Assert.IsTrue(plant.Has<ActiveState>()); // plant is active
            }

            // Assert production result (2nd time)
            RunAllSystems(1f, 1);
            foreach (var plant in plants) {
                Assert.IsFalse(plant.Has<InventoryProductsOrder>()); // request - removed
                Assert.IsFalse(plant.Has<OrderApprovedState>()); // request approved flag - removed
                Assert.IsFalse(plant.Has<Progress>()); // production not started yed (2nd time)
                Assert.IsTrue(plant.Has<ActiveState>()); // plant is active
            }
            foreach (var product in products) {
                Assert.AreEqual(2, inventory.Count(product));
            }
        }


        [Test]
        [TestCase(new[] { 1 }, new[] { 1 }, ExpectedResult = 1)]
        [TestCase(new[] { 10 }, new[] { 2 }, ExpectedResult = 5)]
        [TestCase(new[] { 10 }, new[] { 5 }, ExpectedResult = 2)]
        [TestCase(new[] { 10 }, new[] { 10 }, ExpectedResult = 1)]
        [TestCase(new[] { 100 }, new[] { 1 }, ExpectedResult = 100)]
        [TestCase(new[] { 0 }, new[] { 1 }, ExpectedResult = 0)]
        [TestCase(new[] { 3 }, new[] { 10 }, ExpectedResult = 0)]
        [TestCase(new[] { 0 }, new[] { 0 }, ExpectedResult = 999)] // unlimited count since the request is dummy
        [TestCase(new[] { 1 }, new[] { 0 }, ExpectedResult = 999)] // unlimited count since the request is dummy
        [TestCase(new[] { 0 }, new[] { 1 }, ExpectedResult = 0)]
        [TestCase(new[] { 2, 2 }, new[] { 1, 1 }, ExpectedResult = 2)]
        [TestCase(new[] { 2, 1 }, new[] { 1, 1 }, ExpectedResult = 1)]
        [TestCase(new[] { 2, 0 }, new[] { 1, 1 }, ExpectedResult = 0)]
        [TestCase(new[] { 2, 0 }, new[] { 0, 1 }, ExpectedResult = 0)]
        [TestCase(new[] { 2, 0 }, new[] { 1, 0 }, ExpectedResult = 2)]
        [TestCase(new[] { 2, 0 }, new[] { 0, 0 }, ExpectedResult = 999)]
        [TestCase(new[] { 10, 20, 30, 40 }, new[] { 1, 2, 3, 4 }, ExpectedResult = 10)]
        public int Production(int[] inventoryProductsCounts, int[] requestProductsCounts) { // 'productsCounts' -> [0..3]
            // Determine max production cycles count
            const int unlimitedProductCount = 999;
            const int extraCyclesCount = 10;
            bool isDummyRequest = requestProductsCounts.All(count => count == 0);
            int productionCyclesCount = 0;
            if (!isDummyRequest) {
                productionCyclesCount = int.MaxValue; // initial value as max
                for (int i = 0; i < inventoryProductsCounts.Length; i++) {
                    if (requestProductsCounts[i] == 0) continue;
                    int possibleCount = inventoryProductsCounts[i] / requestProductsCounts[i];
                    productionCyclesCount = Mathf.Min(productionCyclesCount, possibleCount);
                }
            }
            Debug.Log($"productionCyclesCount: {productionCyclesCount}");

            // Prepare recipe
            var productionProduct = products[4]; // products[0..3] - may be used as recipe items
            var productionPlant = plants[4];
            var recipeProducts = new List<PairValue<ProductModel, int>>(requestProductsCounts.Length);
            for (int i = 0; i < requestProductsCounts.Length; i++) {
                recipeProducts.Add(new PairValue<ProductModel, int>(products[i], requestProductsCounts[i]));
            }
            productionProduct.recipe = new ProductsSet(recipeProducts);

            // Pull initial inventory items
            for (int i = 0; i < inventoryProductsCounts.Length; i++) {
                PullProductToInventory(products[i], inventoryProductsCounts[i]);
            }
            RunAllSystems(1);

            // Start
            productionPlant.AddComponent<ActiveState>();
            RunAllSystems(0f, 1);

            // Assert is request approved
            if (productionCyclesCount > 0 || isDummyRequest) {
                Assert.IsTrue(productionPlant.Has<InventoryProductsOrder>()); // request - removed
                Assert.IsTrue(productionPlant.Has<OrderApprovedState>()); // request approved flag - removed
                Assert.IsFalse(productionPlant.Has<Progress>()); // production not started yed (2nd time)
                Assert.IsTrue(productionPlant.Has<ActiveState>()); // plant is active
            }


            // Production all possible products (run all cycles)
            for (int i = 0; i < productionCyclesCount; i++) {
                RunAllSystems(0f, 2); // prepare cycles
                RunAllSystems(1f, 1); // production cycle
            }
            int productCount = productionCyclesCount == 0 ? 0 : inventory.Count(productionProduct);

            // add extra cycles (to be safe)
            for (int i = 0; i < extraCyclesCount; i++) {
                RunAllSystems(0f, 2); // prepare cycles
                RunAllSystems(1f, 1); // production cycle
            }

            // Final assert for item count
            int finalProductCount = productCount;
            if (inventory.Has(productionProduct))
                finalProductCount = inventory.Count(productionProduct);
            bool unlimitedProduction = finalProductCount > productCount && finalProductCount - productCount == extraCyclesCount;
            if (unlimitedProduction)
                finalProductCount = unlimitedProductCount;

            Debug.Log($"productCount: {productCount}, (expected: {finalProductCount})");
            Assert.AreEqual(finalProductCount, isDummyRequest ? unlimitedProductCount : productCount);

            // Assert remaining products count
            RunAllSystems(0, 5);
            List<int> usedProductsCounts = requestProductsCounts.Select((requestCount, i) =>
                requestCount * productionCyclesCount).ToList();
            List<int> remainingProductsCounts = inventoryProductsCounts.Select((initCount, i) =>
                Mathf.Max(0, initCount - usedProductsCounts[i])).ToList();

            Debug.Log(string.Join(", ", remainingProductsCounts));
            for (int i = 0; i < remainingProductsCounts.Count; i++) {
                int remainingCount = remainingProductsCounts[i];
                if (remainingCount > 0)
                    Assert.AreEqual(remainingCount, inventory.Count(products[i]));
                else
                    Assert.IsFalse(inventory.Has(products[i]));
            }

            return finalProductCount;
        }


        #region Helpers

        private Entity PullProductToInventory(ProductModel product, int count) {
            var entity = testWorld.CreateEntity();
            entity.AddComponent<Product>().model = product;
            entity.AddComponent<Count>().value = count;
            entity.AddComponent<InventoryItemPullCall>();
            return entity;
        }

        #endregion


    }
}