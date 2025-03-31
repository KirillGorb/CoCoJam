using CodeScripts.InspectorHelpers.Dictionary;
using UnityEngine;

namespace CodeScripts.PlayerResponse.Config
{
    [CreateAssetMenu]
    public class CollisionResponseConfig : ScriptableObject
    {
        [SerializeField] private SerializeDictionary<LayerMask, GameObject> _gam;
    }
}
