using NUnit.Framework;
using Scellecs.Morpeh;
using VContainer;

namespace CyberFactory.Tests.Fixtures {
    /// <summary>
    /// Register, inject and resolve dependencies, for added systems and tests itself
    /// </summary>
    [TestFixture]
    public abstract class TestFixtureVContainerLayer : TestFixtureMorpehLayer {

        private IObjectResolver resolver;

        [SetUp]
        public override void FixtureSetUp() {
            // Init resolver
            var builder = new ContainerBuilder();
            RegisterDependencies(builder);
            resolver = builder.Build();

            // Inject tests itself
            resolver.Inject(this);

            // use resolver in base implementations
            base.FixtureSetUp();
        }

        [TearDown]
        public void VContainerFixtureTearDown() {
            resolver.Dispose();
            resolver = null;
        }

        protected abstract void RegisterDependencies(IContainerBuilder builder);

        protected override void OnSystemAdded(IInitializer system) {
            base.OnSystemAdded(system);
            resolver.Inject(system);
        }

    }
}