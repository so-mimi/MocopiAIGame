using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject freezeBallPrefab = null;
    private Transform cameraTransform = null;
    
    
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }
    
    
    public void SpawnFreezeBall()
    {
        GameObject freezeBall = Instantiate(freezeBallPrefab, cameraTransform.position + Vector3.up * 0.1f, Quaternion.identity);
    }
}
