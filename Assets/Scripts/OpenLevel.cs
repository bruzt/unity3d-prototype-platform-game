using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLevel : MonoBehaviour
{
    private bool triggerOnce = false;

    [SerializeField] private string openLevel;
    [SerializeField] private bool endLevel = true;

    // Start is called before the first frame update
    void Start()
    {
        
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
                SlowDownTime(0.5f);
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

    private void SlowDownTime(float slowPercent){
        float fixedDeltaTime = Time.fixedDeltaTime;
        Time.timeScale = slowPercent;
        Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
    }
}
