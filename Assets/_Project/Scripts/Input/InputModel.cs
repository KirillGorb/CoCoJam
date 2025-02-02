using System;
using UnityEngine;

namespace _Project.Scripts.Player
{
    [Serializable]
    public class InputModel
    {
        [Header("Настройки клавиатуры")] 
        
        [Tooltip("Клавиша для прыжка")] public KeyCode jump = KeyCode.Space;
        [Tooltip("Клавиша для спринта")] public KeyCode sprint = KeyCode.LeftShift;
        [Tooltip("Клавиша для приседания")] public KeyCode crouch = KeyCode.Z;
        [Tooltip("Клавиша для переключения курсора")] public KeyCode lockToggle = KeyCode.Q;
        
        [Tooltip("Чувствительность мыши, по оси x и y")] public Vector2 sensitivity = new(2f, 2f);
        [Tooltip("Сглаживание движения мыши (попробуйте с ним и без него)")] public Vector2 smoothing = new(1.5f, 1.5f);
        
    }
}