using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ItemPickupSystem : MonoBehaviour
{
    [System.Serializable]
    public class ItemType
    {
        public string name;
        public int count;
        public Text uiText;
        public int pickupAmount;
        public string tag;
    }

    [Header("Настройки")]
    [SerializeField] private float pickupDistance = 2f;
    [SerializeField] private Text pickupPromptText;
    [SerializeField] private Text notificationText;
    [SerializeField] private KeyCode pickupKey = KeyCode.F;
    [SerializeField] private float notificationDuration = 1.5f;

    [Header("Предметы")]
    [SerializeField]
    private List<ItemType> items = new List<ItemType> {
        new ItemType {
            name = "Патроны",
            count = 0,
            tag = "Ammo",
            pickupAmount = 10
        },
        new ItemType {
            name = "Сигареты",
            count = 0,
            tag = "Cigarretes",
            pickupAmount = 5
        }
    };

    private List<GameObject> sceneItems = new List<GameObject>();
    private GameObject nearestItem;
    private Coroutine notificationCoroutine;

    private void Start()
    {
        FindAllItemsInScene();
        UpdateUI();
        HideNotification();
    }

    private void Update()
    {
        CheckNearestItem();
        TryPickupItem();
    }

    private void FindAllItemsInScene()
    {
        sceneItems.Clear();
        foreach (var itemType in items)
        {
            GameObject[] foundItems = GameObject.FindGameObjectsWithTag(itemType.tag);
            sceneItems.AddRange(foundItems);
        }
    }

    private void CheckNearestItem()
    {
        nearestItem = null;
        float minDistance = float.MaxValue;

        foreach (var item in sceneItems)
        {
            if (item == null) continue;

            float distance = Vector3.Distance(transform.position, item.transform.position);
            if (distance < pickupDistance && distance < minDistance)
            {
                minDistance = distance;
                nearestItem = item;
            }
        }

        if (pickupPromptText != null)
        {
            pickupPromptText.gameObject.SetActive(nearestItem != null);
            pickupPromptText.text = "Подобрать [F]";
        }
    }

    private void TryPickupItem()
    {
        if (nearestItem != null && Input.GetKeyDown(pickupKey))
        {
            string itemTag = nearestItem.tag;
            ItemType itemType = items.Find(i => i.tag == itemTag);

            if (itemType != null)
            {
                itemType.count += itemType.pickupAmount;
                UpdateUI();

                // Показываем соответствующее уведомление
                if (itemType.name == "Сигареты")
                {
                    ShowNotification("Сига");
                }
                else if (itemType.name == "Патроны")
                {
                    ShowNotification("Немного патронов");
                }

                sceneItems.Remove(nearestItem);
                Destroy(nearestItem);
            }
        }
    }

    private void UpdateUI()
    {
        foreach (var item in items)
        {
            if (item.uiText != null)
            {
                item.uiText.text = $"{item.count}";
            }
        }
    }

    private void ShowNotification(string message)
    {
        if (notificationText != null)
        {
            // Останавливаем предыдущее уведомление
            if (notificationCoroutine != null)
            {
                StopCoroutine(notificationCoroutine);
            }

            // Показываем новое
            notificationText.text = message;
            notificationText.gameObject.SetActive(true);
            notificationCoroutine = StartCoroutine(HideNotificationAfterDelay());
        }
    }

    private IEnumerator HideNotificationAfterDelay()
    {
        yield return new WaitForSeconds(notificationDuration);
        HideNotification();
    }

    private void HideNotification()
    {
        if (notificationText != null)
        {
            notificationText.gameObject.SetActive(false);
        }
    }
}