using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLevel : MonoBehaviour
{
    private Camera mainCamera;
    private Transform mainCameraTransform;
    private bool triggerOnce = false;
    private float normalTimeScale;
    private float normalFixedDeltaTime;
    private bool levelEnded = false;

    [SerializeField] private string openLevel;
    [SerializeField] private bool shouldEndLevel = true;
    [SerializeField, Range(0, 75)] private int slowDownPercent = 0;
    [SerializeField] private float cameraFieldOfViewWin = 25;
    [SerializeField] private float cameraRotationXWin = 25;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        mainCameraTransform = mainCamera.GetComponent<Transform>();

        normalTimeScale = Time.timeScale;
        normalFixedDeltaTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(levelEnded) ZoomCamera();
    }

    void OnTriggerEnter(Collider other){
        if(other.name.Contains("Player") && triggerOnce == false){
            triggerOnce = true;
            LevelSelector.AddAvaliableLevel(openLevel);

            if(shouldEndLevel) StartCoroutine(EndLevelCoroutine());
        }
    }

    /////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////

    private IEnumerator EndLevelCoroutine(){

        SlowDownTime();

        levelEnded = true;

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

    private void ZoomCamera(){
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, cameraFieldOfViewWin, Time.deltaTime);

        float cameraRotationX = Mathf.Lerp(mainCameraTransform.rotation.eulerAngles.x, cameraRotationXWin, Time.deltaTime);
        mainCameraTransform.rotation = Quaternion.Euler(cameraRotationX, 0, 0);
    }
}
