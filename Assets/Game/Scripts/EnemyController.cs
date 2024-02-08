using System;
using DG.Tweening;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace MocopiAIGame
{
    /// <summary>
    /// 敵の状態を管理するクラス
    /// </summary>
    [RequireComponent(typeof(Animator))]
    internal class EnemyController : MonoBehaviour
    {
        private Animator _animator;
        [SerializeField] private GameObject fireBall;
        [SerializeField] private Transform rightHandTransform;
        [SerializeField] private Material enemyMaterial;
        [SerializeField] private GameObject fireImpactPrefab;
        public EnemyHP enemyHP;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip damageClip;
        [SerializeField] private ResetGame resetGame;
        private PKFire _pkFire;
        private PKThunder _pkThunder;
        private Punch _punch;
        private Kick _kick;
        public Sequence JumpTween;
        private Player _player;
        private bool _isPKFire;
        private bool _isPunch;
        [FormerlySerializedAs("_isTutorial")] public bool isTutorial;

        private static readonly int ThrowAttackHash = Animator.StringToHash("ThrowAttack");
        private static readonly int JumpAttackHash = Animator.StringToHash("JumpAttack");
        private static readonly int DamageHash = Animator.StringToHash("Damage");
        private static readonly int StunHash = Animator.StringToHash("Stun");
        private static readonly int TutorialStunHash = Animator.StringToHash("TutorialStun");
        
        public Subject<Unit> OnPunch = new Subject<Unit>();
        public Subject<Unit> OnTwoHandAttack = new Subject<Unit>();
        public Subject<Unit> OnThrow = new Subject<Unit>();
        public Subject<Unit> OnJump = new Subject<Unit>();
        private void Start()
        {
            _pkFire = FindObjectOfType<PKFire>();
            _pkThunder = FindObjectOfType<PKThunder>();
            _punch = FindObjectOfType<Punch>();
            _kick = FindObjectOfType<Kick>();
            _animator = GetComponent<Animator>();
            _player = FindObjectOfType<Player>();
        }
        
        /// <summary>
        /// 敵の攻撃セレクト
        /// この関数を呼び出すことで、ゲームの根幹となる敵の攻撃を選択する
        /// </summary>
        /// <param name="waitTimeSec"></param>
        public async UniTask SelectAttack(float waitTimeSec)
        {
            await UniTask.Delay((int) (waitTimeSec * 1000));
            int random = Random.Range(0, 4);
            if (random == 0)
            {
                ThrowAnimation();
            }
            else if (random == 1)
            {
                WalkAnimation();
            }
            else if (random == 2)
            {
                JumpAnimation();
            }
            else if (random == 3)
            {
                WalkAnimationTwo();
            }
        }

        /// <summary>
        /// 炎を投げる攻撃の最初の関数
        /// </summary>
        public void ThrowAnimation()
        {
            _animator.SetTrigger(ThrowAttackHash);
        }

        private async UniTask ThrowEffect()
        {
            GameObject fire = Instantiate(fireBall, rightHandTransform.position, Quaternion.identity);
            FireBallAnimation fireBallAnimation = fire.GetComponent<FireBallAnimation>();
            FirePool.Instance.fireBallAnimations.Add(fireBallAnimation);
            
            if (!isTutorial) return;
            
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            _animator.speed = 0.01f;
            OnThrow.OnNext(Unit.Default);
        }
        
        public void JumpAnimation()
        {
            _animator.SetTrigger(JumpAttackHash);
        }

        private void JumpMove()
        {
            if (isTutorial)
            {
                TutorialJump();
                return;
            }
            
            //TODO: VRPlayerの位置を取得して、そこに向かってジャンプする
            JumpTween = DOTween.Sequence(); //Sequence生成
            JumpTween.Append(this.transform.DOMove(endValue:
                        new Vector3(0f, 2.5f, 3.0f), duration: 2.0f).SetEase(Ease.InOutQuad)
                    .OnComplete(() =>
                    {
                        _pkThunder.OnPKThunder += ThunderAttack;
                        UniTask.Delay(2000);
                        TimeManager.Instance.SlowDownTime();
                    })).SetLink(gameObject)
                .Append(this.transform.DOMove(endValue:
                        Camera.main.transform.position, duration: 1.0f).SetEase(Ease.InSine)
                    .OnComplete(() =>
                    {
                        _pkThunder.OnPKThunder -= ThunderAttack;
                        TimeManager.Instance.ResetTime();
                        _player.PlayerDamageEffect();
                        SelectAttack(7f);
                    })).SetLink(gameObject)
                .Append(this.transform.DOJump(
                    new Vector3(0f, 0f, 7f), jumpPower: 2f, numJumps: 1, duration: 2f));
        }

        private void TutorialJump()
        {
            this.transform.DOMove(endValue:
                new Vector3(0f, 2.5f, 3.0f), duration: 2.0f).SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    OnJump.OnNext(Unit.Default);
                    _animator.speed = 0f;
                });
        }

        private void ThunderAttack()
        {
            _pkThunder.OnPKThunder -= ThunderAttack;
            JumpTween.Kill();
            _player.SpawnFreezeBall();
        }

        public void WalkAnimation()
        {
            _animator.SetTrigger("Walk");
            WalkMove();
        }

        public void WalkMove()
        {
            transform.DOMove(new Vector3(0f, 0f, 3f), 1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                PunchAnimation();
            });
        }
        
        public void PunchAnimation()
        {
            _animator.SetBool("Punch", true);
        }
        
        public async void PunchEvent()
        {
            if (isTutorial)
            {
                await TutorialPunch();
                return;
            }
            _isPunch = false;
            TimeManager.Instance.SlowDownTime();
            _animator.speed = 0.25f;
            _punch.OnPunch += PlayerPunchAttack;
            await UniTask.Delay(1000);
            if (!_isPunch)
            {
                _animator.SetBool("Punch", false);
                _punch.OnPunch -= PlayerPunchAttack;
                _animator.speed = 1f;
                TimeManager.Instance.ResetTime();
                _player.PlayerDamageEffect();
                MoveDefaultPosition();
                SelectAttack(5f);
            }
        }

        /// <summary>
        /// チュートリアル用のパンチイベント
        /// </summary>
        public async UniTask TutorialPunch()
        {
            _animator.speed = 0.01f;
            await UniTask.Delay(1000);
            _animator.speed = 0f;
            OnPunch.OnNext(Unit.Default);
        }
        
        /// <summary>
        /// キャラクターを初期位置、初期アニメーションに戻す
        /// </summary>
        public void DefaultPositionAndAnimation()
        {
            transform.DOMove(new Vector3(0f, 0f, 7f), 1f).SetEase(Ease.Linear);
            _animator.SetBool("Punch", false);
            _animator.SetBool("TwoHandAttack", false);
            _animator.SetBool(TutorialStunHash, false);
            _animator.speed = 1f;
        }
        
        private void PlayerPunchAttack()
        {
            _isPunch = true;
            _animator.SetBool("Punch", false);
            _punch.OnPunch -= PlayerPunchAttack;
            _animator.speed = 1f;
            TimeManager.Instance.ResetTime();
            DamageAnimation();
            MoveDefaultPosition();
        }
        
        private void MoveDefaultPosition()
        {
            transform.DOMove(new Vector3(0f, 0f, 7f), 1f).SetEase(Ease.Linear);
        }
        
        /// <summary>
        /// 両手攻撃の開始の関数
        /// </summary>
        public void WalkAnimationTwo()
        {
            _animator.SetTrigger("Walk");
            WalkMoveTwo();
        }
        
        private void WalkMoveTwo()
        {
            transform.DOMove(new Vector3(0f, 0f, 3f), 1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                TwoHandAnimation();
            });
        }
        
        public void TwoHandAnimation()
        {
            _animator.SetBool("TwoHandAttack", true);
        }
        
        public async void TwoHandEvent()
        {
            if (isTutorial)
            {
                await TutorialTwoHandAttack();
                return;
            }
            _isPunch = false;
            TimeManager.Instance.SlowDownTime();
            _animator.speed = 0.25f;
            _kick.OnKick += PlayerKickAttack;
            await UniTask.Delay(1000);
            if (!_isPunch)
            {
                _animator.SetBool("TwoHandAttack", false);
                _kick.OnKick -= PlayerKickAttack;
                _animator.speed = 1f;
                TimeManager.Instance.ResetTime();
                _player.PlayerDamageEffect();
                MoveDefaultPosition();
                SelectAttack(5f);
            }
        }
        
        private async UniTask TutorialTwoHandAttack()
        {
            _animator.speed = 0.1f;
            await UniTask.Delay(1000);
            _animator.speed = 0f;
            OnTwoHandAttack.OnNext(Unit.Default);
        }
        
        private void PlayerKickAttack()
        {
            _isPunch = true;
            _animator.SetBool("TwoHandAttack", false);
            _kick.OnKick -= PlayerKickAttack;
            _animator.speed = 1f;
            TimeManager.Instance.ResetTime();
            DamageAnimation();
            MoveDefaultPosition();
        }

        public async UniTask DamageAnimation()
        {
            _animator.SetTrigger(DamageHash);
            DamageEffect();
            await UniTask.Delay(1000);
            StunAnimation();
        }

        public async void DamageEffect()
        {
            Instantiate(fireImpactPrefab, EnemyPosition.Instance.chestTransform.position, Quaternion.identity);
            TimeManager.Instance.StopTime();
            Sequence sequence = DOTween.Sequence();
            sequence.Append(enemyMaterial.DOColor(Color.white, 0.1f))
                .Append(enemyMaterial.DOColor(Color.black, 0.1f))
                .Append(enemyMaterial.DOColor(Color.white, 0.1f))
                .Append(enemyMaterial.DOColor(Color.black, 0.1f));
            await UniTask.Delay(10);
            enemyHP.Damage(20f);
            audioSource.PlayOneShot(damageClip);
            TimeManager.Instance.ResetTime();
        }
        
        public void StunAnimation()
        {
            _animator.SetTrigger(StunHash);
        }
        
        public void TutorialStunAnimation()
        {
            _animator.SetBool(TutorialStunHash, true);
        }

        public async UniTask StunEvent()
        {
            if (isTutorial)
            {
                return;
            }
            
            _isPKFire = false;
            _pkFire.OnPKFire += PlayerFireAttack;
            await UniTask.Delay(3800);
            if (!_isPKFire)
            {
                _pkFire.OnPKFire -= PlayerFireAttack;
            }
            
            SelectAttack(5f);
        }
        
        private void PlayerFireAttack()
        {
            _isPKFire = true;
            _pkFire.OnPKFire -= PlayerFireAttack;
            _player.SpawnPlayerFire();
        }

        public void BackAnimation()
        {
            this.transform.DOJump(
        new Vector3(0f, 0f, 7f), jumpPower: 2f, numJumps: 1, duration: 2f);
        }
        
        public async void DeathAnimation()
        {
            _animator.SetTrigger("Death");
            await UniTask.Delay(5000);
            gameObject.SetActive(false);
            resetGame.AppearedEndPanel();
        }
    }
}

