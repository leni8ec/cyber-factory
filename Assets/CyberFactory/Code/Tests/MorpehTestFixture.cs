using System.Collections;
using NUnit.Framework;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace CyberFactory.Tests {

    /// <summary>
    /// Source: https://github.com/scellecs/morpeh.examples.tanks/blob/master/Assets/Tanks.Code/Tests/EcsTestFixture.cs
    /// </summary>
    [TestFixture]
    public abstract class MorpehTestFixture {
        protected World testWorld;

        private SystemsGroup testSystems;

        [SetUp]
        public void FixtureSetUp() {
            testWorld = World.Create();
            testWorld.UpdateByUnity = false;

            testSystems = testWorld.CreateSystemsGroup();
            InitSystems(testSystems);
            testWorld.AddSystemsGroup(0, testSystems);
            testWorld.Update(0f);
        }

        [TearDown]
        public void FixtureTearDown() {
            testSystems.Dispose();
            testSystems = null;

            testWorld.Dispose();
            testWorld = null;
        }

        [UnityTearDown]
        public IEnumerator SceneTearDown() {
            Scene scene = SceneManager.GetActiveScene();
            foreach (GameObject o in scene.GetRootGameObjects()) {
                if (o.name.EndsWith("tests runner")) {
                    continue;
                }

                Object.Destroy(o);
            }

            yield return null;
        }

        protected abstract void InitSystems(SystemsGroup systemsGroup);

        protected void AddSystem<T>() where T : ScriptableObject, ISystem {
            var systemInstance = ScriptableObject.CreateInstance<T>();
            testSystems.AddSystem(systemInstance);
        }

        protected void RegisterAdditionalSystems(ISystem[] systems) {
            SystemsGroup systemsGroup = testWorld.CreateSystemsGroup();
            foreach (ISystem system in systems) {
                systemsGroup.AddSystem(system);
            }

            testWorld.AddSystemsGroup(1, systemsGroup);
            testWorld.Update(0f);
        }

        protected void RunAllSystems(float dt, int repeat) {
            for (int i = 0; i < repeat; i++) {
                RunAllSystems(dt);
            }
        }

        protected void RunAllSystems(float dt) {
            RefreshFilters();
            testWorld.FixedUpdate(dt);
            RefreshFilters();
            testWorld.Update(dt);
            RefreshFilters();
            testWorld.LateUpdate(dt);
            RefreshFilters();
            testWorld.CleanupUpdate(dt);
            RefreshFilters();
        }

        protected void RunFixedSystems() {
            RefreshFilters();
            testWorld.FixedUpdate(Time.fixedDeltaTime);
            RefreshFilters();
        }

        protected void RunUpdateSystems(float dt) {
            RefreshFilters();
            testWorld.Update(dt);
            RefreshFilters();
        }

        protected void RunLateUpdateSystems(float dt) {
            RefreshFilters();
            testWorld.LateUpdate(dt);
            RefreshFilters();
        }

        protected void RunCleanUpUpdateSystems(float dt) {
            RefreshFilters();
            testWorld.CleanupUpdate(dt);
            RefreshFilters();
        }

        protected void RefreshFilters() {
            testWorld.Commit();
        }
    }

}