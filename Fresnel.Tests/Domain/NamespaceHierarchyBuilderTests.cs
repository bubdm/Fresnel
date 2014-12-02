//  Author:
//       Vijay Patel
//
// Copyright (c) 2014 Vijay Patel
//
using NUnit.Framework;
using Autofac;
using System;
using System.Linq;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel;
using Envivo.Fresnel.Bootstrap;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Assemblies;
using System.Reflection;
using System.Collections.Generic;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class NamespaceHierarchyBuilderTests
    {
        [Test()]
        public void ShouldCreateHierarchy()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var hierarchyBuilder = container.Resolve<NamespaceHierarchyBuilder>();
            
            // Act:
            var hierarchy = hierarchyBuilder.BuildTreeFor(typeof(SampleModel.Objects.PocoObject).Assembly);

            // Assert:
            Assert.IsNotNull(hierarchy);

            var productNode = hierarchy.FindNodeByName("Product");
            Assert.AreNotEqual(0, productNode.Children.Count());

            Assert.IsTrue(productNode.Children.All(c => c.IsSubClass));
        }

    }
}

