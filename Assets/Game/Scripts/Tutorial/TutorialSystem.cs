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
    [SerializeField] private GameObject creditPanel;
    
    [Header("インタラクティブUI")]
    [SerializeField] private SimpleButton startButton;
    [SerializeField] private SimpleButton creditButton;
    
    [Header("システム")]
    [SerializeField] private MotionDataInputer motionDataInputer;
    [SerializeField] private SendData sendData;

    [SerializeField]
    private TutorialVideoPlayer tutorialVideoPlayer;
    
    private EnemyController _enemyController;
    
    [SerializeField] private GameObject[] onlyTutorialObjects;
    
    void Start()
    {
        _enemyController = FindObjectOfType<EnemyController>();
        Bind();
        StartTutorial();
    }
    
    private void Bind()
    {
        startButton.OnClick.Subscribe(_ =>
        {
            tutorialPanel.SetActive(false);
            PlayTutorialGuide();
        }).AddTo(this);
        
        creditButton.OnClick.Subscribe(_ =>
        {
            SwitchCredit();
        }).AddTo(this);
    }
    
    public void StartTutorial()
    {
        tutorialPanel.SetActive(true);
        tutorialCanvasGroup.alpha = 0;
        _enemyController.isTutorial = true;
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
        PunchTutorial().Forget();
    }
    
    private async UniTask PunchTutorial()
    {
        await ChangeTutorialText("敵がこの動きをしてきたら\nパンチで反撃しよう！");
        //TODO: ここで0に合わせてパンチをする旨、パンチの動画を再生する
        _enemyController.WalkAnimation();
        // チュートリアル用のパンチイベントを発行が発行されるまで待つ
        await _enemyController.OnPunch.First();
        await tutorialVideoPlayer.PlayPunchVideo();
        //4がパンチのモーション番号
        await CountDown(4);
        _enemyController.DefaultPositionAndAnimation();
        KickTutorial().Forget();
    }

    private async UniTask KickTutorial()
    {
        await ChangeTutorialText("敵がこの動きをしてきたら\nキックで反撃しよう！");
        //TODO: ここで0に合わせてkickをする旨、パンチの動画を再生する
        _enemyController.WalkAnimationTwo();
        // チュートリアル用のキックイベントを発行が発行されるまで待つ
        await _enemyController.OnTwoHandAttack.First();
        await tutorialVideoPlayer.PlayKickVideo();
        //5がキックのモーション番号
        await CountDown(5);
        _enemyController.DefaultPositionAndAnimation();
        HitTutorial().Forget();
    }

    private async UniTask HitTutorial()
    {
        await ChangeTutorialText("敵がこの動きをしてきたら\n打ち返す動きで反撃しよう！");
        //TODO: ここで0に合わせてkickをする旨、パンチの動画を再生する
        _enemyController.ThrowAnimation();
        // チュートリアル用のヒットイベントを発行が発行されるまで待つ
        await _enemyController.OnThrow.First();
        await tutorialVideoPlayer.PlayHitVideo();
        //2がヒットのモーション番号
        await CountDown(2);
        FirePool.Instance.Clean();
        _enemyController.DefaultPositionAndAnimation();
        PKFreezeTutorial().Forget();
    }

    private async UniTask PKFreezeTutorial()
    {
        await ChangeTutorialText("敵がこの動きをしてきたら\n腕をクロスして反撃しよう！");
        _enemyController.JumpAnimation();
        // チュートリアル用のPKサンダーイベントを発行が発行されるまで待つ
        await _enemyController.OnJump.First();
        await tutorialVideoPlayer.PlayPKThunderVideo();
        //1がPKサンダーのモーション番号
        await CountDown(1);
        _enemyController.DefaultPositionAndAnimation();
        PKFireTutorial().Forget();
    }
    
    private async UniTask PKFireTutorial()
    {
        await ChangeTutorialText("敵が疲れていたら\n両手から炎を出して攻撃しよう！");
        _enemyController.TutorialStunAnimation();
        await tutorialVideoPlayer.PlayPKFireVideo();
        // 0がPKファイヤのモーション番号
        await CountDown(0);
        _enemyController.DefaultPositionAndAnimation();
        NutoralTutorial().Forget();
    }
    
    private async UniTask NutoralTutorial()
    {
        await ChangeTutorialText("最後に攻撃をしていない棒立ちの\n状態をAIに送信しよう！");
        await UniTask.Delay(TimeSpan.FromSeconds(3f));
        //3が何もしないのモーション番号
        await CountDown(3);
        _enemyController.DefaultPositionAndAnimation();
        MakeAITime();
    }
    
    private async UniTask MakeAITime()
    {
        await ChangeTutorialText("あなたの動きから\nAIを作成しているよ！");
        //テスト送信
        sendData.SendCSVPath(motionDataInputer.filePath);
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
    public async UniTask CountDown(int motionNumber)
    {
        UniTaskCompletionSource completionSource = new UniTaskCompletionSource();

        countDownCanvasGroup.alpha = 0;
        countDownCanvasGroup.DOFade(1, 0.5f).SetLink(gameObject);
        List<float> motionData = new List<float>();

        for (int i = 0; i < 6; i++)
        {
            motionData.Add(i == motionNumber ? 1 : 0);
        }

        Observable.Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1))
            .Take(6)
            .Subscribe(
                count =>
                {
                    countDownText.text = (5 - count).ToString();
                    if (count == 2)
                    {
                        motionDataInputer.DataInputTimer(motionData); // ここでのmotionDataInputerとDataInputTimerは例です。実際のメソッド名に置き換えてください。
                    }
                    if (count == 5)
                    {
                        countDownCanvasGroup.DOFade(0, 0.5f).SetLink(gameObject).OnComplete(() => completionSource.TrySetResult());
                    }
                },
                () => completionSource.TrySetResult()) // オブザーバブルが完了したときに呼び出されます。
            .AddTo(this);

        await completionSource.Task; // ここでカウントダウンの完了を待機します。
    }
    
    public void EndTutorial()
    {
        foreach (var obj in onlyTutorialObjects)
        {
            obj.SetActive(false);
        }
        _enemyController.isTutorial = false;
        _enemyController.SelectAttack(3f);
    }
    
    public void AppearTutorialObjects()
    {
        foreach (var obj in onlyTutorialObjects)
        {
            obj.SetActive(true);
        }
    }

    private void SwitchCredit()
    {
        creditPanel.SetActive(!creditPanel.activeSelf);
    }
    
}
