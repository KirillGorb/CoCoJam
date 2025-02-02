using Inventory;
using UnityEngine;

namespace Shop
{
    public class GenerateShop : MonoBehaviour
    {
        [SerializeField] private ShopContainer shop;
        [SerializeField] private Transform parent;
        [SerializeField] private ShopViewItem prefabItem;
        [SerializeField] private ViewInventory inventory;

        private void Start()
        {
            shop.Init();
            View();
        }

        private void View()
        {
            foreach (var item in shop.Shop)
                Instantiate(prefabItem, parent).SetView(item.Value.description, item.Value.cost)
                    .Subscribe(() => inventory.AddResource(item.Value.keyResource));
        }
    }
}