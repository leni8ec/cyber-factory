using System.Collections;
using NUnit.Framework;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace CyberFactory.Tests.Fixtures {
    /// <summary>
    /// Create test world with systems<br/>
    /// <br/>
    /// Source: https://github.com/scellecs/morpeh.examples.tanks/blob/master/Assets/Tanks.Code/Tests/EcsTestFixture.cs
    /// </summary>
    [TestFixture]
    public abstract class TestFixtureMorpehLayer {
        protected World testWorld;

        private SystemsGroup testSystems;

        [SetUp]
        public virtual void FixtureSetUp() {
            testWorld = World.Create();
            testWorld.UpdateByUnity = false;

            testSystems = testWorld.CreateSystemsGroup();
            RegisterSystems(testSystems);
            testWorld.AddSystemsGroup(0, testSystems);
            testWorld.Update(0f);
        }

        [TearDown]
        public void MorpehFixtureTearDown() {
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

        /// Called before [SetUp] from derived class
        protected abstract void RegisterSystems(SystemsGroup systemsGroup);


        protected void AddSystem<T>() where T : ScriptableObject, ISystem {
            var system = ScriptableObject.CreateInstance<T>();
            testSystems.AddSystem(system);
            OnSystemAdded(system);
        }

        protected void AddInitializerSystem<T>() where T : Initializer {
            var system = ScriptableObject.CreateInstance<T>();
            testSystems.AddInitializer(system);
            OnSystemAdded(system);
        }

        protected void RegisterAdditionalSystems(ISystem[] systems) {
            SystemsGroup systemsGroup = testWorld.CreateSystemsGroup();
            foreach (ISystem system in systems) {
                systemsGroup.AddSystem(system);
                OnSystemAdded(system);
            }

            testWorld.AddSystemsGroup(1, systemsGroup);
            testWorld.Update(0f);
        }

        protected virtual void OnSystemAdded(IInitializer system) { }

        /// Run all systems multiple times (default: 5) for 1 second each
        protected void RunAllSystemsMultipleTimes(int repeat = 5) {
            for (int i = 0; i < repeat; i++) {
                RunAllSystems(1);
            }
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