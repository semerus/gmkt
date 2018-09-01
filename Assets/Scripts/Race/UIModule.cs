using System;
using DG.Tweening;
using GiraffeStar;
using UnityEngine;
using UnityEngine.UI;

public class PopupMsg : MessageCore
{
    public string Message;
    public Action OnClick;
}

public class UIModule : Module
{
    private bool isInitialized;
    private GameObject popup;
    private Text debugText;
    private Button commonButton;

    public override void OnRegister()
    {
        base.OnRegister();
        Init();
    }

    void Init()
    {
        if (isInitialized)
        {
            return;
        }

        var root = GameObject.Find("Root");
        popup = root.FindChildByName("Popup");
        debugText = root.FindChildByName("DebugText").GetComponent<Text>();
        commonButton = root.FindChildByName("CommonButton").GetComponent<Button>();
        ClosePopup();

        isInitialized = true;
    }

    public void ClosePopup()
    {
        popup.SetActive(false);
    }
    
    [Subscriber]
    void OpenPopup(PopupMsg msg)
    {
        popup.SetActive(true);
        if (msg.OnClick != null)
        {
            commonButton.onClick.RemoveAllListeners();
            commonButton.onClick.AddListener(() => msg.OnClick());
            commonButton.onClick.AddListener(ClosePopup);
        }
        else
        {
            commonButton.gameObject.SetActive(false);
            DOVirtual.DelayedCall(2f, ClosePopup);
        }

        debugText.gameObject.SetActive(true);
        debugText.text = msg.Message;
    }
}
