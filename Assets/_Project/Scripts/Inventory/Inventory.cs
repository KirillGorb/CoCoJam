using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [Serializable]
    public class Inventory
    {
        [SerializeField] private int countCell;
        [SerializeField] private int sizeCell;
        [SerializeField] private DictionaryView<string, int> testDic;

        private Dictionary<string, int> _resources;
        private int _countItem;

        public int SizeCell => sizeCell;
        public int CountCell => countCell;
        public IReadOnlyDictionary<string, int> Resources => _resources;

        public void Init() => _resources = testDic.Create();

        public int AddResource(string cellKey)
        {
            if (_countItem + 1 >= countCell * sizeCell) return sizeCell;

            _countItem++;
            if (!_resources.ContainsKey(cellKey))
                _resources.Add(cellKey, 1);
            else
                _resources[cellKey]++;
            return _resources[cellKey];
        }
    }
}