using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _health = 1;
    [SerializeField] private Animator _animator;
    [SerializeField] private DeathEffect deathEffect;

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
        // ��������� ������ � ����������
        var colliders = GetComponents<Collider2D>();
        foreach (var col in colliders) col.enabled = false;

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        // ��������� ������ ������
        if (deathEffect != null)
        {
            deathEffect.PlayDeathEffect();
        }
        
        if (_animator != null)
        {
            _animator.Play("Dead");
        }
        else
        {
            Destroy(gameObject);
        }
    }
}