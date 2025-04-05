using UnityEngine;

public class FirstFall : MonoBehaviour
{
    [SerializeField] private FallRecoverySystem _fallRecoverySystem;

    private void Start()
    {
        if (_fallRecoverySystem == null)
        {
            _fallRecoverySystem = FindObjectOfType<FallRecoverySystem>();
        }

        _fallRecoverySystem.TriggerFall();
    }
}