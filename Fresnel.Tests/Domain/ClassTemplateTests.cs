//  Author:
//       Vijay Patel
//
// Copyright (c) 2014 Vijay Patel
//
using NUnit.Framework;
using Autofac;
using System;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel;
using Envivo.Fresnel.Bootstrap;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class ClassTemplateTests
    {
        [Test()]
        public void ShouldCreateClassTemplate()
        {
            var container = new ContainerFactory().Build();

            var builder = container.Resolve<ClassTemplateBuilder>();

            var template = builder.CreateTemplate(typeof(SampleModel.BasicTypes.TextValues));

            Assert.IsNotNull(template);
        }

    }
}

