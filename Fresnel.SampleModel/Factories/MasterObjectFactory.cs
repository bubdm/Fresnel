using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;using Envivo.DomainTypes;
using Envivo.TrueView.Domain.Attributes;
using System.Diagnostics;
using Envivo.Sample.Model.Objects;

namespace Envivo.Sample.Model.Factories
{
    public class MasterObjectFactory : IFactory<MasterObject>
    {

        public MasterObject Create()
        {
            return new MasterObject();
        }

        public MasterObject CreatePreLoadedMaster()
        {
            var result = new MasterObject();
            result.Name = "A pre-loaded Master Object";

            //-----

            var child1 = new DetailObject(result);
            child1.Name = "Child1";
            for (var i = 1; i <= 2; i++)
            {
                var newDetail = new DetailObject();
                newDetail.Name = "Detail " + i.ToString();
                child1.MoreChildren.Add(newDetail);
            }
            result.Children.Add(child1);

            //-----

            var child2 = new DetailObject(result);
            child2.Name = "Child2";
            for (var i = 1; i <= 4; i++)
            {
                var newDetail = new DetailObject();
                newDetail.Name = "Detail " + i.ToString();
                child2.MoreChildren.Add(newDetail);
            }
            result.Children.Add(child2);

            //-----

            var child3 = new DetailObject(result);
            child3.Name = "Child3";
            for (var i = 1; i <= 6; i++)
            {
                var newDetail = new DetailObject();
                newDetail.Name = "Detail " + i.ToString();
                child3.MoreChildren.Add(newDetail);
            }
            result.Children.Add(child3);

            return result;
        }

    }
}
