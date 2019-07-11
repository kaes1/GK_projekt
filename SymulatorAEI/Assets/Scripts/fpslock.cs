using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fpslock : MonoBehaviour
{
    public int target = 60;
    public float deltaTime = 0.0f;
    public float fps;
    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = target;
        QualitySettings.vSyncCount = 0;
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;
    }

}
