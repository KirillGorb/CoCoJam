using Sirenix.OdinInspector;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class SelectTypes<T>
{
    [ShowInInspector, HideLabel] public T Instance;

    [SerializeField, ValueDropdown(nameof(GetDerivedTypes)), OnValueChanged(nameof(OnTypeSelected))]
    public string selectedTypeName;

    private string[] GetDerivedTypes()
    {
        var baseType = typeof(T);
        var assembly = baseType.Assembly;
        return assembly.GetTypes()
            .Where(t => baseType.IsAssignableFrom(t) && !t.IsAbstract)
            .OrderBy(t => t.Name)
            .Select(t => t.FullName).ToArray();
    }

    private void OnTypeSelected(string typeName)
    {
        if (string.IsNullOrEmpty(typeName))
        {
            Instance = default;
            return;
        }

        Type type = Type.GetType(typeName);
        if (type != null)
            Instance = (T)Activator.CreateInstance(type);
    }
}