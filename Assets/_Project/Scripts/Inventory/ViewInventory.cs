using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Inventory
{
    public class ViewInventory : MonoBehaviour
    {
        [SerializeField] private Transform parent;
        [SerializeField] private Canvas plane;
        [SerializeField] private Inventory inventory;
        [SerializeField] private CellAction initCell;
        [SerializeField] private ViewCellCountWrite noneCellCountWrite;
        [SerializeField] private ContainerViewResource containerViewView;

        [SerializeField] private int maxX;
        [SerializeField] private Vector2 offsetStep;
        [SerializeField] private Vector3 offset;

        private CellAction[] _cells;

        private void Awake()
        {
            inventory.Init();
            containerViewView.Init();
        }

        private void Start()
        {
            CreateInventory();
            View();
        }

        private void CreateInventory()
        {
            _cells = new CellAction[inventory.CountCell];

            for (int i = 0, x = 0, y = 0; i < inventory.CountCell; i++)
            {
                _cells[i] = Instantiate(initCell, new Vector3(x * offsetStep.x, y * offsetStep.y) + offset,
                    Quaternion.identity);
                _cells[i].transform.parent = parent;
                _cells[i].Resource = (ViewCellCountWrite)Instantiate(noneCellCountWrite, _cells[i].transform).Create(plane, _cells[i]);

                x++;
                if (x >= maxX)
                {
                    x = 0;
                    y++;
                }
            }
        }

        private void View()
        {
            int iss = 0;
            foreach (var item in inventory.Resources)
            {
                var view = containerViewView.View[item.Key];
                if (item.Value <= inventory.SizeCell)
                {
                    ActiveCell(_cells[iss], view, item.Key, item.Value);
                    iss++;
                }
                else
                {
                    int size = item.Value;
                    int s = inventory.SizeCell;
                    while (size > s)
                    {
                        ActiveCell(_cells[iss], view, item.Key, item.Value);
                        size -= s;
                        iss++;
                    }

                    ActiveCell(_cells[iss], view, item.Key, size);
                    iss++;
                }
            }
        }

        public void AddResource(string keyResource)
        {
            var view = containerViewView.View[keyResource];
            Action<CellAction> set = e =>
            {
                ActiveCell(e, view, keyResource, inventory.AddResource(e.Resource.Name));
            };

            bool flag = true;
            CellAction cellNull = null;

            foreach (var cell in _cells)
            {
                if (cellNull == null && cell.Resource.Name == "")
                    cellNull = cell;

                if (cell.Resource.Name == keyResource)
                {
                    set(cell);
                    flag = false;
                    break;
                }
            }

            if (flag && cellNull != null)
                set(cellNull);
        }

        private void ActiveCell(CellAction cell, Sprite view, string key, int count)
        {
            if (count == 0) return;
            cell.Resource.SetView(view, key);
            ((ViewCellCountWrite)cell.Resource)?.SetCount(count);
        }
    }
}