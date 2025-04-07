using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Pointer : MonoBehaviour
{

    [Header("Main")]
    public float radius = 2f;          // Радиус полукруга
    public float manualAngleStep = 20f; // Шаг при ручном управлении
    public float rotationSpeed = 5f;   // Скорость поворота (для плавности)

    [Header("Auto")]
    public bool autoMove = true;       // Вкл/выкл автоматическое движение
    public bool moveClockwise = true;   // Направление движения
    public float autoMoveSpeed = 10f;   // Скорость автоматического движения (градусов/сек)
    [SerializeField] private FallRecoverySystem _fallRecoverySystem;

    private Vector3 centerPosition;
    [SerializeField] private float currentAngle;
    private float targetAngle;
    private bool isManualControl;

    [Header("Speed Settings")]
    [SerializeField] private float FirSpeed;
    [SerializeField] private float SecSpeed;
    [SerializeField] private float ThiSpeed;
    [SerializeField] private float ForSpeed;
    [SerializeField] private int NowSpeed;
    private bool ReadyToChange;

    [SerializeField] Camera MainCum;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private GameObject SigaPrefab;

    [Header("Player Following")]
    public float followSmoothness = 5f;
    private Transform playerTransform;
    private Vector3 offsetFromPlayer;
    private Vector3 CumeraWasPoint;
    
    private Coroutine twentySecondCoroutine;
    private GameObject SigaPref;

    [SerializeField] private GameObject PuffButton;
    [SerializeField] private wlking PlayerScr;

    [SerializeField] private GameObject BalancePart;
    [SerializeField] GunStateController GunContr;

    public void Puff()
    {
        PuffButton.SetActive(false);
        MainCum.GetComponent<CinemachineBrain>().enabled = false;
        CumeraWasPoint = MainCum.GetComponent<Transform>().position;
        Debug.Log(CumeraWasPoint.x);
        SigaPref = Instantiate(SigaPrefab, SpawnPoint.position , Quaternion.identity);
        Debug.Log(GameObject.FindWithTag("SigaCumeraPoint"));
        Vector3 CumPoint= GameObject.FindWithTag("SigaCumeraPoint").GetComponent<Transform>().position;
        MainCum.GetComponent<Transform>().position = new Vector3(CumPoint.x, CumPoint.y, -10);
        GoSmokePause();
        Debug.Log(CumeraWasPoint.x);
        PlayerScr.AbleMove = false;
    }
    public void GoBackAfterSmoking()
    {
        Debug.Log(CumeraWasPoint.x);
        PuffButton.SetActive(true);
        GunContr.EnableGun();
        MainCum.GetComponent<CinemachineBrain>().enabled = true;
        MainCum.GetComponent<Transform>().position = new Vector3(CumeraWasPoint.x, CumeraWasPoint.y,-10);
        Destroy(SigaPref);
        autoMove = true;
        PlayerScr.AbleMove = true;
        Smoked();
        PlayEnemysAfterSmoking();
    }
    private void Awake()
    {

        // Получаем трансформ игрока (родительский объект)
        playerTransform = transform.parent;

        if (playerTransform == null)
        {
            Debug.LogError("Arrow must be a child of the player object!");
            return;
        }

        // Рассчитываем начальное смещение от игрока
        offsetFromPlayer = transform.position - playerTransform.position;

        // Инициализация позиции
        centerPosition = playerTransform.position + offsetFromPlayer;
        currentAngle = 90f;
        targetAngle = currentAngle;
        UpdatePosition();
        twentySecondCoroutine = StartCoroutine(TwentySecondCoroutine());
    }
    void OnEnable()
    {
        GunContr.EnableGun();
        currentAngle = 90f;
        targetAngle = currentAngle;
        UpdatePosition();
    }

    IEnumerator TwentySecondCoroutine()
    {
        Debug.Log("Корутина началась в: " + Time.time);

        float duration = 20f; // Длительность в секундах
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Здесь можно выполнять действия каждый кадр
            // Например, обновлять UI с оставшимся временем
            //Debug.Log("Прошло времени: " + elapsedTime.ToString("F1") + " секунд");

            yield return null; // Ждем следующий кадр
        }

        Debug.Log("Корутина завершилась через 20 секунд в: " + Time.time);
        EndTimerFunc();

        // Здесь можно выполнить действия по завершению таймера
    }

    public void GoSmokePause()
    {
        StopTwentySecondCoroutine();
        autoMove = false;
    }

    public void EndTimerFunc()
    {
        IncrSpeed();
        twentySecondCoroutine = StartCoroutine(TwentySecondCoroutine());
    }

    public void Smoked()
    {
        StopTwentySecondCoroutine();
        DecrSpeed();
        twentySecondCoroutine = StartCoroutine(TwentySecondCoroutine());
    }
    public void StopTwentySecondCoroutine()
    {
        if (twentySecondCoroutine != null)
        {
            StopCoroutine(twentySecondCoroutine);
            Debug.Log("Корутина остановлена принудительно!");
            twentySecondCoroutine = null;
        }
    }
    void Update()
    {
        if (playerTransform == null) return;

        // Обновляем центр окружности относительно игрока
        centerPosition = playerTransform.position + offsetFromPlayer;

        // Ручное управление (шагами по 20 градусов)
        if (Input.GetKeyDown(KeyCode.E) && moveClockwise)
        {
            targetAngle = Mathf.Clamp(targetAngle - manualAngleStep, 0f, 180f);
            isManualControl = true;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && !moveClockwise)
        {
            targetAngle = Mathf.Clamp(targetAngle + manualAngleStep, 0f, 180f);
            isManualControl = true;
        }

        // Автоматическое плавное движение
        if (autoMove && !isManualControl)
        {
            float direction = moveClockwise ? 1f : -1f;
            targetAngle += autoMoveSpeed * direction * Time.deltaTime;

            // Проверка границ и смена направления
            if (targetAngle >= 180f)
            {
                targetAngle = 180f;
                Debug.Log("u fall to the left");//FALL TO THE LEFT
                PlayerScr.AbleMove = false;
                GunContr.DisableGun();
                PuffButton.SetActive(false);
                _fallRecoverySystem.TriggerFall();
                StopEnemysOnSmoking();
                BalancePart.SetActive(false);
            }
            else if (targetAngle <= 10f)
            {
                targetAngle = 0f;
                Debug.Log("u fall to the right"); //FALL TO THE RIGHT
                PlayerScr.AbleMove = false;
                GunContr.DisableGun();
                PuffButton.SetActive(false);
                _fallRecoverySystem.TriggerFall();
                PlayEnemysAfterSmoking();
                BalancePart.SetActive(false);
            }
        }

        // Плавное перемещение к целевому углу
        if (Mathf.Abs(currentAngle - targetAngle) > 0.1f)
        {
            Debug.Log("Um Moving");
            currentAngle = Mathf.Lerp(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
            UpdatePosition();
        }
        else
        {
            currentAngle = targetAngle;
            isManualControl = false; // Сброс флага ручного управления
        }

        CheckChangeFallingSide();
    }

    void UpdatePosition()
    {
        // Вычисляем новую позицию относительно центра
        float radians = currentAngle * Mathf.Deg2Rad;
        float x = centerPosition.x + radius * Mathf.Cos(radians);
        float y = centerPosition.y + radius * Mathf.Sin(radians);

        // Устанавливаем позицию стрелки
        transform.position = new Vector3(x, y, transform.position.z);

        // Поворачиваем стрелку по касательной к окружности
        transform.rotation = Quaternion.Euler(0, 0, currentAngle - 90);
    }

    public void CheckChangeFallingSide()
    {
        if (targetAngle > 80 && targetAngle < 100)
        {
            if (targetAngle == currentAngle && ReadyToChange)
            {
                bool ShoudGoNext = Random.Range(0f, 1f) <= 0.5f;
                moveClockwise = ShoudGoNext;
                ReadyToChange = false;
            }
        }
        else if (targetAngle < 80) //Right
        {
            ReadyToChange = true;
            moveClockwise = false;
        }
        else if (targetAngle > 100) //Left
        {
            ReadyToChange = true;
            moveClockwise = true;
        }
    }

    public void DecrSpeed()
    {
        autoMoveSpeed = FirSpeed;
        NowSpeed = 0;
    }

    public void IncrSpeed()
    {
        switch (NowSpeed)
        {
            case 0:
                autoMoveSpeed = SecSpeed;
                NowSpeed = 1;
                break;
            case 1:
                autoMoveSpeed = ThiSpeed;
                NowSpeed = 2;
                break;
            case 2:
                autoMoveSpeed = ForSpeed;
                NowSpeed = 3;
                break;
            case 3:
                autoMoveSpeed = ForSpeed;
                NowSpeed = 3;
                break;
        }
    }

    // Методы для внешнего управления
    public void ToggleAutoMove()
    {
        autoMove = !autoMove;
    }

    public void MoveArrow(int direction) // 1 - вправо, -1 - влево
    {
        targetAngle = Mathf.Clamp(targetAngle + manualAngleStep * direction, 0f, 180f);
        isManualControl = true;
    }

    public void SetAutoMoveSpeed(float speed)
    {
        autoMoveSpeed = Mathf.Clamp(speed, 10f, 360f);
    }
    public void StopEnemysOnSmoking()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyLogic>()._isPaused = true;
        }

    }
    public void PlayEnemysAfterSmoking()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyLogic>()._isPaused = false;
        }
    }
}
