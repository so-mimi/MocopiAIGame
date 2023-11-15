using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MocopiAIGame;
using UnityEngine;

public class PKFireBallAnimation : MonoBehaviour
{
    private EnemyController enemyController;
    private void OnEnable()
    {
        enemyController = FindObjectOfType<EnemyController>();
        if (TimeManager.Instance.isSlowDown)
        {
            TimeManager.Instance.ResetTime();
        }
        StartFireBallAnimation();
    }
    
    private void StartFireBallAnimation()
    {
        transform.DOMove(EnemyPosition.Instance.chestTransform.position, 1f)
            .SetEase(Ease.Linear).OnComplete(() =>
            {
                enemyController.DamageEffect();
                gameObject.SetActive(false);
            });
    }
}
