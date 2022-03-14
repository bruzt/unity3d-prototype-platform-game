using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLevel : MonoBehaviour
{
    private bool triggerOnce = false;
    private float normalTimeScale;
    private float normalFixedDeltaTime;

    [SerializeField] private string openLevel;
    [SerializeField] private bool endLevel = true;
    [SerializeField, Range(0, 75)] private int slowDownPercent = 0;

    // Start is called before the first frame update
    void Start()
    {
        normalTimeScale = Time.timeScale;
        normalFixedDeltaTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if(other.name.Contains("Player") && triggerOnce == false){
            triggerOnce = true;
            LevelSelector.AddAvaliableLevel(openLevel);

            if(endLevel) StartCoroutine(GoToWinScene());
        }
    }

    /////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////

    private IEnumerator GoToWinScene(){

        SlowDownTime();

        yield return new WaitForSeconds(1);

        NormalTime();

        WinLevel.LoadScene();

        yield return null;
    }

    private void SlowDownTime(){
        Time.timeScale = (float)(-(slowDownPercent-100))/(float)100;
        Time.fixedDeltaTime = Time.fixedDeltaTime * Time.timeScale;
    }

    private void NormalTime(){
        Time.timeScale = normalTimeScale;
        Time.fixedDeltaTime = normalFixedDeltaTime;
    }
}
