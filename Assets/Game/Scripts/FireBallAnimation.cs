using System;
using UnityEngine;
using DG.Tweening;
using MocopiAIGame;
using Cysharp.Threading.Tasks;

public class FireBallAnimation : MonoBehaviour
{
    private Transform _cameraTransform;
    private Tween _tween;
    private Hit _hit;
    private EnemyController _enemyController;
    private bool _isHit = false;

    private void OnEnable()
    {
        _hit = FindObjectOfType<Hit>();
        _enemyController = FindObjectOfType<EnemyController>();
        _cameraTransform = Camera.main.transform;
        _tween = transform.DOMove(_cameraTransform.position + _cameraTransform.forward * 0.1f, 5f).OnStart(() =>
        {
            FireBallTime();
        }).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private async UniTask FireBallTime()
    {
        await UniTask.Delay(2500);
        TimeManager.Instance.SlowDownTime();
        _hit.OnHit += Hit;
        await UniTask.Delay(1500);
        _hit.OnHit -= Hit;
        if (TimeManager.Instance.isSlowDown)
        {
            TimeManager.Instance.ResetTime();
        }
    }


    public void Hit()
    {
        if (_isHit) return;
        _isHit = true;
        _tween.Kill();
        _tween = transform.DOMove(EnemyPosition.Instance.chestTransform.position, 1f).SetEase(Ease.InCubic).OnStart(() =>
        {
            TimeManager.Instance.ResetTime();
        }).OnComplete(() =>
        {
            FirePool.Instance.fireBallAnimations.Remove(this);
            _enemyController.DamageAnimation();
            gameObject.SetActive(false);
        });
    }
}