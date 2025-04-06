using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class DeathEffect : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Sprite deathSprite;
    [SerializeField] private float flashDuration = 0.2f;

    [Header("References")]
    [SerializeField] private Image flashImage;
    [SerializeField] private SpriteRenderer enemySprite;

    [SerializeField] GameObject FlashPref;

    private Color originalColor;

    Camera MainCum;
    private void Awake()
    {
        // Находим компонент вспышки, если не назначен
        //if (flashImage == null)
        //{
        //    var flashObj = GameObject.FindWithTag("FlashEffect");
        //    if (flashObj != null) flashImage = flashObj.GetComponent<Image>();
        //}

        //originalColor = enemySprite.color;
        MainCum = Camera.main;

    }

    public void PlayDeathEffect()
    {
        // Меняем спрайт
        if (deathSprite != null)
        {
             GetComponent<EnemyLogic>()._player.color = new Color(0,0,0);
            enemySprite.sprite = deathSprite;
        }

        // Запускаем эффекты
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        Vector2 SpawnPos = new Vector2 (MainCum.transform.position.x, MainCum.transform.position.y);
        GameObject Flash = Instantiate(FlashPref, SpawnPos, Quaternion.identity);
        // Белая вспышка
        yield return new WaitForSeconds(flashDuration);
        Destroy(Flash);
        GetComponent<EnemyLogic>()._player.color = new Color(1, 1, 1);
        if (flashImage != null)
        {
            flashImage.color = Color.white;
            flashImage.enabled = true;
            yield return new WaitForSeconds(flashDuration);
            flashImage.enabled = false;
        }

        // Исчезновение врага
        float fadeTime = 0.3f;
        float timer = 0;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            enemySprite.color = Color.Lerp(originalColor, Color.clear, timer / fadeTime);
            yield return null;
        }

        Destroy(gameObject);
     
    }
}