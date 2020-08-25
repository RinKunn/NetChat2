using System;
using Autofac;
using NetChat2.ViewModel;
using NUnit.Framework;

namespace NetChat2.Tests
{
    [TestFixture]
    public class BuilderTests
    {
        [Test]
        public void BuildTest()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAppServices();
            Assert.DoesNotThrow(() => builder.Build());
        }

        [Test]
        public void ResolveMainWindow()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAppServices();
            var container = builder.Build();

            MainViewModel viewmodel = null;
            using (var scope = container.BeginLifetimeScope())
            {
                viewmodel = scope.Resolve<MainViewModel>();
            }
            
            Assert.NotNull(viewmodel);
            Assert.NotNull(viewmodel);
        }
    }
}
