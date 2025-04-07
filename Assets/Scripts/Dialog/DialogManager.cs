//using System.Collections.Generic;
//using UnityEngine;

//public class DialogManager : MonoBehaviour
//{
//    [Space(15)]
//    [SerializeField] private Transform _dialogNotMeContainer;
//    [SerializeField] private Transform _dialogMeContainer;
//    [SerializeField] private Transform _dialogChoiceContainer;
//    [Space(15)]
//    [SerializeField] private DialogView _dialogNotMeView;
//    [SerializeField] private DialogView _dialogMeView;
//    [SerializeField] private DialogView _dialogChoiceView;
//    [Space(15)]
//    [SerializeField] private List<DialogData> _dialogs = new List<DialogData>();
    
//    private DialogView _currentNotMeDialog;
//    private DialogView _currentMeDialog;
//    private DialogView _currentChoiceDialog;
//    private int _currentIndex;
//    private bool _isActive;

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (_isActive)
//            return;
            
//        if (other.gameObject.CompareTag("Player"))
//        {
//            _isActive = true;
//            NextDialog();
//        }
//    }
    
//    private void Update()
//    {
//        if (!_isActive)
//            return;
        
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            NextDialog();
//        }
//    }

//    private void NextDialog()
//    {
//        if (_currentChoiceDialog != null)
//            return;
        
//        if (_currentNotMeDialog != null && _currentMeDialog != null && _currentChoiceDialog != null)
//        {
//            Destroy(_currentNotMeDialog.gameObject);
//            Destroy(_currentMeDialog.gameObject);
//            Destroy(_currentChoiceDialog.gameObject);
//        }

//        if (!_isActive)
//            return;
        
//        if (_currentIndex < _dialogs.Count)
//        {
//            switch (_dialogs[_currentIndex].Type)
//            {
//                case DialogType.NotMe:
                    
//                    if (_currentNotMeDialog != null)
//                        Destroy(_currentNotMeDialog.gameObject);
                    
//                    SpawnDialog1(_dialogs[_currentIndex]);
//                    break;
//                case DialogType.Me:
                    
//                    if (_currentMeDialog != null)
//                        Destroy(_currentMeDialog.gameObject);
                    
//                    SpawnDialog2(_dialogs[_currentIndex]);
//                    break;
//                case DialogType.Choice:
                    
//                    if (_currentChoiceDialog != null)
//                        Destroy(_currentChoiceDialog.gameObject);
                    
//                    SpawnDialog3(_dialogs[_currentIndex]);
//                    break;
//            }
            
//            _currentIndex++;
//        }
//        else
//        {
//            if (_currentNotMeDialog != null)
//                Destroy(_currentNotMeDialog.gameObject);
            
//            if (_currentMeDialog != null)
//                Destroy(_currentMeDialog.gameObject);
            
//            if (_currentChoiceDialog != null)
//                Destroy(_currentChoiceDialog.gameObject);
            
//            _isActive = false;
//        }
//    }

//    private void SpawnDialog1(DialogData data)
//    {
//        _currentNotMeDialog = Instantiate(_dialogNotMeView, _dialogNotMeContainer);
//        _currentNotMeDialog.Init(data.Sprite, data.Text);
//    }

//    private void SpawnDialog2(DialogData data)
//    {
//        _currentMeDialog = Instantiate(_dialogMeView, _dialogMeContainer);
//        _currentMeDialog.Init(data.Sprite, data.Text);
//    }

//    private void SpawnDialog3(DialogData data)
//    {
//        _currentChoiceDialog = Instantiate(_dialogChoiceView, _dialogChoiceContainer);
//        _currentChoiceDialog.Init(data.FirstChoiceText, data.SecondChoiceText);
//    }
//}

//[System.Serializable]
//public class DialogData
//{
//    public DialogType Type;
//    public Sprite Sprite;
//    public string Text;
//    public string FirstChoiceText;
//    public string SecondChoiceText;
//}

//[System.Serializable]
//public enum DialogType
//{
//    NotMe,
//    Me,
//    Choice
//}
