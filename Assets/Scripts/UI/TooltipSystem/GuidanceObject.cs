using CodeScripts.Abstraction;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace CodeScripts.UI.TooltipSystem
{
    public abstract class GuidanceObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Inject] private readonly TooltipPointer _service;

        public abstract IData Data { get; }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
                _service.Data.Execute(Data);
                _service.Enter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
                _service.Data.Execute(Data);
                _service.Exit();
        }
    }
}