using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    [Header("Main")]
    public float radius = 2f;          // ������ ���������
    public float manualAngleStep = 20f; // ��� ��� ������ ����������
    public float rotationSpeed = 5f;   // �������� �������� (��� ���������)

    [Header("Auto")]
    public bool autoMove = true;       // ���/���� �������������� ��������
    public bool moveClockwise = true;   // ����������� ��������
    public float autoMoveSpeed = 10f;   // �������� ��������������� �������� (��������/���)

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
    void Start()
    {
        // ������������� �������
        centerPosition = transform.position;
        currentAngle = 90f;
        targetAngle = currentAngle;
        UpdatePosition();
    }

    public void CheckChangeFallingSide()
    {
        if (targetAngle >80 && targetAngle < 100 )
        {
            if(targetAngle == currentAngle && ReadyToChange)
            {
                bool ShoudGoNext = Random.Range(0f, 1f) <= 0.5f;
                moveClockwise = ShoudGoNext;
                ReadyToChange=false;
            }
        }
        else if (targetAngle <80) //Right
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
        switch (NowSpeed)
        {
            case 0:
                autoMoveSpeed = FirSpeed; //Was 1 now 1
                NowSpeed = 0;
                break;
            case 1:
                autoMoveSpeed = FirSpeed; //Was 2 now 1
                NowSpeed = 0;
                break;
            case 2:
                autoMoveSpeed = SecSpeed; //Was 3 now 2
                NowSpeed = 1;
                break;
            case 3:
                autoMoveSpeed = ThiSpeed; //Was 4 now 3
                NowSpeed = 2;
                break;
        }
    }

    public void IncrSpeed()
    {
        switch (NowSpeed)
        {
            case 0:
                autoMoveSpeed = SecSpeed; //Was 1 now 2
                NowSpeed = 1;
                break;
            case 1:
                autoMoveSpeed = ThiSpeed; //Was 2 now 3
                NowSpeed = 2;
                break;
            case 2:
                autoMoveSpeed = ForSpeed; //Was 3 now 4
                NowSpeed = 3;
                break;
            case 3:
                autoMoveSpeed = ForSpeed; //Was 4 now 4
                NowSpeed = 3;
                break;
        }
    }

    void Update()
    {
        // ������ ���������� (������ �� 20 ��������)
        if (Input.GetKeyDown(KeyCode.E) )
        {
            targetAngle = Mathf.Clamp(targetAngle - manualAngleStep, 0f, 180f);
            isManualControl = true;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            targetAngle = Mathf.Clamp(targetAngle + manualAngleStep, 0f, 180f);
            isManualControl = true;
        }

        // �������������� ������� ��������
        if (autoMove && !isManualControl)
        {
            float direction = moveClockwise ? 1f : -1f;
            targetAngle += autoMoveSpeed * direction * Time.deltaTime;

            // �������� ������ � ����� �����������
            if (targetAngle >= 180f)
            {
                targetAngle = 180f;
                Debug.Log("u fall to the left");

            }
            else if (targetAngle <= 10f)
            {
                targetAngle = 0f;
                Debug.Log("u fall to the right");
            }
        }

        // ������� ����������� � �������� ����
        if (Mathf.Abs(currentAngle - targetAngle) > 0.1f)
        {
            currentAngle = Mathf.Lerp(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
            UpdatePosition();
        }
        else
        {
            currentAngle = targetAngle;
            isManualControl = false; // ����� ����� ������� ����������
        }
        CheckChangeFallingSide();
    }

    void UpdatePosition()
    {
        // ��������� ����� �������
        float radians = currentAngle * Mathf.Deg2Rad;
        float x = centerPosition.x + radius * Mathf.Cos(radians);
        float y = centerPosition.y + radius * Mathf.Sin(radians);

        // ������������� ������� �������
        transform.position = new Vector3(x, y, transform.position.z);

        // ������������ ������� �� ����������� � ����������
        transform.rotation = Quaternion.Euler(0, 0, currentAngle - 90);
    }

    // ������ ��� �������� ����������
    public void ToggleAutoMove()
    {
        autoMove = !autoMove;
    }

    public void MoveArrow(int direction) // 1 - ������, -1 - �����
    {
        targetAngle = Mathf.Clamp(targetAngle + manualAngleStep * direction, 0f, 180f);
        isManualControl = true;
    }

    public void SetAutoMoveSpeed(float speed)
    {
        autoMoveSpeed = Mathf.Clamp(speed, 10f, 360f);
    }
}
