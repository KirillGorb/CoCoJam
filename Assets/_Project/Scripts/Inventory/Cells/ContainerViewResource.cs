using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Game/Inventory/ContainerViewResource", fileName = "ContainerViewResource", order = 1)]
    public class ContainerViewResource : ScriptableObject
    {
        [SerializeField] private DictionaryView<string, Sprite> view;

        public IReadOnlyDictionary<string, Sprite> View => view.Owner;

        public void Init() => view.Create();
    }
}