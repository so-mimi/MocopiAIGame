using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ViewOfWhole : MonoBehaviour
{
    [SerializeField] private Image damageImage = null;
    
    public void PlayerDamageEffect()
    {
        damageImage.color = new Color(1f, 0f, 0f, 0.5f);
        damageImage.DOColor(new Color(1f, 0f, 0f, 0f), 0.5f).SetLink(gameObject);
    }
    
    public void SlowEffect()
    {
        damageImage.color = new Color(0.7f, 0f, 1f, 0.1f);
    }

    public void ResetEffect()
    {
        damageImage.color = new Color(0f, 0f, 0f, 0f);
    }
}
