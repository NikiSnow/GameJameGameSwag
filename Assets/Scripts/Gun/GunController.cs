using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private int _bullet = 5;
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _range = 100f;
    [SerializeField] private LayerMask _enemyLayer;
    
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _lineDuration = 0.1f;

    private int _currentBullet;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        _currentBullet = _bullet;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && _currentBullet != 0)
        {
            Shoot();
        }
    }

    public void AddBullet(int value)
    {
        _currentBullet += value;
    }

    private void Shoot()
    {
         _currentBullet--;
         
        Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var shootDirection = (mousePosition - (Vector2)transform.position).normalized;
        var hit = Physics2D.Raycast(transform.position, shootDirection, _range, _enemyLayer);

        if (hit.collider != null)
        {
            var targetHealth = hit.collider.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(_damage);
            }
        }
        
        StartCoroutine(ShowShotLine(transform.position, hit, shootDirection));
    }
    
    private IEnumerator ShowShotLine(Vector2 start, RaycastHit2D hit, Vector2 shootDirection)
    {
        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, hit.collider != null ? hit.point : start + shootDirection * _range);
    
        yield return new WaitForSeconds(_lineDuration);
    
        _lineRenderer.enabled = false;
    }
}