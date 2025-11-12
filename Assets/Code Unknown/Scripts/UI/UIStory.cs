using cfg;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIStory : MonoBehaviour
{
    [Tooltip("The prefab for the option to be created for each log entry")]
    [SerializeField] private GameObject optionPrefab;

    [Tooltip("The parent of all of the options in the log selection screen")]
    [SerializeField] private Transform optionsParent;

    [Tooltip("The text object that displays the title of the current log")]
    [SerializeField] private TextMeshProUGUI logTitle;

    [Tooltip("The text object that displays the contents of the current log")]
    [SerializeField] private TextMeshProUGUI logText;

    [Tooltip("The scrollbar tied to the text that displays the log")]
    [SerializeField] private Scrollbar textScrollbar;

    private TbStoryParam storyData;

    private List<GameObject> titles = new List<GameObject>();
    private void Start()
    {
        storyData = LubanTablesMgr.Instance.tables.TbStoryParam;
        
        int amount = storyData.DataMap.Count;
        for (int i = 0; i < amount; i++)
        {
            GameObject title = Instantiate(optionPrefab, optionsParent);
            int i_ = i; //necessary to "freeze" the variable for delegate
            title.GetComponent<Button>().onClick.AddListener(delegate { ViewEntry(i_); });
            titles.Add(title);
        }

        int optionIndex = 0;
        foreach (GameObject title in titles)
        {
            TextMeshProUGUI titleText = title.GetComponent<TextMeshProUGUI>();

            bool unlocked = StoryUnlockManager.Instance.IsUnlocked(optionIndex);
            if (unlocked)
            {
                titleText.text = storyData.DataList[optionIndex].Title;
            }
            else
            {
                titleText.text = "[ Locked ]";
                title.GetComponent<Button>().interactable = false;
            }

            optionIndex++;
        }
    }

    //Exit the UI and return to the start screen
    public void Exit()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    //Select a current entry and allow the player to view it
    public void ViewEntry(int index)
    {
        //Don't do anything if the entry is locked!
        if (StoryUnlockManager.Instance.IsUnlocked(index) == false)
        {
            return;
        }

        //Display the title and text for this log entry
        logTitle.text = storyData.DataList[index].Title;
        logText.text = storyData.DataList[index].Text;

        //Reset the scrollbar's position to the start of the entry
        textScrollbar.value = 1;

        //Un-highlight the old selected entry, and highlight the new selected entry
        for (int i = 0; i < titles.Count; i++)
        {
            TextMeshProUGUI titleText = titles[i].GetComponent<TextMeshProUGUI>();
            if (i == index)
            {
                titleText.color = new Color(1.0f, 1.0f, 0.0f);
            }
            else if (StoryUnlockManager.Instance.IsUnlocked(index))
            {
                titleText.color = new Color(1.0f, 1.0f, 1.0f);
            }
        }
    }
}
