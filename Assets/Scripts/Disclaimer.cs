using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Disclaimer : MonoBehaviour
{
    public TMP_Text textToFade; // ������ �� ��������� ���������
    public TMP_Text textToFade2;
    public float fadeDuration = 1.0f; // ������������ ������� fade
    public string nextSceneName; // ��� ��������� ����� ��� ��������

    private void Start()
    {
        // �������� �������� ��� ������
        StartCoroutine(FadeText());
    }

    private IEnumerator FadeText()
    {
        // Fade In (��������� ������)
        yield return Fade(0f, 1f, fadeDuration);

        // ��� 3 �������
        yield return new WaitForSeconds(3f);

        // Fade Out (������������ ������)
        yield return Fade(1f, 0f, fadeDuration);

        // ��������� ��������� �����
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

        // ��������, ��� �������� �������� ����������� �����
        textToFade.color = new Color(color.r, color.g, color.b, endAlpha);
        textToFade2.color = new Color(color2.r, color2.g, color2.b, endAlpha);
    }

}
