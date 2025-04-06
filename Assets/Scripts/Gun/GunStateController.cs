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

    // ������ ���������� ����������� ������
    public void DisableGun()
    {
        // ��������� ������� ���������
        _wasShootingEnabled = _gunController.enabled;
        _wasCrosshairEnabled = _crosshair.activeSelf;

        // ��������� ������
        _gunController.enabled = false;

        // ��������� ������
        if (_lineRenderer != null) _lineRenderer.enabled = false;
        if (_crosshair != null) _crosshair.SetActive(false);
        _gunView.enabled = false;
    }

    // ��������� ������� � ��������������� ���������
    public void EnableGun()
    {
        // ��������������� ������
        _gunController.enabled = _wasShootingEnabled;

        // ��������������� ������
        if (_crosshair != null) _crosshair.SetActive(_wasCrosshairEnabled);
        _gunView.enabled = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DisableGun();
        Debug.Log("���� ��� �������!");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EnableGun();
        Debug.Log("���� ���� � ������!");
    }
}
