using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _range = 100f;
    [SerializeField] private LayerMask _enemyLayer;
    
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _lineDuration = 0.1f;
    [SerializeField] private ItemPickupSystem lootSystem;
    [Header("Highlight")]
    [SerializeField] private GameObject _firePrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _highlightTime = 0.5f;
    [SerializeField] private AudioSource ASos;
    [SerializeField] private AudioClip ShootSound;

    public int _currentBullet;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        
    }

    public void Update()
    {
        _currentBullet = lootSystem.GetAmmoCount();

        if (Input.GetButtonDown("Fire1") && _currentBullet != 0)
        {
            Shoot();
            ASos.PlayOneShot(ShootSound);
        }
    }

    public void AddBullet(int value)
    {
        _currentBullet += value;
    }

    private void Shoot()
    {
        var highlight = Instantiate(_firePrefab, _firePoint);
        Destroy(highlight, _highlightTime);

        lootSystem.DecAmmoCount();

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