using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    ///////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////

    public void NextLevel(){
        LevelSelector.PlayLevel(LevelSelector.GetNextLevel());
    }

    public void SelectLevel(){
        LevelSelector.LoadScene();
    }

    public void Quit(){
        Application.Quit();
    }

    public static void LoadScene(){
        SceneManager.LoadScene("WinLevel");
    }
}
