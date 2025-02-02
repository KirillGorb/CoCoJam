using UnityEngine;

namespace Inventory
{
    public class CraftPlane : MonoBehaviour
    {
        [SerializeField] private CraftCell[] cells;
        [SerializeField] private CellAction cellRez;
        [SerializeField] private Canvas rezCanvase;
        [SerializeField] private FactoryCell factory;
        [SerializeField] private DictionaryView<string, string> rez;

        private string[] _ids = new string[9];

        private void Start()
        {
            rez.Create();
            
            int i = 0;
            foreach (var item in cells)
            {
                item.SetAction(i++, CheckCraft);
                factory.Create(rezCanvase, item);
            }

            factory.Create(rezCanvase, cellRez);
        }

        private void CheckCraft(int id, string str)
        {
            _ids[id] = str;

            foreach (var i in _ids)
                if (i is null)
                    return;

            string fullstr = string.Join("", _ids);
            if (rez.Owner.ContainsKey(fullstr))
                factory.Create(rezCanvase, cellRez, rez[fullstr]);
        }
    }
}