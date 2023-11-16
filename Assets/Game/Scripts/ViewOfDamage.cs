using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ViewOfDamage : MonoBehaviour
{
    [SerializeField] private Image damageImage = null;
    
    public void PlayerDamageEffect()
    {
        damageImage.color = new Color(1f, 0f, 0f, 0.5f);
        damageImage.DOColor(new Color(1f, 0f, 0f, 0f), 0.5f).SetLink(gameObject);
    }
}
