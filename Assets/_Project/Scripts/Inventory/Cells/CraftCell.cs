using System;

namespace Inventory
{
    public sealed class CraftCell : CellAction
    {
        private int _id;
        private Action<int, string> _changePuckDown;

        public void SetAction(int id, Action<int, string> change)
        {
            _id = id;
            _changePuckDown = change;
        }

        protected override void SetResource(InteractiveCell value)
        {
            base.SetResource(value);
            _changePuckDown?.Invoke(_id, value.Name);
        }
    }
}