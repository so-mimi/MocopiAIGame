using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PKFireBallAnimation : MonoBehaviour
{
    private void OnEnable()
    {
        StartFireBallAnimation();
    }
    
    private void StartFireBallAnimation()
    {
        transform.DOMove(EnemyPosition.Instance.chestTransform.position, 1f)
            .SetEase(Ease.Linear).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }
}
