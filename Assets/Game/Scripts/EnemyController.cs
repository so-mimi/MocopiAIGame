using DG.Tweening;
using UnityEngine;
using Cysharp.Threading.Tasks;

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
        [SerializeField] private EnemyHP enemyHP;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip damageClip;
        private PKFire _pkFire;
        private PKThunder _pkThunder;
        private Punch _punch;
        private Kick _kick;
        public Sequence JumpTween;
        private Player _player;
        private bool _isPKFire;
        private bool _isPunch;

        private static readonly int ThrowAttackHash = Animator.StringToHash("ThrowAttack");
        private static readonly int JumpAttackHash = Animator.StringToHash("JumpAttack");
        private static readonly int DamageHash = Animator.StringToHash("Damage");
        private static readonly int StunHash = Animator.StringToHash("Stun");
        
        private void Start()
        {
            _pkFire = FindObjectOfType<PKFire>();
            _pkThunder = FindObjectOfType<PKThunder>();
            _punch = FindObjectOfType<Punch>();
            _kick = FindObjectOfType<Kick>();
            _animator = GetComponent<Animator>();
            _player = FindObjectOfType<Player>();
            SelectAttack(3f);
            
        }
        
        /// <summary>
        /// 敵の攻撃セレクト
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
        /// 炎を投げる攻撃
        /// </summary>
        private void ThrowAnimation()
        {
            _animator.SetTrigger(ThrowAttackHash);
        }

        private void ThrowEffect()
        {
            GameObject fire = Instantiate(fireBall, rightHandTransform.position, Quaternion.identity);
            FireBallAnimation fireBallAnimation = fire.GetComponent<FireBallAnimation>();
            FirePool.Instance.fireBallAnimations.Add(fireBallAnimation);
        }
        
        private void JumpAnimation()
        {
            _animator.SetTrigger(JumpAttackHash);
        }

        private void JumpMove()
        {
            //TODO: VRPlayerの位置を取得して、そこに向かってジャンプする
            JumpTween = DOTween.Sequence(); //Sequence生成
            JumpTween.Append(this.transform.DOMove(endValue:
                        new Vector3(0f, 5.0f, 0), duration: 2.0f).SetEase(Ease.InOutQuad)
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
        
        private void WalkAnimationTwo()
        {
            _animator.SetTrigger("Walk");
            WalkMoveTwo();
        }
        
        public void WalkMoveTwo()
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

        public async UniTask StunEvent()
        {
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
        }
    }
}

