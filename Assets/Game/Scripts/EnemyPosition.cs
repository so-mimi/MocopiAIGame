using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPosition : MonoBehaviour
{
    public Transform chestTransform = null;
    public static EnemyPosition Instance { get; private set; }
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }
}
