using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueNS : MonoBehaviour
{
    [SerializeField] GameObject BlackBack;

    [Header("First place")]
    [SerializeField] Image ImagePlace1;
    [SerializeField] TMP_Text TextPlace1;
    [SerializeField] GameObject BlackCover1;

    [Header("Second Place")]
    [SerializeField] Image ImagePlace2;
    [SerializeField] TMP_Text TextPlace2;
    [SerializeField] GameObject BlackCover2;

    [Header("Third Place")]
    [SerializeField] Image ImagePlace3;
    [SerializeField] TMP_Text TextPlace3;
    [SerializeField] GameObject BlackCover3;

    [SerializeField] private List<DialogData> _dialogs = new List<DialogData>();

    [SerializeField] int Counter = 0;
    [SerializeField] int PlaceCounter = 0;

    [SerializeField] bool IsJudi = false;
    [SerializeField] bool IsTutorialEnemy = false;
    [SerializeField] bool IsFinalFather = false;

    [SerializeField] private GameObject PlayerBalance;
    [SerializeField] private wlking wlking;

    bool ThisDialogueIsActive = false;

    private bool Triggered=false;
    [SerializeField] private GameObject Loading;
    [SerializeField] AsunLoadScene AsunLoader;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&& !Triggered)
        {
            Triggered = true;
            StartDialogue();
        }
    }
    public void StartDialogue()
    {
        PlayerBalance.SetActive(false);
        wlking.AbleMove = false;
        BlackBack.SetActive(true);
        ClearPlaces();
        BlackCover1.SetActive(false);
        ThisDialogueIsActive =true;
        ImagePlace1.sprite = _dialogs[Counter].Sprite;
        TextPlace1.text = _dialogs[Counter].Text;
        Counter++;
        PlaceCounter++;
        Debug.Log("Started dialogue on " + _dialogs.Count + " frazes");
    }
    public void AlternativeStartDialogue()
    {
        Debug.Log("Started dialogue on " + _dialogs.Count + " frazes");
        Debug.Log(Counter);
        BlackBack.SetActive(true);
        ClearPlaces();
        //BlackCover1.SetActive(false);
        //ImagePlace1.sprite = _dialogs[Counter].Sprite;
        //TextPlace1.text = _dialogs[Counter].Text;
        Debug.Log(Counter);
        ThisDialogueIsActive = true;
    }
    public void ClearPlaces()
    {
        ImagePlace1.sprite = null;
        TextPlace1.text = " ";
        BlackCover1.SetActive(true);
        BlackCover2.SetActive(true);
        BlackCover3.SetActive(true);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&& ThisDialogueIsActive|| Input.GetMouseButtonDown(0) && ThisDialogueIsActive)
        {
            Debug.Log("IEnteredUpdQqqq");
            DialogQueue();
        }
    }
    public void DialogQueue()
    {
        if (Counter >= _dialogs.Count) //END OF DIALOGUE
        {
            ClearPlaces();
            BlackBack.SetActive(false);
            if (IsJudi)
            {
                Loading.SetActive(true);
                AsunLoader.ChangeLoadActi();
                return;
            }
            if (IsTutorialEnemy)
            {
                PlayerBalance.SetActive(true);
                wlking.AbleMove = true;
                this.gameObject.GetComponent<TutorialEnemyt>().ActivateEnemy();
            }
            if (IsFinalFather)
            {
                SceneManager.LoadScene("LastScene");
                return;
            }
            wlking.AbleMove = true;
            PlayerBalance.SetActive(true);
            return;
        }
        if (PlaceCounter == 3) //SWAP ON NEXT PAGE
        {
            ClearPlaces();
            PlaceCounter = 0;
            BlackCover1.SetActive(true);
            BlackCover2.SetActive(true);
            BlackCover3.SetActive(true);
        }
        if (PlaceCounter == 0) //First placeholder
        {
            ImagePlace1.sprite = _dialogs[Counter].Sprite;
            TextPlace1.text = _dialogs[Counter].Text;
            BlackCover1.SetActive(false);
        }
        else if (PlaceCounter == 1) //Second placeholder
        {
            ImagePlace2.sprite = _dialogs[Counter].Sprite;
            TextPlace2.text = _dialogs[Counter].Text;
            BlackCover2.SetActive(false);
        }
        else if (PlaceCounter == 2) //Third placeholder
        {
            ImagePlace3.sprite = _dialogs[Counter].Sprite;
            TextPlace3.text = _dialogs[Counter].Text;
            BlackCover3.SetActive(false);
        }

        Counter++;
        PlaceCounter++;
    }
}
[System.Serializable]
public class DialogData
{
    public Sprite Sprite;
    public string Text;
}
