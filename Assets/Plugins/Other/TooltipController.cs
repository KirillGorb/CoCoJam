using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.InputSystem;

public class TooltipController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject tooltip;
    [SerializeField] private Vector2 offset;
    [SerializeField] private int timeToShowMS;

    private CancellationTokenSource _cancellationTokenSource;
    private Camera _main;

    private void Start()
    {
        _main = Camera.main;
        // Скрываем тултип в начале
        tooltip.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        ShowTooltipWithDelay(timeToShowMS, _cancellationTokenSource.Token).Forget();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Скрываем тултип и отменяем задержку
        _cancellationTokenSource?.Cancel();
        HideTooltip();
    }

    private async UniTaskVoid ShowTooltipWithDelay(int delayMilliseconds, CancellationToken cancellationToken)
    {
        await UniTask.Delay(delayMilliseconds, cancellationToken: cancellationToken);

        if (!cancellationToken.IsCancellationRequested)
        {
            // Показываем тултип после задержки с анимацией
            tooltip.SetActive(true);
            var s = tooltip.GetComponent<CanvasGroup>();
            s.alpha = 0; // Устанавливаем прозрачность в 0
            s.DOFade(1, 0.25f); // Плавное появление
        }
    }

    private void HideTooltip()
    {
        // Анимация исчезновения
        tooltip.GetComponent<CanvasGroup>().DOFade(0, 0.25f)
            .OnComplete(() => tooltip.SetActive(false)); // Плавное исчезновение
    }

    private void Update()
    {
        // Обновляем позицию тултипа в соответствии с позицией мыши
        if (tooltip.activeSelf)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue(); // Получаем позицию мыши из новой системы ввода

            // Преобразуем позицию мыши в локальные координаты канваса
            RectTransform canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, mousePosition, _main, out localPoint);

            // Проверяем, не выходит ли тултип за границы экрана
            RectTransform tooltipRect = tooltip.GetComponent<RectTransform>();
            tooltipRect.anchoredPosition = localPoint  + offset;
        }
    }
}