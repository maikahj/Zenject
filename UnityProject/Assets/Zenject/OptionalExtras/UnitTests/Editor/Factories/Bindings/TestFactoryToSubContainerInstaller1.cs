using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using System.Linq;
using ModestTree;
using Assert=ModestTree.Assert;

namespace Zenject.Tests.Bindings
{
    [TestFixture]
    public class TestFactoryToSubContainerInstaller1 : TestWithContainer
    {
        [Test]
        public void TestSelf()
        {
            Container.BindFactory<string, Foo, Foo.Factory>().ToSubContainerSelf<FooInstaller>();

            AssertValidates();

            Assert.IsEqual(Container.Resolve<Foo.Factory>().Create("asdf").Value, "asdf");
        }

        [Test]
        public void TestConcrete()
        {
            Container.BindFactory<string, IFoo, IFooFactory>().ToSubContainer<Foo, FooInstaller>();

            AssertValidates();

            Assert.IsEqual(Container.Resolve<IFooFactory>().Create("asdf").Value, "asdf");
        }

        class FooInstaller : Installer
        {
            readonly string _value;

            public FooInstaller(string value)
            {
                _value = value;
            }

            public override void InstallBindings()
            {
                Container.Bind<Foo>().ToSelf().WithArgumentsExplicit(
                    InjectUtil.CreateArgListExplicit(_value));
            }
        }

        interface IFoo
        {
            string Value
            {
                get;
            }

        }

        class IFooFactory : Factory<string, IFoo>
        {
        }

        class Foo : IFoo
        {
            public Foo(string value)
            {
                Value = value;
            }

            public string Value
            {
                get;
                private set;
            }

            public class Factory : Factory<string, Foo>
            {
            }
        }
    }
}
