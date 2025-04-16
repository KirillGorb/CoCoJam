using CodeScripts.Abstraction;
using CodeScripts.UI.TooltipSystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using DG.Tweening;

namespace CodeScripts.Timeline.View
{
    public class TooltipCollapseView : MonoBehaviour, ITooltipView
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Vector2 offset;
        [SerializeField] private TMP_Text text;

        [Inject] private readonly TooltipPointer _pointer;

        private Camera _main;

        [field: SerializeField] public int TimeToShowMS { get; private set; }

        private void Start()
        {
            _main = Camera.main;
            gameObject.SetActive(false);
            _pointer.Init(this);
        }

        public void SetData(IData data)
        {
            if (data is MapItemView view && view.InAge(out var col))
            {
                text.text = col.ScenePlay;
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
            var s = gameObject.GetComponent<CanvasGroup>();
            s.alpha = 0;
            s.DOFade(1, 0.25f);
        }

        public void Hide()
        {
            gameObject.GetComponent<CanvasGroup>().DOFade(0, 0.25f)
                .OnComplete(() => gameObject.SetActive(false));
        }

        public void UpdatePosition()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            RectTransform canvasRectTransform = canvas.transform as RectTransform;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, mousePosition, _main,
                out Vector2 localPoint);

            RectTransform tooltipRect = gameObject.GetComponent<RectTransform>();
            tooltipRect.anchoredPosition = localPoint + offset;
        }
    }
}