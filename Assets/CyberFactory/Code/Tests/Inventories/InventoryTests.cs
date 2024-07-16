using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CyberFactory.Basics.Objects;
using CyberFactory.Common.Components;
using CyberFactory.Inventories.Components;
using CyberFactory.Inventories.Requests;
using CyberFactory.Inventories.Services;
using CyberFactory.Inventories.Systems;
using CyberFactory.Products.Components;
using CyberFactory.Products.Models;
using CyberFactory.Products.Objects;
using NUnit.Framework;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.TestTools;

namespace CyberFactory.Tests.Inventories {

    public class InventoryTests : MorpehTestFixture {

        private Filter inventoryItems;
        private ProductModel[] products;
        private InventoryService service;

        // ReSharper disable once GrammarMistakeInComment
        private readonly Regex inventoryPrefixRegex = new("^.Inventory.*"); // has "[Inventory*"

        protected override void InitSystems(SystemsGroup systemsGroup) {
            AddSystem<InventoryServiceSyncSystem>();
        }

        [SetUp]
        public void SetUp() {
            inventoryItems = testWorld.Filter.With<Product>().With<InventoryItem>().Build();
            products = new ProductModel[5];
            for (int i = 0; i < products.Length; i++) {
                var product = ScriptableObject.CreateInstance<ProductModel>();
                product.price = i + 1;
                product.name = $"Product_{i}";
                products[i] = product;
            }
            service = testWorld.Filter.With<Inventory>().Build()
                .FirstOrDefault().GetComponent<Inventory>().service;
        }


        [Test]
        [TestCase(new[] { 1 }, ExpectedResult = 1)]
        [TestCase(new[] { 10 }, ExpectedResult = 10)]
        [TestCase(new[] { -1 }, ExpectedResult = -1)]
        [TestCase(new[] { 0 }, ExpectedResult = -1)]
        [TestCase(new[] { 100, 50, 50 }, ExpectedResult = 200)]
        [TestCase(new[] { 1, 2, 3, 4, 5 }, ExpectedResult = 15)]
        [TestCase(new[] { 1, 1, 1 }, ExpectedResult = 3)]
        [TestCase(new[] { 5, -2, 0 }, ExpectedResult = 5)]
        [TestCase(new[] { 1, 2, -1 }, ExpectedResult = 3)]
        [TestCase(new[] { 0, 2, -1 }, ExpectedResult = 2)]
        [TestCase(new[] { -1, 2, -1 }, ExpectedResult = 2)]
        [TestCase(new[] { -1, -2, 2 }, ExpectedResult = 2)]
        [TestCase(new[] { -1, -3, -9 }, ExpectedResult = -1)]
        [TestCase(new[] { -2, 0, -1 }, ExpectedResult = -1)]
        [TestCase(new[] { 0, 0, 0 }, ExpectedResult = -1)]
        public int PullItems(int[] counts) {
            Debug.Log($"----- Start Test data set: [ {string.Join(", ", counts)} ] -----");

            // Systems
            AddSystem<InventoryPullSystem>();
            // Prepare
            var testProduct = products[0];
            var pullRequests = new Entity[counts.Length];

            // Process
            for (int i = 0; i < counts.Length; i++) {
                pullRequests[i] = PullProductToInventory(testProduct, counts[i]);
                RunAllSystems(1, 5);
            }

            // Assert console errors
            bool isAnyCountInvalid = counts.Any(count => count <= 0);
            bool isAllCountInvalid = counts.All(count => count <= 0);
            if (isAnyCountInvalid) {
                foreach (int count in counts) {
                    if (count <= 0) LogAssert.Expect(LogType.Error, inventoryPrefixRegex);
                }

                if (isAllCountInvalid) return -1;
            }

            // Assert
            Assert.That(inventoryItems.GetLengthSlow() == 1);
            Assert.That(service.ItemsCount == 1);

            // Result
            var inventoryItem = service.Get(testProduct);
            int result = inventoryItem.GetComponent<Count>().value;
            Debug.Log($"----- Result: [ {result} ] -----");
            return result;
        }


