using UnityEngine;

public class GunView : MonoBehaviour
{
    [SerializeField] private float _rotationOffset;
    [SerializeField] private SpriteRenderer _weaponSprite;
    [SerializeField] private Transform _handPivot;
    [SerializeField] private GameObject _crosshair;

    private Vector3 _mousePosition;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        var mousePos = GetMouseWorldPosition();
        RotateWeapon(mousePos);
        UpdateSpriteFlip(mousePos);
        UpdateCrosshairPosition();
    }

    private Vector3 GetMouseWorldPosition()
    {
        var pos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }

    private void RotateWeapon(Vector3 targetPosition)
    {
        Vector2 direction = targetPosition - _handPivot.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var targetRot = Quaternion.Euler(0, 0, angle + _rotationOffset);

        transform.rotation = targetRot;
        transform.position = _handPivot.position;
    }

    private void UpdateSpriteFlip(Vector3 mousePosition)
    {
        _weaponSprite.flipY = mousePosition.x < transform.position.x;
    }
    
    private void UpdateCrosshairPosition()
    {
        _mousePosition = Input.mousePosition;
        _mousePosition.z = _mainCamera.nearClipPlane;
        _crosshair.transform.position = _mainCamera.ScreenToWorldPoint(_mousePosition);
    }
}