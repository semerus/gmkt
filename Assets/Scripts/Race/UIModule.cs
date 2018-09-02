using System;
using DG.Tweening;
using GiraffeStar;
using UnityEngine;
using UnityEngine.UI;

public class PopupMsg : MessageCore
{
    public string Message;
    public string ButtonMessage;
    public Action OnClick;
}

public class UIModule : Module
{
    private bool isInitialized;
    private GameObject popup;
    private Text debugText;
    private Button commonButton;
    private Text speedText;

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
        speedText = root.FindChildByName("Speed").GetComponent<Text>();
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
            var buttonText = commonButton.GetComponentInChildren<Text>();
            buttonText.text = msg.ButtonMessage;
            commonButton.gameObject.SetActive(true);
        }
        else
        {
            commonButton.gameObject.SetActive(false);
            DOVirtual.DelayedCall(2f, ClosePopup);
        }

        debugText.gameObject.SetActive(true);
        debugText.text = msg.Message;
    }

    public void ShowSpeed(float speed)
    {
        var showSpeed = speed * 3000f;
        var rounded = Mathf.RoundToInt(showSpeed);
        var adjusted = rounded * 0.1f;
        speedText.text = adjusted.ToString();
    }
}
