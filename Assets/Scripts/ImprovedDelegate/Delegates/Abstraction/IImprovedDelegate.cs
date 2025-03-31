using CodeScripts.Algorithms.Sort;
using CodeScripts.Algorithms.Sort.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeScripts.ImprovedDelegate.Abstraction
{
    public abstract class IImprovedDelegate<T> where T : Delegate
    {
        protected Dictionary<int, List<T>> _subscribers = new();
        private ISortAlgorithms _sort = new TimSort();

        protected List<int> _sortedKeys = new();

        public void AddListener(T action, int priority = 0)
        {
            if (!_subscribers.ContainsKey(priority))
            {
                _subscribers.Add(priority, new());
                _sortedKeys = _sort.SortList(_subscribers.Keys.ToList());
            }

            _subscribers[priority].Add(action);
        }
        public void RemoveListeners(T action)
        {
            foreach (var key in _subscribers.Keys)
                RemoveListener(action, key);
        }
        public void RemoveListener(T action, int priority)
        {
            if (_subscribers[priority].Contains(action))
            {
                _subscribers[priority].Remove(action);

                if (_subscribers[priority].Count <= 0)
                    _subscribers.Remove(priority);
                else
                    _sortedKeys = _sort.SortList(_subscribers.Keys.ToList());
                return;
            }
            throw new InvalidOperationException();
        }
    }
}
