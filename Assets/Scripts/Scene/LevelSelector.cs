using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    private static List<string> avaliableLevels = new List<string>{"Level1"};
    private static string currentLevel = "Level1";
    private GameObject[] levelButtons;

    // Start is called before the first frame update
    void Start()
    {
        levelButtons = GameObject.FindGameObjectsWithTag("Button");
        HideNotAvaliableLevelButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    ////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////

    public static void PlayLevel(string level){
        if(avaliableLevels.Contains(level)) {
            currentLevel = level;
            SceneManager.LoadScene(level);
        }
    }

    public static void AddAvaliableLevel(string newLevel){
        if(avaliableLevels.Contains(newLevel) == false) {
            int currentIndex = avaliableLevels.IndexOf(currentLevel);
            avaliableLevels.Insert(currentIndex+1, newLevel);
        }
    }

    private void HideNotAvaliableLevelButtons(){
        foreach(GameObject levelButton in levelButtons){
            string level = levelButton.name.Replace("Button", "");

            if(avaliableLevels.Contains(level) == false){
                levelButton.SetActive(false);
            }
        }
    }

    public static void LoadScene(){
        SceneManager.LoadScene("LevelSelector");
    }

    ////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////

    public static string GetCurrentLevel(){
        return currentLevel;
    }

    public static string GetNextLevel(){
        int currentIndex = avaliableLevels.IndexOf(currentLevel);

        return avaliableLevels[currentIndex+1];
    }
}
