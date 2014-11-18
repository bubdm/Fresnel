using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;
using Envivo.Fresnel.DomainTypes.Interfaces;

namespace Envivo.Fresnel.Introspection
{

    /// <summary>
    /// Used to collect/aggregate a set of Assertions
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public class AssertionSet : IEnumerable<IAssertion>, IAssertion
    {
        private List<IAssertion> _Assertions = new List<IAssertion>();

        public void AddRange(AssertionSet assertionSet)
        {
            _Assertions.AddRange(assertionSet.Items);
        }

        public void AddRange(IEnumerable<IAssertion> assertions)
        {
            _Assertions.AddRange(assertions);
        }

        public ReadOnlyCollection<IAssertion> Items
        {
            get { return new ReadOnlyCollection<IAssertion>(_Assertions); }
        }

        public void Add(IAssertion assertion)
        {
            _Assertions.Add(assertion);
        }

        public void Add(AssertionSet assertionSet)
        {
            _Assertions.AddRange(assertionSet._Assertions);
        }

        public void AddWarning(string warningReason)
        {
            this.Add(Assertion.PassWithWarning(warningReason));
        }

        public void AddWarning(string format, params object[] args)
        {
            this.Add(Assertion.PassWithWarning(string.Format(format, args)));
        }

        public void AddFailure(string failureReason)
        {
            this.Add(Assertion.Fail(failureReason));
        }

        public void AddFailure(string format, params object[] args)
        {
            this.Add(Assertion.Fail(string.Format(format, args)));
        }

        public void Clear()
        {
            _Assertions.Clear();
        }

        public bool Passed
        {
            get { return _Assertions.Any(a => a.Passed); }
        }

        public bool Failed
        {
            get { return _Assertions.Any(a => a.Failed); }
        }

        public bool HasWarning
        {
            get { return _Assertions.Any(a => a.HasWarning); }
        }

        public string FailureReason
        {
            get { return this.GetFailureReason(); }
        }

        public Exception FailureException
        {
            get { return this.GetFailureException(); }
        }

        public string WarningReason
        {
            get { return this.GetWarningReason(); }
        }

        private string GetFailureReason()
        {
            var failures = _Assertions.Where(p => p.Failed);
            return string.Join(Environment.NewLine, failures.Select(f => f.FailureReason));
        }

        private Exception GetFailureException()
        {
            var exceptions = _Assertions
                                .Where(F => F.FailureException != null)
                                .Select(f => f.FailureException)
                                .ToArray();

            switch (exceptions.Length)
            {
                case 0:
                    return null;
                case 1:
                    return exceptions[0];
                default:
                    var messages = string.Join(Environment.NewLine, exceptions.Select(e=> e.Message));
                    var ex = new FresnelException(messages);
                    return ex;
            }
        }

        private string GetWarningReason()
        {
            var warnings = _Assertions.Where(p => p.HasWarning);
            return string.Join(Environment.NewLine, warnings.Select(w => w.WarningReason));
        }

        public void Throw<T>()
            where T : Exception
        {
            Exception ex = null;

            if (this.FailureException != null)
            {
                ex = (T)Activator.CreateInstance(typeof(T), this.FailureReason, this.FailureException);
            }
            else if (!string.IsNullOrEmpty(this.FailureReason))
            {
                ex = (T)Activator.CreateInstance(typeof(T), this.FailureReason);
            }

            if (ex != null)
            {
                throw ex;
            }
        }

        public IEnumerable<IAssertion> ToEnumerable()
        {
            var results = new List<IAssertion>();
            this.FlattenRecursive(this, results);
            return results;
        }

        private void FlattenRecursive(AssertionSet assertions, List<IAssertion> flatList)
        {
            if (assertions == null)
            {
                return;
            }

            foreach (var assertion in assertions.Items)
            {
                if (assertion is AssertionSet)
                {
                    this.FlattenRecursive(assertion as AssertionSet, flatList);
                }
                else
                {
                    flatList.Add(assertion);
                }
            }

        }

        public IEnumerator<IAssertion> GetEnumerator()
        {
            return _Assertions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Assertions.GetEnumerator();
        }
    }

}