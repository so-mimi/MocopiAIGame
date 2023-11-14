using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public bool isSlowDown;
    public static TimeManager Instance { get; private set; }
    void Start()
    {
        Instance = this;
    }

    public void SlowDownTime()
    {
        Time.timeScale = 0.2f;
        isSlowDown = true;
    }
    
    public void ResetTime()
    {
        Time.timeScale = 1f;
        isSlowDown = false;
    }
}
