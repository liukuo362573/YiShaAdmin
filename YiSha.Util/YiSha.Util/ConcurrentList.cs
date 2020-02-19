using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiSha.Util
{
    public class ConcurrentList<T> : IList<T>
    {
        protected static object _lock = new object();
        protected List<T> _interalList = new List<T>();

        public IEnumerator<T> GetEnumerator()
        {
            return Clone().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Clone().GetEnumerator();
        }

        public int Count { get { return _interalList.Count; } }

        public bool IsReadOnly { get { return false; } }

        public T this[int index]
        {
            get
            {
                lock (_lock)
                {
                    return _interalList[index];
                }
            }
            set
            {
                lock (_lock)
                {
                    _interalList[index] = value;
                }
            }
        }

        public List<T> Clone()
        {
            List<T> newList = new List<T>();
            lock (_lock)
            {
                _interalList.ForEach(x => newList.Add(x));
            }
            return newList;
        }

        public int IndexOf(T item)
        {
            return _interalList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _interalList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            lock (_lock)
            {
                _interalList.RemoveAt(index);
            }
        }

        public void Add(T item)
        {
            lock (_lock)
            {
                _interalList.Add(item);
            }
        }

        public void AddRange(IEnumerable<T> list)
        {
            foreach (T item in list)
            {
                Add(item);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _interalList.Clear();
            }
        }

        public bool Contains(T item)
        {
            return _interalList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _interalList.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            if (item == null)
            {
                return false;
            }
            lock (_lock)
            {
                return _interalList.Remove(item);
            }
        }

        public void RemoveAll(Predicate<T> match)
        {
            if (match == null)
            {
                return;
            }
            Contract.Ensures(Contract.Result<int>() >= 0);
            Contract.Ensures(Contract.Result<int>() <= Contract.OldValue(Count));
            Contract.EndContractBlock();

            foreach (T t in Clone())
            {
                if (match(t))
                {
                    Remove(t);
                }
            }
        }
    }
}
