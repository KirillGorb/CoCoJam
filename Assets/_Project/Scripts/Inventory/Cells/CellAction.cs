using UnityEngine;

namespace Inventory
{
    public class CellAction : MonoBehaviour
    {
        private InteractiveCell _resource;

        public InteractiveCell Resource
        {
            get => _resource;
            set => SetResource(value);
        }

        public void SetActiveCell() => _resource.gameObject.SetActive(_resource.Name != "");

        protected virtual void SetResource(InteractiveCell value)
        {
            _resource = value;
            var tr = _resource.transform;
            var trT = transform;
            tr.parent = trT;
            tr.position = trT.position;
            _resource.OwnerCell = this;
            SetActiveCell();
        }
    }
}