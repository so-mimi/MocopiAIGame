using System;
using System.Collections;
using System.Collections.Generic;
using MocopiAIGame;
using UnityEngine;
using DG.Tweening;

public class FreezeBallAnimation : MonoBehaviour
{
    private EnemyController enemyController = null;
    
    private void OnEnable()
    {
        enemyController = FindObjectOfType<EnemyController>();
        AttackAnimation();
    }

    private void AttackAnimation()
    {
        this.transform.DOMove(EnemyPosition.Instance.chestTransform.position, 2f).SetEase(Ease.Linear).OnStart(() =>
        {
            enemyController.JumpTween.Kill();
            Invoke("ResetTime", 1f);
        }).OnComplete(() =>
        {
            enemyController.DamageAnimation();
            enemyController.BackAnimation();
            gameObject.SetActive(false);
        });
    }

    private void ResetTime()
    {
        TimeManager.Instance.ResetTime();
    }
}
