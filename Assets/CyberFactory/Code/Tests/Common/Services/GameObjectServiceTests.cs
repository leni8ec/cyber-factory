using System;
using CyberFactory.Basics.Utils;
using CyberFactory.Common.Services.GameObjects;
using NUnit.Framework;
using UnityEngine;

namespace CyberFactory.Tests.Common.Services {
    public class GameObjectServiceTests {

        private GameObjectsService service;

        [SetUp]
        public void SetUp() {
            service = new GameObjectsService();
        }

        [Test]
        [TestCase()]
        [TestCase(null)]
        [TestCase("null")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("New object")]
        public void GO_Create_Test(string name = null) {
            if (name == null) name = "New Game Object";
            var gameObject = service.Create(name);
            Assert.IsNotNull(gameObject);
            Assert.AreEqual(name, gameObject.name);
        }

        [Test]
        [TestCase(null, null)]
        [TestCase("null", "null")]
        [TestCase("", "")]
        [TestCase(" ", " ")]
        [TestCase("New object", "New object")]
        public void GO_Create_Multiple_Test(params string[] names) {
            foreach (string name in names) {
                GO_Create_Test(name);
            }
        }

        [TestCase()]
        [TestCase(typeof(SpriteRenderer), typeof(RectTransform))]
        [TestCase(typeof(BoxCollider2D), typeof(Rigidbody2D))]
        public void GO_Create_Components_Test(params Type[] components) {
            var gameObject = service.Create(null, components);

            foreach (var componentType in components) {
                Assert.IsInstanceOf(componentType, gameObject.GetComponent(componentType));
            }
        }

        [TestCase("parent/go")]
        [TestCase("parent")]
        [TestCase("parent/parent2/go")]
        [TestCase("parent/parent2/go1/go2")]
        [TestCase("parent/parent2/go1/go2/go3")]
        [TestCase("sample/parent2/go1/go2/go3")]
        [TestCase("/root")]
        [TestCase("/sample")]
        [TestCase("/sample/parent")]
        public void Check_GO_Path_After_Create_GO_By_Path(in string path) {
            const string gameObjectName = "gameObject";

            string expectedPath;
            if (PathUtil.IsRoot(path))
                expectedPath = PathUtil.CombineForced(path.Substring(1, path.Length - 1), gameObjectName);
            else
                expectedPath = PathUtil.CombineForced(GameObjectsService.DEFAULT_ROOT_PATH, path, gameObjectName);

            var gameObject = service.Create(path, gameObjectName);
            string gameObjectPath = service.GetPath(gameObject);

            Assert.AreEqual(expectedPath, gameObjectPath);

            Debug.Log($"Expected path: {expectedPath}");
            Debug.Log($"Actual path: {gameObjectPath}");
        }


        [TestCase("root")]
        [TestCase("root", "go")]
        [TestCase("root", "parent", "go")]
        [TestCase("root", "parent", "go1", "go2")]
        [TestCase("root", "parent", "go1", "go2", "go3")]
        [TestCase("", "parent")]
        [TestCase(" ", "parent")]
        public void Check_GO_Path_After_Create_GO_By_Path_Parts(params string[] pathParts) {
            string expectedPath = PathUtil.CombineForced(pathParts);

            GameObject gameObject = null;
            Transform parent = null;
            foreach (string part in pathParts) {
                gameObject = new GameObject(part);
                if (parent) gameObject.transform.parent = parent;
                parent = gameObject.transform;
            }
            string testPath = service.GetPath(gameObject);

            Assert.AreEqual(expectedPath, testPath);

            Debug.Log(expectedPath);
            Debug.Log(testPath);
        }

    }
}