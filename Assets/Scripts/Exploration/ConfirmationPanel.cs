using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum eConfirmationType : uint
{ 
    none = 0,
    validate = 1, 
    cancel = 1 << 1, 
    both  = validate | cancel
}
public class ConfirmationPanel : MonoBehaviour
{
    [SerializeField] private Button _validateButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private GameObject _panel;
    [SerializeField] private CanvasGroup _canvasGroup;

    public delegate void OnConfirmation (bool answer);
    private OnConfirmation _confirmationFun = null;
    public bool hideOnValidate = true;


    private void Start()
    {
        _validateButton.onClick.AddListener(Validate);
        _cancelButton.onClick.AddListener(Cancel);
        _panel.SetActive(false);
        if (_canvasGroup == null) { GetComponent<CanvasGroup>(); }
    }
    protected void Validate()
    {
        _confirmationFun?.Invoke(true);
        if (hideOnValidate)
            Hide();
    }
    protected void Cancel()
    {
        _confirmationFun?.Invoke(false);
        if (hideOnValidate)
            Hide();
    }
    public void Hide()
    {
        if (_canvasGroup == null)
        {
            _panel.SetActive(false);
        }
        else
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }

    }
    public void BindAction(OnConfirmation fun)
    {
        _confirmationFun = fun;
    }


    /// <summary>
    /// Display the confirmation panel and link 
    /// If the player confirm, it will call the bind function with true and hide
    /// If the player cancel, it will call the bind function with end and hide
    /// </summary>
    /// <param name="confirmationType"></param>
    public void Display(eConfirmationType confirmationType)
    {

        _panel.SetActive(true);
        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
        }

        _validateButton.interactable = (confirmationType & eConfirmationType.validate) > 0;
        _cancelButton.interactable = (confirmationType & eConfirmationType.cancel) > 0;
    }
    public void Display(eConfirmationType confirmationType, OnConfirmation fun)
    {
        BindAction(fun);
        Display(confirmationType);
    }

}
