using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemyt : MonoBehaviour
{
    [SerializeField] private EnemyLogic TutorialEnemy;
    public void ActivateEnemy()
    {
        TutorialEnemy._isPaused = false;
    }
}
