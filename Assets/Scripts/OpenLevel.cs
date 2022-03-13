using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLevel : MonoBehaviour
{
    private bool triggerOnce = false;

    [SerializeField] private string openLevel;
    [SerializeField] private bool endLevel = true;
    [SerializeField, Range(0, 75)] private int slowDownPercent = 0;

    // Start is called before the first frame update
    void Start()
    {
        NormalTime();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if(other.name.Contains("Player") && triggerOnce == false){
            triggerOnce = true;
            LevelSelector.AddAvaliableLevel(openLevel);

            if(endLevel) {
                SlowDownTime();
                StartCoroutine(GoToWinScene());
            }
        }
    }

    /////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////

    private IEnumerator GoToWinScene(){
        yield return new WaitForSeconds(1);

        WinLevel.LoadScene();

        yield return null;
    }

    private void SlowDownTime(){
        float fixedDeltaTime = Time.fixedDeltaTime;
        Time.timeScale = (float)(-(slowDownPercent-100))/(float)100;
        Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
    }

    private void NormalTime(){
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
    }
}
