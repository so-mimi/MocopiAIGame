using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using MocopiAIGame;
using UniRx;
using UnityEngine;

public class ResetGame : MonoBehaviour
{
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private SendData sendData;
    [SerializeField] private MotionDataInputer motionDataInputer;
    [SerializeField] private SimpleButton resetButton;
    [SerializeField] private TutorialSystem tutorialSystem;
    [SerializeField] private GameObject enemy;
    [SerializeField] private EnemyController enemyController;
    
    /// <summary>
    /// TODO:ぜったいここで書く処理ではない
    /// </summary>
    private void Start()
    {
        endPanel.SetActive(false);
        startPanel.SetActive(true);
        Bind();
    }
    
    private void Bind()
    {
        resetButton.OnClick.Subscribe(_ =>
        {
            ResetGameProcess();
        }).AddTo(this);
    }
    
    public void AppearedEndPanel()
    {
        endPanel.SetActive(true);
        startPanel.SetActive(false);
    }
    
    
    private void ResetGameProcess()
    {
        sendData.SendResetCommand();
        motionDataInputer.filePath = "";
        motionDataInputer._fileName = "";
        endPanel.SetActive(false);
        startPanel.SetActive(true);
        tutorialSystem.StartTutorial();
        enemyController.enemyHP.resetHP();
        enemy.SetActive(true);
        tutorialSystem.AppearTutorialObjects();
    }
}
