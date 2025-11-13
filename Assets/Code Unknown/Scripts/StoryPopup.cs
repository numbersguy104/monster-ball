using System;
using TMPro;
using UnityEngine;

public class StoryPopup : MonoBehaviour
{
    [Tooltip("How long the popup should take to appear/disappear, in seconds")]
    [SerializeField] float fadeTime = 0.04f;

    [Tooltip("How long the popup should last for, in seconds")]
    [SerializeField] float duration = 7.0f;

    [Tooltip("The text where the title of the log entry is displayed")]
    [SerializeField] TextMeshProUGUI contentText;

    [NonSerialized] public int entryIndex = 0;

    private float timer = 0.0f;
    private void Awake()
    {
        transform.localScale = new Vector3(1.0f, 0.0f, 1.0f);
    }

    private void Start()
    {
        string title = LubanTablesMgr.Instance.tables.TbStoryParam.DataList[entryIndex].Title;
        contentText.text = "\"" + title + "\"\n can be read from the Collection menu.";
    }

    private void Update()
    {

        float scaleY = 1.0f;
        if (timer > duration)
        {
            Destroy(gameObject);
        }
        else if (timer < fadeTime)
        {
            scaleY = timer / fadeTime;
        }
        else if (duration - timer < fadeTime)
        {
            scaleY = (duration - timer) / fadeTime;
        }
        transform.localScale = new Vector3(1.0f, scaleY, 1.0f);
        timer += Time.deltaTime;
    }
}
