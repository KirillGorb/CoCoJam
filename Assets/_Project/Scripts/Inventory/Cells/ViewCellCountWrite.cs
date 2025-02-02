using TMPro;
using UnityEngine;

namespace Inventory
{
    public class ViewCellCountWrite : InteractiveCell
    {
        [SerializeField] private TMP_Text text;

        public void SetCount(int count)
        {
            text.text = count.ToString();
        }
    }
}