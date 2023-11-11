using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePool : MonoBehaviour
{
    public List<FireBallAnimation> fireBallAnimations = new List<FireBallAnimation>();
    
    public static FirePool Instance { get; private set; }
    
    void Start()
    {
        Instance = this;
    }
}
