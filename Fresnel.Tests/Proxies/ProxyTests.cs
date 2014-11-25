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
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Proxies;
using System.ComponentModel;

namespace Envivo.Fresnel.Tests.Proxies
{
    [TestFixture()]
    public class ProxyTests
    {

        [Test()]
        public void ShouldCreateProxyForDomainObject()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var proxyCache = container.Resolve<ProxyCache>();

            // Act:
            var poco = new SampleModel.Objects.PocoObject();
            var pocoProxy = proxyCache.GetProxy(poco);
            var proxy = pocoProxy as IFresnelProxy;

            // Assert:
            Assert.IsInstanceOf<IFresnelProxy>(pocoProxy);
        }

        //[Test]
        //public void ShouldCreateViewModelForDomainCollection()
        //{
        //    var coll = new Collection<PocoObject>();
        //    var collVM = My.Instance.Engine.ViewModelCache.GetViewModel(coll);
        //    var proxy = collVM as IFresnelProxy;

        //    Assert.IsNotNull(proxy);
        //}

        //[Test]
        //public void ShouldExposePocoMembers()
        //{
        //    var poco = new SampleModel.Objects.PocoObject();
        //    var pocoProxy = My.Instance.Engine.ViewModelCache.GetViewModel(poco);
        //    var proxy = pocoProxy as IFresnelProxy;

        //    Assert.IsNotNull(proxy.Meta.Properties["NormalDate"]);
        //    Assert.IsNotNull(proxy.Meta["NormalDate"]);
        //}

        //[Test]
        //public void ShouldExposeChangeTrackingMembers()
        //{
        //    var poco = new SampleModel.Objects.PocoObject();
        //    var pocoProxy = My.Instance.Engine.ViewModelCache.GetViewModel(poco);
        //    var proxy = pocoProxy as IFresnelProxy;

        //    Assert.IsNotNull(proxy.Meta.ChangeTracker);
        //    Assert.IsTrue(proxy.Meta.ChangeTracker.IsNewInstance);
        //}

        [Test()]
        public void ShouldAttachINotifyPropertyChangedToProxy()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var proxyCache = container.Resolve<ProxyCache>();

            // Act:
            var poco = new SampleModel.Objects.PocoObject();
            var pocoProxy = proxyCache.GetProxy(poco);

            Assert.IsInstanceOf<INotifyPropertyChanged>(pocoProxy);

            var proxy = pocoProxy as INotifyPropertyChanged;
            var propertyChanges = new List<string>();
            proxy.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
            {
                propertyChanges.Add(e.PropertyName);
            };

            pocoProxy.FormattedText = "This is a new string";

