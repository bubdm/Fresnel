//  Author:
//       Vijay Patel
//
// Copyright (c) 2014 Vijay Patel
//
using NUnit.Framework;
using System;
using Envivo.Fresnel.SampleModel.Objects;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class CollectionTests
    {
        [Test()]
        public void TestCase()
        {
            var source = new BiDirectionalExample();
            var target = new BiDirectionalExample();

            source.AddToContents(target);
            Assert.IsTrue(source.Contents.Contains(target));
            Assert.IsTrue(target.Contents.Contains(source));
        }

    }
}

