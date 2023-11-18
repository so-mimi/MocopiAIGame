using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject freezeBallPrefab;
    [SerializeField] private GameObject pkFireBallPrefab;
    [SerializeField] private ViewOfDamage viewOfDamage;
    [SerializeField] private PlayerHP playerHP;
    private Transform cameraTransform = null;
    
    
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }
    
    
    public void SpawnFreezeBall()
    {
        GameObject freezeBall = Instantiate(freezeBallPrefab, cameraTransform.position + Vector3.up * 0.1f, Quaternion.identity);
    }

    public void SpawnPlayerFire()
    {
        Debug.Log("PlayerFire");
        GameObject pkFireBall = Instantiate(pkFireBallPrefab, cameraTransform.position + Vector3.forward * 0.1f, Quaternion.identity);
    }
    
    public void PlayerDamageEffect()
    {
        viewOfDamage.PlayerDamageEffect();
        playerHP.Damage(20f);
    }
}
