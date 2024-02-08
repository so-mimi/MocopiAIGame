using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject freezeBallPrefab;
    [SerializeField] private GameObject pkFireBallPrefab;
    [SerializeField] private ViewOfWhole viewOfDamage;
    [SerializeField] private PlayerHP playerHP;
    private Transform cameraTransform = null;
    [SerializeField] private Transform fireSpawnPoint;


    public void SpawnFreezeBall()
    {
        GameObject freezeBall = Instantiate(freezeBallPrefab,  fireSpawnPoint.position, Quaternion.identity);
    }

    public void SpawnPlayerFire()
    {
        GameObject pkFireBall = Instantiate(pkFireBallPrefab, fireSpawnPoint.position, Quaternion.identity);
    }
    
    public void PlayerDamageEffect()
    {
        viewOfDamage.PlayerDamageEffect();
        playerHP.Damage(12f);
    }
}
