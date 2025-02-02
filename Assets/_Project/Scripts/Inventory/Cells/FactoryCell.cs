using UnityEngine;

namespace Inventory
{
    public class FactoryCell : MonoBehaviour
    {
        [SerializeField] private ContainerViewResource viewResource;
        [SerializeField] private InteractiveCell cell;

        public void Create(Canvas canvas, CellAction owner, string nameResource)
        {
            var rezCell = Instantiate(cell);
            owner.Resource = rezCell;
            rezCell.Create(canvas, owner).SetView(viewResource.View[nameResource], nameResource);

        }

        public void Create(Canvas canvas, CellAction owner)
        {
            var rezCell = Instantiate(cell);
            rezCell.Create(canvas, owner);
            owner.Resource = rezCell;
        }
    }
}