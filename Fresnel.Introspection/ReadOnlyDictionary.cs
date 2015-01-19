using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.Introspection
{
    /// <summary>
    /// A dictionary that can be updated internally, but is read-only to external consumers
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDisposable
    {
        private readonly IDictionary<TKey, TValue> _Items;

        public ReadOnlyDictionary()
        {
            _Items = new Dictionary<TKey, TValue>();
        }

        public ReadOnlyDictionary(IDictionary<TKey, TValue> items)
        {
            _Items = items;
        }

        public ReadOnlyDictionary(IDictionary<TKey, TValue> items, IDictionary<string, TValue> namedItems)
        {
            _Items = items;
        }

        protected void AddItem(TKey key, string name, TValue value)
        {
            _Items.Add(key, value);
        }

        #region IDictionary<TKey,TValue> Members

        public void Add(TKey key, TValue value)
        {
            throw new NotSupportedException("This dictionary is read-only");
        }

        public bool ContainsKey(TKey key)
        {
            return _Items.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return _Items.Keys; }
        }

        public bool Remove(TKey key)
        {
            throw new NotSupportedException("This dictionary is read-only");
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _Items.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get { return _Items.Values; }
        }

        public TValue this[TKey key]
        {
            get
            {
                return _Items[key];
            }
            set
            {
                throw new NotSupportedException("This dictionary is read-only");
            }
        }

        #endregion IDictionary<TKey,TValue> Members

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException("This dictionary is read-only");
        }

        public void Clear()
        {
            throw new NotSupportedException("This dictionary is read-only");
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _Items.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _Items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _Items.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException("This dictionary is read-only");
        }

        #endregion ICollection<KeyValuePair<TKey,TValue>> Members

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _Items.GetEnumerator();
        }

        #endregion IEnumerable<KeyValuePair<TKey,TValue>> Members

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (_Items as System.Collections.IEnumerable).GetEnumerator();
        }

        #endregion IEnumerable Members

        public void Dispose()
        {
            _Items.Clear();
        }
    }
}