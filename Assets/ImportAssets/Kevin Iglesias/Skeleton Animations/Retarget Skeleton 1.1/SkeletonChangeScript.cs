using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonChangeScript : MonoBehaviour
{
    bool generic;
    
    public GameObject[] skeleton;
    
    public bool change;
    
    public void Change()
    {
        generic = !generic;
        
        if(generic)
        {
            skeleton[0].SetActive(false);
            skeleton[1].SetActive(true);
        }else{
            skeleton[1].SetActive(false);
            skeleton[0].SetActive(true);
        }
    }
    
    void Update()
    {
        if(change)
        {
            change = false;
            Change();
        }
    }
}