        [Test]
        [TestCase(003, new[] { 1, 1, 1 }, ExpectedResult = 0)]
        [TestCase(010, new[] { 1, 2, 3 }, ExpectedResult = 4)]
        [TestCase(100, new[] { 50, 20, 10 }, ExpectedResult = 20)]
        [TestCase(100, new[] { -1, -2, 3 }, ExpectedResult = 97)]
        [TestCase(100, new[] { -1, -1, -1 }, ExpectedResult = 100)]
        [TestCase(100, new[] { -1, -1, 1 }, ExpectedResult = 99)]
        [TestCase(100, new[] { 0, 0, 0 }, ExpectedResult = 100)]
        [TestCase(000, new[] { 1, 1, 1 }, ExpectedResult = 0)]
        [TestCase(-02, new[] { 1, 2, 3 }, ExpectedResult = 0)]
        [TestCase(-02, new[] { -1, 2, 3 }, ExpectedResult = 0)]
        [TestCase(-02, new[] { 1, 2, -3 }, ExpectedResult = 0)]
        public int ReleaseItems(int initCount, int[] counts) {
            Debug.Log($"----- Start Test data set: [ {initCount} ] - [ {string.Join(", ", counts)} ] -----");

            // Systems
            AddSystem<InventoryPullSystem>();
            AddSystem<InventoryReleaseSystem>();
            // Prepare
            var testProduct = products[0];
            var releaseRequests = new Entity[counts.Length];

            // Process
            PullProductToInventory(testProduct, initCount);
            RunAllSystems(1, 5);
            for (int i = 0; i < counts.Length; i++) {
                releaseRequests[i] = ReleaseProductFromInventory(testProduct, counts[i]);
                RunAllSystems(1, 5);
            }

            // Assert console errors
            bool initCountInvalid = initCount <= 0;
            bool isAnyCountInvalid = counts.Any(count => count <= 0);
            if (initCountInvalid) LogAssert.Expect(LogType.Error, inventoryPrefixRegex);
            if (initCountInvalid || isAnyCountInvalid) {
                foreach (int count in counts) {
                    if (initCountInvalid || count <= 0) LogAssert.Expect(LogType.Error, inventoryPrefixRegex);
                }
            }

            // Assert
            int expectedResult = Mathf.Max(0, Mathf.Max(0, initCount) - counts.Where(count => count > 0).Sum());
            int expectedInventoryCount = expectedResult > 0 ? 1 : 0;
            Assert.That(inventoryItems.GetLengthSlow() == expectedInventoryCount);
            Assert.That(service.ItemsCount == expectedInventoryCount);

            // Result
            int result = 0;
            if (expectedInventoryCount > 0) {
                var inventoryItem = service.Get(testProduct);
                result = inventoryItem.GetComponent<Count>().value;
            }
            Debug.Log($"----- Result: [ {result} ] (expected: {expectedResult}) -----");
            Assert.That(result == expectedResult); // check values of the [TestCase]'s
            return result;
        }

        [Test]
        [TestCase(new[] { 1 }, new[] { 1 }, ExpectedResult = true)]
        [TestCase(new[] { 10 }, new[] { 1 }, ExpectedResult = true)]
        [TestCase(new[] { 10 }, new[] { -1 }, ExpectedResult = false)]
        [TestCase(new[] { 10 }, new[] { -3 }, ExpectedResult = false)]
        [TestCase(new[] { 0 }, new[] { 1 }, ExpectedResult = false)]
        [TestCase(new[] { 0 }, new[] { 0 }, ExpectedResult = false)]
        [TestCase(new[] { 0 }, new[] { -1 }, ExpectedResult = false)]
        [TestCase(new[] { -1 }, new[] { 2 }, ExpectedResult = false)]
        [TestCase(new[] { 1, 1, 1 }, new[] { 1, 1, 1 }, ExpectedResult = true)]
        [TestCase(new[] { 10, 10, 10 }, new[] { 1, 1, 1 }, ExpectedResult = true)]
        [TestCase(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, ExpectedResult = true)]
        [TestCase(new[] { 1, 2, 3, 4, 5 }, new[] { 1, 2, 3, 4, 5 }, ExpectedResult = true)]
        [TestCase(new[] { 100, 100, 100 }, new[] { 50, 20, 10 }, ExpectedResult = true)]
        [TestCase(new[] { 10, 10, 10 }, new[] { 1, 2, -3 }, ExpectedResult = false)]
        [TestCase(new[] { 10, 10, 10 }, new[] { -1, -1, -1 }, ExpectedResult = false)]
        [TestCase(new[] { 10, 10, 10 }, new[] { -1, -1, -1 }, ExpectedResult = false)]
        [TestCase(new[] { 10, 10, 10 }, new[] { -1, -1, 1 }, ExpectedResult = false)]
        [TestCase(new[] { 10, 10, -10 }, new[] { 10, 10 }, ExpectedResult = true)]
        [TestCase(new[] { 0, 0, 0 }, new[] { 0, 0, 0 }, ExpectedResult = false)]
        [TestCase(new[] { 10, 10, 10 }, new[] { 10, 10 }, ExpectedResult = true)]
        [TestCase(new[] { 10, 10 }, new[] { 10, 10, 10 }, ExpectedResult = false)]
        public bool RequestItems(int[] initCounts, int[] requestItemsCounts) {
            Debug.Log($"----- Start Test data set | init: [ {string.Join(", ", initCounts)} ] | requests: [ {string.Join(", ", requestItemsCounts)} ] -----");

            // Systems
            AddSystem<InventoryPullSystem>();
            AddSystem<InventoryRequestSystem>();

            // Prepare
            // 1. Init inventory items
            for (int i = 0; i < initCounts.Length; i++) {
                PullProductToInventory(products[i], initCounts[i]);
                if (initCounts[i] <= 0) LogAssert.Expect(LogType.Error, inventoryPrefixRegex);
            }
            RunAllSystems(1, 5);

            // 2. Validate request items counts
            bool recipeHasInvalidValue = requestItemsCounts.Any(count => count <= 0);
            if (recipeHasInvalidValue) LogAssert.Expect(LogType.Error, inventoryPrefixRegex);

            // Process
            var requestEntity = RequestProductsFromInventory(products, requestItemsCounts);
            RunAllSystems(1, 5);

            // Assert is request approved
            bool isRequestApproved = requestEntity.Has<RequestApprovedState>();

            // Assert remaining items count
            int n = Mathf.Min(initCounts.Length, requestItemsCounts.Length);
            for (int i = 0; i < n; i++) {
                var product = products[i];
                int remainingCount = service.Has(product) ? service.Get(product).GetComponent<Count>().value : 0;
                int expectedRemainingCount = Mathf.Max(0, initCounts[i]) - (isRequestApproved ? Mathf.Max(0, requestItemsCounts[i]) : 0);

                // Debug.Log($"Remaining items: [{i}] '{product.name}' -> '{remainingCount}' (expected: '{expectedRemainingCount}')");

                Assert.AreEqual(remainingCount, expectedRemainingCount);
                if (expectedRemainingCount <= 0) {
                    Assert.That(!service.Has(product));
                }
            }

            Debug.Log($"----- Result (request approved): {isRequestApproved} -----");
            return isRequestApproved;
        }

