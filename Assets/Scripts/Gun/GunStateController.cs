using UnityEngine;
using UnityEngine.EventSystems;

public class GunStateController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private GunController _gunController;
    [SerializeField] private GunView _gunView;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private GameObject _crosshair;

    private bool _wasShootingEnabled = true;
    private bool _wasCrosshairEnabled = true;

    // Полное отключение функционала оружия
    public void DisableGun()
    {
        // Сохраняем текущие состояния
        _wasShootingEnabled = _gunController.enabled;
        _wasCrosshairEnabled = _crosshair.activeSelf;

        // Отключаем логику
        _gunController.enabled = false;

        // Отключаем визуал
        if (_lineRenderer != null) _lineRenderer.enabled = false;
        if (_crosshair != null) _crosshair.SetActive(false);
        _gunView.enabled = false;
    }

    // Включение обратно с восстановлением состояния
    public void EnableGun()
    {
        // Восстанавливаем логику
        _gunController.enabled = _wasShootingEnabled;

        // Восстанавливаем визуал
        if (_crosshair != null) _crosshair.SetActive(_wasCrosshairEnabled);
        _gunView.enabled = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DisableGun();
        Debug.Log("Мышь НАД кнопкой!");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EnableGun();
        Debug.Log("Мышь УШЛА с кнопки!");
    }
}
