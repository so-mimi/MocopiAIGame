using System;
using UnityEngine;
using DG.Tweening;
using MocopiAIGame;

public class FireBallAnimation : MonoBehaviour
{
    private Transform _cameraTransform;

    private Tween _tween;
    
    private Hit _hit;
    private EnemyController _enemyController;

    private void OnEnable()
    {
        _hit = FindObjectOfType<Hit>();
        _enemyController = FindObjectOfType<EnemyController>();
        _cameraTransform = Camera.main.transform;
        _tween = transform.DOMove(_cameraTransform.position + _cameraTransform.forward * 0.1f, 8f).OnStart(() =>
        {
            Invoke("SlowTime" , 4f);
        }).OnComplete(() =>
        {
            TimeManager.Instance.ResetTime();
            gameObject.SetActive(false);
        });
    }
    
    private void SlowTime()
    {
        TimeManager.Instance.SlowDownTime();
        _hit.OnHit += Hit;
    }


    public void Hit()
    {
        _tween.Kill();
        _tween = transform.DOMove(EnemyPosition.Instance.chestTransform.position, 0.4f).SetEase(Ease.InCubic).OnStart(() =>
        {
            Invoke("ResetTime", 1f);
        }).OnComplete(() =>
        {
            FirePool.Instance.fireBallAnimations.Remove(this);
            _enemyController.DamageAnimation();
            gameObject.SetActive(false);
        });
    }
    
    private void ResetTime()
    {
        TimeManager.Instance.ResetTime();
    }
}