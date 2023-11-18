using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private Slider slider = null;
    private float maxHP = 100f;
    private float currentHP = 100f;

    private void Start()
    {
        currentHP = maxHP;
        SetHP(currentHP);
    }
    
    public void SetHP(float hp)
    {
        slider.value = hp;
    }
    
    public void Damage(float damage)
    {
        currentHP -= damage;
        SetHP(currentHP);
    }
    
}