            // Assert:
            Assert.AreNotEqual(0, propertyChanges.Count);
        }

        [Test()]
        public void ShouldConvertPropertyValueToProxy()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var proxyCache = container.Resolve<ProxyCache>();

            // Act:
            var poco = new SampleModel.Objects.PocoObject();
            var pocoProxy = proxyCache.GetProxy(poco);

            // Assert:
            var childObjects = pocoProxy.ChildObjects;

            Assert.IsInstanceOf<IFresnelProxy>(pocoProxy.ChildObjects);
        }

        //[Test]
        //public void ShouldReportPropertyChanges()
        //{
        //    var poco = new SampleModel.Objects.PocoObject();
        //    var pocoProxy = My.Instance.Engine.ViewModelCache.GetViewModel(poco);
        //    var proxy = pocoProxy as IFresnelProxy;

        //    var propertyChanges = new List<string>();
        //    ((INotifyPropertyChanged)pocoProxy).PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
        //    {
        //        propertyChanges.Add(e.PropertyName);
        //    };

        //    Assert.IsTrue(proxy.Meta.ChangeTracker.IsNewInstance);
        //    pocoProxy.NormalText = "Text was changed at " + DateTime.Now.ToString();
        //    Assert.IsTrue(proxy.Meta.ChangeTracker.IsDirty);

        //    Assert.AreNotEqual(0, propertyChanges.Count);
        //}

        //[Test]
        //public void ShouldDetectCollectionAddOperations()
        //{
        //    var poco = new BiDirectionalExample();
        //    var pocoProxy = My.Instance.Engine.ViewModelCache.GetViewModel(poco);
        //    var proxy = pocoProxy as IFresnelProxy;

        //    Assert.IsFalse(proxy.Meta.ChangeTracker.HasDirtyChildren);
        //    var child1 = new BiDirectionalExample();
        //    var child2 = new BiDirectionalExample();
        //    var child3 = new BiDirectionalExample();

        //    pocoProxy.Contents.Add(child1);
        //    pocoProxy.Contents.Add(child2);
        //    pocoProxy.Contents.Add(child3);

        //    Assert.AreEqual(3, poco.Contents.Count);

        //    var childObjectsVM = pocoProxy.Contents as IFresnelProxy;
        //    Assert.IsTrue(childObjectsVM.Meta.ChangeTracker.HasDirtyChildren);
        //}

        //[Test]
        //public void ShouldDetectCollectionRemoveOperations()
        //{
        //    var poco = new SampleModel.Objects.PocoObject();
        //    var child = new SampleModel.Objects.PocoObject();
        //    poco.ChildObjects.Add(child);

        //    var pocoProxy = My.Instance.Engine.ViewModelCache.GetViewModel(poco);
        //    var proxy = pocoProxy as IFresnelProxy;

        //    Assert.IsFalse(proxy.Meta.ChangeTracker.HasDirtyChildren);

        //    pocoProxy.ChildObjects.Remove(child);
        //    Assert.AreEqual(0, poco.ChildObjects.Count);

        //    Assert.IsTrue(proxy.Meta.ChangeTracker.HasDirtyChildren);
        //}

        [Test()]
        public void ShouldDetectMethodInvoke()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var proxyCache = container.Resolve<ProxyCache>();

            // Act:
            var poco = new SampleModel.Objects.PocoObject();
            var pocoProxy = proxyCache.GetProxy(poco);

            pocoProxy.AddSomeChildObjects();

            // Assert:
            Assert.AreEqual(3, pocoProxy.ChildObjects.Count);
        }

        //[Test]
        //public void ShouldDetectNumberOfDirtyObjects()
        //{
        //    var poco = new SampleModel.Objects.PocoObject();
        //    var pocoProxy = My.Instance.Engine.ViewModelCache.GetViewModel(poco);
        //    var proxy = pocoProxy as IFresnelProxy;

        //    // This ensures that the Method can be invoked, and not complain:
        //    Assert.IsTrue(proxy.Meta.ChangeTracker.IsNewInstance);
        //    My.Instance.Session.Save(poco);

        //    Assert.IsFalse(proxy.Meta.ChangeTracker.IsNewInstance);

        //    // The ChildObjects contents are only added to the object graph when they're firt accessed:
        //    var dummy = pocoProxy.ChildObjects;

        //    pocoProxy.AddSomeChildObjects();

        //    // 4 = ChildObjects + 3 * (Poco Object + it's ChildObject):
        //    Assert.AreEqual(4, proxy.Meta.ChangeTracker.DirtyObjectGraph.Count);
        //}

        //[Test]
        //public void ShouldAllowExplicitAccessToLazyProperties()
        //{
        //    My.Instance.Engine.ViewModelCache.Behaviour.IsAutoLoadingEnabled = false;

        //    var poco = new SampleModel.Objects.PocoObject();
        //    var pocoProxy = My.Instance.Engine.ViewModelCache.GetViewModel(poco);
        //    var proxy = pocoProxy as IFresnelProxy;

        //    Assert.IsNull(pocoProxy.ChildObjects);

        //    proxy.Meta.Properties["ChildObjects"].GetValueByForce();

        //    Assert.IsNotNull(pocoProxy.ChildObjects);
        //}

        [Test()]
        public void ShouldDenyProxiesForSystemTypes()
        {
            // TODO : Change this to System.MulticastDelegate
            // Arrange:
            var container = new ContainerFactory().Build();
            var proxyCache = container.Resolve<ProxyCache>();

            // Act:
            var obj = new object();

            // Assert:
            Assert.Throws(typeof(ArgumentOutOfRangeException),
                            () => proxyCache.GetProxy(obj));
        }

        //[Test]
        //[ExpectedException(typeof(ArgumentOutOfRangeException))]
        //public void ShouldDenyViewModelsForConcreteCollections()
        //{
        //    var obj = new UnsupportedTypes();
        //    var objectVM = My.Instance.Engine.ViewModelCache.GetViewModel(obj);
        //}

        //[Test]
        //public void ShouldPreventViewModelsForConcreteCollections()
        //{
        //    var obj = new UnsupportedTypes();
        //    var objectVM = My.Instance.Engine.ViewModelCache.GetViewModel(obj, false);

        //    Assert.IsNull(objectVM);
        //}

        //[Test]
        //public void ShouldRaisePropertyStyleUpdate()
        //{
        //    var obj = new TextValues();
        //    var objectVM = My.Instance.Engine.ViewModelCache.GetViewModel(obj);
        //    var proxy = objectVM as IFresnelProxy;

        //    var changeTracker = proxy.Meta.ChangeTracker;
        //    var propertyChanges = new List<string>();
        //    changeTracker.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
        //    {
        //        propertyChanges.Add(e.PropertyName);
        //    };

        //    // Styles can be retrieved in 2 ways:
        //    var style = proxy.Meta.Properties["NormalText"].UI.Style;
        //    style = proxy.Meta["NormalText"].UI.Style;
        //    var originalLabel = style.Label.Value;

        //    Assert.IsFalse(changeTracker.HasChanges);
        //    objectVM.NormalText = "Text was changed at " + DateTime.Now.ToString();
        //    Assert.IsTrue(changeTracker.HasChanges);

        //    var updatedLabel = style.Label.Value;

        //    Assert.AreNotEqual(0, propertyChanges.Count);

        //    Assert.AreNotEqual(originalLabel, updatedLabel);
        //}

        //[Test]
        //public void ShouldExposePermissions()
        //{
        //    var poco = new SampleModel.Objects.PocoObject();
        //    var pocoProxy = My.Instance.Engine.ViewModelCache.GetViewModel(poco);
        //    var proxy = pocoProxy as IFresnelProxy;

        //    var permissions = proxy.Meta.Permissions;
        //    Assert.IsNotNull(permissions);

        //    permissions.Read.Check();
        //    Assert.IsNotNull(permissions.Read.Result.Passed);
        //}

        //[Test]
        //public void ShouldExposeUiStyleMembers()
        //{
        //    var poco = new SampleModel.Objects.PocoObject();
        //    var pocoProxy = My.Instance.Engine.ViewModelCache.GetViewModel(poco);
        //    var proxy = pocoProxy as IFresnelProxy;

        //    Assert.IsNotNull(proxy.Meta.UI);
        //    Assert.AreNotSame(string.Empty, proxy.Meta.UI.Style.Label.ToString());
        //}

        //[Test]
        //public void ShouldHonourPermissionsViaINPC()
        //{
        //    var obj = new SecuredObject();
        //    var objVM = My.Instance.Engine.ViewModelCache.GetViewModel(obj);

        //    var propertyChanges = new List<string>();
        //    ((INotifyPropertyChanged)objVM).PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
        //    {
        //        propertyChanges.Add(e.PropertyName);
        //    };

        //    try
        //    {
        //        objVM.BooleanValue = true;
        //        Assert.Fail("Property setter should have failed");
        //    }
        //    catch (Exception ex)
        //    {
        //        var propVM = ((IFresnelProxy)objVM).Meta.Properties["BooleanValue"];
        //        Assert.IsNotNull(propVM.ErrorMessage);
        //    }

        //    try
        //    {
        //        var dateValue = objVM.DateValue;
        //        Assert.Fail("Property getter should have failed");
        //    }
        //    catch (Exception ex)
        //    {
        //        var propVM = ((IFresnelProxy)objVM).Meta.Properties["DateValue"];
        //        Assert.IsNotNull(propVM.ErrorMessage);
        //    }

        //    Assert.AreEqual(0, propertyChanges.Count);
        //}

        //[Test]
        //public void ShouldDetectChangesToNonInterceptableProperties()
        //{
        //    var obj = new NonInterceptablePropertyObjects();
        //    var objVM = My.Instance.Engine.ViewModelCache.GetViewModel(obj);
        //    var proxy = objVM as IFresnelProxy;

        //    Assert.AreSame(obj, proxy.Meta.RealObject);
        //    Assert.AreEqual(obj.DetailObject.ID, objVM.DetailObject.ID, "DetailObject propery MUST be virtual for interception to work");

        //    var detailObjectChangeTracker = proxy.Meta["DetailObject"].InnerObject.ChangeTracker;
        //    detailObjectChangeTracker.ResetDirtyFlags();
        //    Assert.IsFalse(detailObjectChangeTracker.IsDirty);

        //    objVM.DetailObject.Name = "This is a test";

        //    // We MUST use IsDirty to detect property changes:
        //    Assert.IsTrue(proxy.Meta["DetailObject"].InnerObject.IsDirty);

        //    Assert.IsTrue(detailObjectChangeTracker.HasChanges);
        //}

        [Test()]
        public void ShouldIdentifyRealTypeFromProxy()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var proxyCache = container.Resolve<ProxyCache>();

            var poco = new SampleModel.Objects.PocoObject();
            var pocoProxy = proxyCache.GetProxy(poco);

            var typeResolver = container.Resolve<FresnelTypeResolver>();

            // Act:
            var realType = typeResolver.GetRealType(pocoProxy.GetType());

            // Assert:
            Assert.AreEqual(poco.GetType(), realType);
        }

    }

}

