using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private bool _isRight;
    [SerializeField] private Animator _animator;
    
    private Rigidbody2D _rigidbody2D;
    private Vector2 _direction;
    private Transform _target;
    public bool _isPaused;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (_isPaused)
        {
            Stay();
            return;
        }
        
        if (_target == null)
        {
            Stay();
            return;
        }
        
        _direction = _target.position - transform.position;
        _direction.Normalize();
        LookToTarget(_direction);
    }

    private void FixedUpdate()
    {
        if (_isPaused)
            return;
        
        _rigidbody2D.velocity = _rigidbody2D.gravityScale == 0 ? new Vector2(_speed * _direction.x, _speed * _direction.y) : new Vector2(_speed * _direction.x, _rigidbody2D.velocity.y);
    }
    
    private void LookToTarget(Vector2 direction)
    {
        var y = direction.x > 0 ? 0f : 180f;
        transform.rotation = new Quaternion(transform.rotation.x, y, transform.rotation.z, transform.rotation.w);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Stay();
            
            if (_animator != null)
                _animator.SetTrigger("Attack");
            
            //other.GetComponent<Player>().Dead();
        }
    }

    private void Stay()
    {
        _rigidbody2D.velocity = Vector2.zero;
        _direction = Vector2.zero;
    }

    public void Pause()
    {
        _isPaused = true;
    }

    public void Unpause()
    {
        _isPaused = false;
    }
}
