using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogView : MonoBehaviour
{
    [Header("For Me and Me")]
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _text;
    [Header("Choice")]
    [SerializeField] private Button _firstChoiceButton;
    [SerializeField] private TMP_Text _firstChoiceText;
    [SerializeField] private Button _secondChoiceButton;
    [SerializeField] private TMP_Text _secondChoiceText;

    public void Init(Sprite sprite, string value)
    {
        _image.sprite = sprite;
        _text.text = value;
    }

    public void Init(string firstChoiceValue, string secondChoiceValue)
    {
        _firstChoiceText.text = firstChoiceValue;
        _secondChoiceText.text = secondChoiceValue;
    }

    private void Awake()
    {
        if (_firstChoiceButton != null)
            _firstChoiceButton.onClick.AddListener(FirstChoiceButtonClicked);

        if (_secondChoiceButton != null)
            _secondChoiceButton.onClick.AddListener(SecondChoiceButtonClicked);
    }

    private void OnDestroy()
    {
        if (_firstChoiceButton != null)
            _firstChoiceButton.onClick.RemoveAllListeners();
        
        if (_secondChoiceButton != null)
            _secondChoiceButton.onClick.RemoveAllListeners();
    }

    private void FirstChoiceButtonClicked()
    {
        Destroy(gameObject);
    }

    private void SecondChoiceButtonClicked()
    {
        Destroy(gameObject);
    }
}
