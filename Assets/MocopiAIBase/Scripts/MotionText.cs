using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MotionText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text = null;
    
    public void SetText(string str)
    {
        text.text = str;
        Debug.Log(str);
    }
}
