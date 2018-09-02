using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmoticonIndicator : MonoBehaviour
{
    int emoteCount = 8;
    List<Image> emotes = new List<Image>();

    void Awake()
    {
        Setup();
    }

    void Setup()
    {
        LoadResources();
        gameObject.SetActive(false);
    }

    void LoadResources()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Race/EmoteLocation") ;
        var path = "Image/face";

        for (int i = 0; i < emoteCount; i++)
        {
            var sprite = Resources.Load<Sprite>(path + (i + 1));
            var emote = GameObject.Instantiate(prefab, transform);
            var image = emote.GetComponent<Image>();
            image.sprite = sprite;

            emotes.Add(image);
            image.gameObject.SetActive(false);
        }
    }

    public void ChangeEmotes(int level)
    {
        CloseEmotes();
        gameObject.SetActive(true);
        if (level < emotes.Count)
        {
            emotes[level].gameObject.SetActive(true);
        }
    }

    public void CloseEmotes()
    {
        foreach (var emote in emotes)
        {
            emote.gameObject.SetActive(false);
        }

        gameObject.SetActive(false);
    }

    public void SleepEmote()
    {
        ChangeEmotes(7);
    }
}
