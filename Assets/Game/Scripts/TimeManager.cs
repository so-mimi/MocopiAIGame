using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private ViewOfWhole viewOfWhole = null;
    public bool isSlowDown { get; private set; }
    public static TimeManager Instance { get; private set; }
    void Start()
    {
        Instance = this;
    }

    public void SlowDownTime()
    {
        Time.timeScale = 0.2f;
        viewOfWhole.SlowEffect();
        isSlowDown = true;
    }
    
    public void ResetTime()
    {
        Time.timeScale = 1f;
        viewOfWhole.ResetEffect();
        isSlowDown = false;
    }
    
    public void StopTime()
    {
        Time.timeScale = 0.05f;
    }
}