        // todo
        [Test]
        public void Sequence1_Pull_Request() {
            // Systems
            AddSystem<InventoryPullSystem>();
            AddSystem<InventoryRequestSystem>();

            // Process
            var product = products[0];
            PullProductToInventory(product, 10);
            RunAllSystems(1, 5);
            var requestEntity = RequestProductFromInventory(product, 5);
            RunAllSystems(1, 5);

            // Assert
            bool isRequestApproved = requestEntity.Has<RequestApprovedState>();
            Assert.That(isRequestApproved);
        }

        [Test]
        public void Sequence2_Request_Pull() {
            // Systems
            AddSystem<InventoryPullSystem>();
            AddSystem<InventoryRequestSystem>();

            // Process
            var product = products[0];
            var requestEntity = RequestProductFromInventory(product, 5);
            RunAllSystems(1, 5);
            PullProductToInventory(product, 10);
            RunAllSystems(1, 5);

            // Assert
            bool isRequestApproved = requestEntity.Has<RequestApprovedState>();
            Assert.That(isRequestApproved);
        }


        private Entity PullProductToInventory(ProductModel product, int count) {
            var entity = testWorld.CreateEntity();
            entity.AddComponent<Product>().model = product;
            entity.AddComponent<Count>().value = count;
            entity.AddComponent<InventoryItemPullRequest>();
            return entity;
        }

        private Entity ReleaseProductFromInventory(ProductModel product, int count) {
            var entity = testWorld.CreateEntity();
            entity.AddComponent<Product>().model = product;
            entity.AddComponent<Count>().value = count;
            entity.AddComponent<InventoryItemReleaseRequest>();
            return entity;
        }


        private Entity RequestProductFromInventory(ProductModel product, int count) {
            return RequestProductsFromInventory(new[] { product }, new[] { count });
        }

        private Entity RequestProductsFromInventory(ProductModel[] products, int[] counts) {
            var requestRecipe = new List<PairValue<ProductModel, int>>(counts.Length);
            for (int i = 0; i < counts.Length; i++) {
                requestRecipe.Add(new PairValue<ProductModel, int>(products[i], counts[i]));
            }
            var productSet = new ProductsSet(requestRecipe);
            return RequestProductSetFromInventory(productSet);
        }


        private Entity RequestProductSetFromInventory(ProductsSet productsSet) {
            Debug.Log($"ProductSet: {productsSet}");
            var entity = testWorld.CreateEntity();
            entity.AddComponent<InventoryProductsRequest>().products = productsSet;
            return entity;
        }

    }

}