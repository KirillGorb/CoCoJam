using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class InteractiveCell : MonoBehaviour
    {
        [SerializeField] private Image img;

        public string Name { get; private set; }
        private Canvas _plane;
        public CellAction OwnerCell { get; set; }

        public InteractiveCell Create(Canvas plane, CellAction owner)
        {
            Name = "";
            _plane = plane;
            OwnerCell = owner;
            transform.position = OwnerCell.transform.position;
            return this;
        }

        public void SetView(Sprite sprite, string key)
        {
            img.sprite = sprite;
            Name = key;
            OwnerCell.SetActiveCell();
        }

        public void DragHandler(BaseEventData data)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)_plane.transform,
                    ((PointerEventData)data).position, _plane.worldCamera, out Vector2 pos))
            {
                img.raycastTarget = false;
                transform.position = _plane.transform.TransformPoint(pos);
                OwnerCell.transform.SetAsLastSibling();
            }
        }

        public void EndDrag(BaseEventData data)
        {
            var releasedObject = ((PointerEventData)data).pointerEnter;
            if (releasedObject != null && releasedObject.TryGetComponent(out CellAction cell))
                (OwnerCell.Resource, cell.Resource) = (cell.Resource, OwnerCell.Resource);
            else if (releasedObject != null && releasedObject.TryGetComponent(out ViewCellCountWrite cellView))
                (OwnerCell.Resource, cellView.OwnerCell.Resource) = (cellView.OwnerCell.Resource, OwnerCell.Resource);
            else
                transform.position = OwnerCell.transform.position;

            img.raycastTarget = true;
        }
    }
}