using System;
using UnityEngine;
using DG.Tweening;

public class FireBallAnimation : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    
    private void OnEnable()
    {
        _cameraTransform = Camera.main.transform;
        transform.DOMove(_cameraTransform.position + _cameraTransform.forward * 0.1f, 8f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
