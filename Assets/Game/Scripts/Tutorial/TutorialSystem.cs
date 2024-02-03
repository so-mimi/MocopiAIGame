using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MocopiAIGame;
using TMPro;
using UniRx;
using UnityEngine;

public class TutorialSystem : MonoBehaviour
{
    [Header("表示画面")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private CanvasGroup tutorialCanvasGroup;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private CanvasGroup countDownCanvasGroup;
    [SerializeField] private TextMeshProUGUI countDownText;
    
    [Header("インタラクティブUI")]
    [SerializeField] private SimpleButton startButton;
    
    [Header("システム")]
    [SerializeField] private MotionDataInputer motionDataInputer;
    
    private EnemyController _enemyController;
    
    void Start()
    {
        tutorialPanel.SetActive(true);
        tutorialCanvasGroup.alpha = 0;
        _enemyController = FindObjectOfType<EnemyController>();
        _enemyController.isTutorial = true;
        Bind();
    }
    
    private void Bind()
    {
        startButton.OnClick.Subscribe(_ =>
        {
            tutorialPanel.SetActive(false);
            PlayTutorialGuide();
        }).AddTo(this);
    }

    private async void PlayTutorialGuide()
    {
        tutorialCanvasGroup.alpha = 0;
        tutorialCanvasGroup.DOFade(1, 0.5f).SetLink(gameObject);
        tutorialText.text = "チュートリアルをはじめるよ！";
        await UniTask.Delay(TimeSpan.FromSeconds(3f));
        await ChangeTutorialText("このゲームでは動きのデータを記録して\nあなたの動きに合わせたAIを作成するよ！");
        await UniTask.Delay(TimeSpan.FromSeconds(5f));
        await ChangeTutorialText("敵の攻撃に合わせて反撃の動きをしよう！");
        await UniTask.Delay(TimeSpan.FromSeconds(4f));
        PunchTutorial();
    }
    
    private async UniTask PunchTutorial()
    {
        await ChangeTutorialText("敵がこの動きをしてきたら\nパンチで反撃しよう！");
        _enemyController.WalkAnimation();
        // チュートリアル用のパンチイベントを発行が発行されるまで待つ
        await _enemyController.OnPunch.First();
        await CountDown(4);
        
    }
    
    
    /// <summary>
    /// チュートリアルの文字の切り替えをいい感じにする
    /// </summary>
    /// <param name="text"></param>
    private async UniTask ChangeTutorialText(string text)
    {
        await tutorialCanvasGroup.DOFade(0, 0.5f).SetLink(gameObject);
        tutorialText.text = text;
        await tutorialCanvasGroup.DOFade(1, 0.5f).SetLink(gameObject);
    }
    
    /// <summary>
    /// 0番目:PKファイヤ 1番目PKフリーズ 2番目:打つ 3番目:その他 4番目:パンチ 5番目:蹴る
    /// </summary>
    /// <param name="motionNumber"></param>
    private async UniTask CountDown(int motionNumber)
    {
        countDownCanvasGroup.alpha = 0;
        countDownCanvasGroup.DOFade(1, 0.5f).SetLink(gameObject);
        List<float> motionData = new List<float>();
        //motionDataはmodel.motionCountの長さを持ち、currentCountDownMotionNumber番目の要素にはcurrentCountDownTimes番目だけ1で、ほかは0が入っている
        for (int i = 0; i < 6; i++)
        {
            if (i == motionNumber)
            {
                motionData.Add(1);
            }
            else
            {
                motionData.Add(0);
            }
        }
        // UniRxを使って5秒のカウントダウンを行う、のこり2秒になったらMotionDataInputerに記録を開始させる, その後カウントダウンを非表示にする
        Observable.Timer(System.TimeSpan.FromSeconds(0), System.TimeSpan.FromSeconds(1))
            .Take(6)
            .Subscribe(count =>
            {
                countDownText.text = (5 - count).ToString();
                if (count == 2)
                {
                    motionDataInputer.DataInputTimer(motionData);
                }
                if (count == 5)
                {
                    countDownCanvasGroup.DOFade(0, 0.5f).SetLink(gameObject);
                }
            }).AddTo(this);
    }
    
}
