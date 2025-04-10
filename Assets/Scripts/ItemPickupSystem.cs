using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ItemPickupSystem : MonoBehaviour
{
    [System.Serializable]
    public class ItemType
    {
        public string name;
        public int count;
        public TMP_Text uiText;
        public int pickupAmount;
        public string tag;
    }

    [Header("Настройки")]
    [SerializeField] private float pickupDistance = 2f;
    [SerializeField] private TMP_Text pickupPromptText;
    [SerializeField] private TMP_Text notificationText;
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

    [SerializeField] Button Puffbut;

    [Header("CheckpointInfo")]
    [SerializeField] public Transform CheckPointPosition;
    [SerializeField] private int WasAmmo;
    [SerializeField] private int WasSiga;

    [SerializeField] private List<GameObject> NonCheckedItems = new List<GameObject>();
    [SerializeField] public List<AdvancedEnemySpawner> NonCheckedEnemyTriggers = new List<AdvancedEnemySpawner>();

    [SerializeField] GunStateController GunState;

    private void Start()
    {
        FindAllItemsInScene();
        UpdateUI();
        UpdatePuffButton();
        HideNotification();
    }

    private void Update()
    {
        CheckNearestItem();
        TryPickupItem();
    }

    public void CheckPointReached(Transform CheckPointPos)
    {
        Debug.Log("CheckPoint Reached");
        CheckPointPosition = CheckPointPos;
        WasAmmo = items.Find(item => item.name == "Патроны").count;
        WasSiga = items.Find(item => item.name == "Сигареты").count;
        UpdateUI();
        UpdatePuffButton();
        NonCheckedItems.Clear();
        NonCheckedEnemyTriggers.Clear();
    }
    public bool RespawnOnLastCheckP()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1f);
        GameObject[] objectsWithFlash = GameObject.FindGameObjectsWithTag("FlashAfterDeath");
        foreach (GameObject obj in objectsWithFlash)
        {
            Destroy(obj);
        }
        if (CheckPointPosition==null)
            return false;
        GunState.EnableGun();
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject obj in objectsWithTag)
        {
            Destroy(obj);
        }
        Debug.Log("CheckPoint Respawned");
        items.Find(item => item.name == "Патроны").count = WasAmmo;
        items.Find(item => item.name == "Сигареты").count = WasSiga;
        UpdateUI();
        UpdatePuffButton();
        foreach (var item in NonCheckedItems)
        {
            item.SetActive(true);
            sceneItems.Add(item);
        }
        NonCheckedItems.Clear();
        foreach (var trigger in NonCheckedEnemyTriggers)
        {
            trigger.GetComponent<AdvancedEnemySpawner>().triggered = false;
        }
        NonCheckedEnemyTriggers.Clear();

        return true;
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
                    UpdatePuffButton();
                }
                else if (itemType.name == "Патроны")
                {
                    ShowNotification("Немного патронов");
                }

                sceneItems.Remove(nearestItem);
                nearestItem.SetActive(false);
                NonCheckedItems.Add(nearestItem);

            }
        }
    }
    public void StartedPuffing()
    {
        ItemType SigaItem = items.Find(item => item.name == "Сигареты");
        SigaItem.count -= 1;
        UpdateUI();
        UpdatePuffButton();
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
    private void UpdatePuffButton()
    {
        ItemType SigaItem = items.Find(item => item.name == "Сигареты");
        if (SigaItem.count > 0)
        {
            Puffbut.interactable = true;
        }
        else
        {
            Puffbut.interactable = false;
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

    public int GetAmmoCount()
    {
        ItemType ammoItem = items.Find(item => item.name == "Патроны");
        return ammoItem?.count ?? 0;
    }
    public void DecAmmoCount()
    {
        ItemType ammoItem = items.Find(item => item.name == "Патроны");
        ammoItem.count -= 1;
        UpdateUI();

    }
}