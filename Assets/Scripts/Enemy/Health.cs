using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _health = 1;
    [SerializeField] private Animator _animator;

    public void TakeDamage(int damage)
    {
        _health -= damage;
        
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("Dead");
            Destroy(gameObject, _animator.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
