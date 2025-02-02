using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Shop
{
    public class ShopViewItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text textDescription;
        [SerializeField] private TMP_Text textCost;
        [SerializeField] private Button buttonShop;


        public void Subscribe(UnityAction action) => buttonShop.onClick.AddListener(action);

        public ShopViewItem SetView(string description, int cost)
        {
            textDescription.text = description;
            textCost.text = cost.ToString();
            return this;
        }
    }
}