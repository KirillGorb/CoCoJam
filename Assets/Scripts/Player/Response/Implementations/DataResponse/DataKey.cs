using System;
using CodeScripts.Abstraction;
using UnityEngine;

namespace CodeScripts.PlayerResponse.Implementations
{
    [Serializable]
    public struct DataKey : IData
    {
        public int IdKey;
        public GameObject KeyGo;
    }
}