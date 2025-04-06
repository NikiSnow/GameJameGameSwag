using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class FallRecoverySystem : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float recoveryTime = 3f;
    [SerializeField] private int minPressCount = 3;
    [SerializeField] private int maxPressCount = 8;

    [Header("UI элементы")]
    [SerializeField] private Image eKeyImage;        // Картинка кнопки E
    [SerializeField] private TMP_Text pressCountText;   // Текст счетчика
    [SerializeField] private TMP_Text timerText;        // Текст таймера

    private bool isFallen;
    private int requiredPresses;
    private int currentPresses;
    private float remainingTime;
    private Coroutine recoveryCoroutine;

    [SerializeField] wlking Player;
    [SerializeField] GameObject BalanceVisual;
    [SerializeField] GameObject PuffButton;
    public void TriggerFall()
    {
        if (isFallen) return;

        isFallen = true;
        requiredPresses = Random.Range(minPressCount, maxPressCount + 1);
        currentPresses = 0;
        remainingTime = recoveryTime;

        // Включаем все UI элементы
        SetUIActive(true);
        UpdateUI();

        // Запускаем таймер сразу при падении
        recoveryCoroutine = StartCoroutine(RecoveryCountdown());

        Debug.Log($"Требуется нажатий: {requiredPresses} за {recoveryTime} сек.");
    }

    private void Update()
    {
        if (!isFallen) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentPresses++;
            UpdateUI();
            //Debug.Log($"Нажато: {currentPresses}/{requiredPresses}");

            if (currentPresses >= requiredPresses)
            {
                StandUp();
            }
        }
    }

    private IEnumerator RecoveryCountdown()
    {
        while (remainingTime > 0 && currentPresses < requiredPresses)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerUI(); // Обновляем таймер каждый кадр
            yield return null;
        }

        if (remainingTime <= 0)
        {
            Die();
        }
    }

    private void StandUp()
    {
        isFallen = false;
        SetUIActive(false);
        Player.AbleMove=true;
        PuffButton.SetActive(true);
        BalanceVisual.SetActive(true);
        Debug.Log("Персонаж поднялся!");
    }

    private void Die()
    {
        isFallen = false;
        SetUIActive(false);
        Debug.LogError("ИГРОК УМЕР!");
    }

    private void UpdateUI()
    {
        if (pressCountText != null)
        {
            pressCountText.text = $"E: {currentPresses}/{requiredPresses}";
        }
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            // Округляем до десятых и выводим
            float roundedTime = Mathf.Round(remainingTime * 10) * 0.1f;
            timerText.text = $"{roundedTime:F1}";
        }
    }

    private void SetUIActive(bool state)
    {
        if (eKeyImage != null)
            eKeyImage.gameObject.SetActive(state);

        if (pressCountText != null)
            pressCountText.gameObject.SetActive(state);

        if (timerText != null)
            timerText.gameObject.SetActive(state);
    }
}