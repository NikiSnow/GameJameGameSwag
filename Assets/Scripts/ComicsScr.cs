using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComicsScr : MonoBehaviour
{
    [SerializeField] Image MainImage;
    [SerializeField] private List<Sprite> Sprites = new List<Sprite>();
    [SerializeField] DialogueNS DialogueNS;
    private int counter = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)|| Input.GetMouseButtonDown(0))
        {
            NextFrameOfComics();
        }
    }
    public void NextFrameOfComics()
    {
        if (counter >= Sprites.Count)
        {
            DialogueNS.AlternativeStartDialogue();
            this.enabled=false;
            return;
        }
        MainImage.sprite = Sprites[counter];
        counter++;
    }
}
