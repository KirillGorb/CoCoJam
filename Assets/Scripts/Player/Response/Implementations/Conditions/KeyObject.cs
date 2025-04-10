using CodeScripts.Timeline.Model;
using UnityEngine;
using Zenject;

namespace CodeScripts.PlayerResponse.Implementations.Conditions
{
    public class KeyObject : MonoBehaviour, InteractionCondition
    {
        [SerializeField] private CollapseModel open;

        [Inject] private readonly DataSwitcher _dataSwitcher;

        private void Start()
        {
            var key = new DataKey { IdKey = open.ID, KeyGo = gameObject };
            _dataSwitcher.Container.Add(typeof(KeyObject), key);
        }
    }
}