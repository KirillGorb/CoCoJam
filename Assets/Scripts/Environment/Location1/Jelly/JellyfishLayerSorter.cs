using UnityEngine;

namespace CodeScripts.Environment.Location1.Jelly
{
    class JellyfishLayerSorter : MonoBehaviour
    {
        [SerializeField] private int _layerMin = -5;

#if UNITY_EDITOR
        private void OnValidate()
        {
            var meduses = GameObject.FindGameObjectsWithTag("Medus");

            int i = _layerMin;
            foreach (var item in meduses)
                item.gameObject.GetComponent<SpriteRenderer>().sortingOrder = i++;
        }
#endif
    }
}