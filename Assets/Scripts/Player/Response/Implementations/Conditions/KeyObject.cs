using System;
using CodeScripts.PlayerResponse.Player.Response.Implementations.Conditions;
using CodeScripts.Timeline.Model;
using UnityEngine;
using Zenject;

namespace CodeScripts.PlayerResponse.Implementations.Conditions
{
    [Serializable]
    public class KeyObject : MonoBehaviour, InteractionCondition
    {
        [SerializeField] private CollapseModel open;

        [Inject] private ServiceInteraction _s;

        public int Key => open.ID;

        public void Callback() => _s.key = new DataKey { IdKey = open.ID, KeyGo = gameObject };
    }
}