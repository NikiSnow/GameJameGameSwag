using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class DeathEffect : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Sprite deathSprite;
    [SerializeField] private float flashDuration = 0.2f;

    [Header("References")]
    [SerializeField] private SpriteRenderer enemySprite;

    [SerializeField] GameObject FlashPref;

    private Color originalColor;

    Camera MainCum;
    private void Awake()
    {
        MainCum = Camera.main;
    }

    public void PlayDeathEffect()
    {
        // Меняем спрайт
        if (deathSprite != null)
        {
            GetComponent<EnemyLogic>()._player.color = new Color(0, 0, 0);
            enemySprite.color = new Color(0, 0, 0);
            enemySprite.sprite = deathSprite;
            Debug.Log("death spri");
        }
        Debug.Log("death cory");
        // Запускаем эффекты
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        Debug.Log("death inside cor");
        Vector2 SpawnPos = new Vector2 (MainCum.transform.position.x, MainCum.transform.position.y);
        GameObject Flash = Instantiate(FlashPref, SpawnPos, Quaternion.identity);
        // Белая вспышка
        yield return new WaitForSeconds(flashDuration);
        Destroy(Flash);
        GetComponent<EnemyLogic>()._player.color = new Color(1, 1, 1);

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