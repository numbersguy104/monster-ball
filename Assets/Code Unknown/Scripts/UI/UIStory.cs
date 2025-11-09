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

    private List<GameObject> titles = new List<GameObject>();
    private void Start()
    {
        //TODO: Get the amount of titles from data sheets instead of just using 10
        for (int i = 0; i < 10; i++)
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
                //TODO: Get the title from data sheets instead of just "Log #1" "Log #2" etc.
                titleText.text = "Log #" + (optionIndex + 1).ToString();
            }
            else
            {
                titleText.text = "[ Locked ]";
                titleText.color = new Color(0.75f, 0.75f, 0.75f);
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
        //TODO: Read from sheets instead of just displaying the log number
        logTitle.text = "Log #" + (index+1).ToString();
        logText.text = "This is log " + (index+1).ToString() + "!";

        //Un-highlight the old selected entry, and highlight the new selected entry
        foreach (GameObject title in titles)
        {
            title.transform.GetChild(0).gameObject.SetActive(false);
        }
        titles[index].transform.GetChild(0).gameObject.SetActive(true);
    }
}
