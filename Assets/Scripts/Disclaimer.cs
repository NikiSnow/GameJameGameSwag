using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Disclaimer : MonoBehaviour
{
    public TMP_Text textToFade; // Ссылка на текстовый компонент
    public TMP_Text textToFade2;
    public float fadeDuration = 1.0f; // Длительность эффекта fade
    public string nextSceneName; // Имя следующей сцены для загрузки

    private void Start()
    {
        // Начинаем корутину при старте
        StartCoroutine(FadeText());
    }

    private IEnumerator FadeText()
    {
        // Fade In (появление текста)
        yield return Fade(0f, 1f, fadeDuration);

        // Ждём 3 секунды
        yield return new WaitForSeconds(3f);

        // Fade Out (исчезновение текста)
        yield return Fade(1f, 0f, fadeDuration);

        // Загружаем следующую сцену
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float time = 0f;
        Color color = textToFade.color;
        Color color2 = textToFade2.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            textToFade.color = new Color(color.r, color.g, color.b, alpha);
            textToFade2.color = new Color(color2.r, color2.g, color2.b, alpha);
            yield return null;
        }

        // Убедимся, что конечное значение установлено точно
        textToFade.color = new Color(color.r, color.g, color.b, endAlpha);
        textToFade2.color = new Color(color2.r, color2.g, color2.b, endAlpha);
    }

}
