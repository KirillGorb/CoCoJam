using System;
using System.Collections.Generic;
using Inventory;
using UnityEngine;

namespace Shop
{
    [CreateAssetMenu(menuName = "Game/Inventory/ShopContainer", fileName = "ShopContainer", order = 1)]
    public class ShopContainer : ScriptableObject
    {
        [SerializeField] private DictionaryView<string, Product> shop;

        public IReadOnlyDictionary<string, Product> Shop => shop.Owner;

        public void Init() => shop.Create();
    }

    [Serializable]
    public struct Product
    {
        public int cost;
        public string description;
        public string keyResource;
    }
}