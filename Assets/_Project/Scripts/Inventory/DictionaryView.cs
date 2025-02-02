using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [Serializable]
    public class DictionaryView<T, T1>
    {
        [SerializeField] private List<View<T, T1>> viewDictionary;

        public Dictionary<T, T1> Owner { get; set; }

        public T1 this[T id] => Owner[id];

        public Dictionary<T, T1> Create()
        {
            Owner = new();
            foreach (var item in viewDictionary)
            {
                if (Owner.ContainsKey(item.key)) continue;
                Owner.Add(item.key, item.value);
            }

            return Owner;
        }
    }

    [Serializable]
    public struct View<T, T1>
    {
        public T key;
        public T1 value;
    }
}