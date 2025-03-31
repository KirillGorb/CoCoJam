using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeScripts.PlayerMove.Config
{
    [Serializable]
    public class ModificationValue
    {
        [SerializeField] private float _init;
        private List<float> _mods = new();

        private float? _value;

        public float GetValue
        {
            get
            {
                _value ??= _init;
                return _value.Value;
            }
        }

        public ModificationValue(float init)
        {
            _value = _init = init;
        }

        public void AddMod(float mod)
        {
            _mods.Add(mod);

            float value = 0;
            foreach (var item in _mods)
                value += item;
            _value = value;
        }

        public void RemoveMod(float mod)
        {
            _mods.Remove(mod);

            float value = 0;
            foreach (var item in _mods)
                value += item;
            _value = value;
            if (_mods.Count <= 0)
                _value = _init;
        }

        public void ClearMod()
        {
            _value = _init;
            _mods.Clear();
        }
    }
}