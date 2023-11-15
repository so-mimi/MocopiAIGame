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
        private PKFire _pkFire;
        private PKThunder _pkThunder;
        public Sequence JumpTween;
        private Player _player;
        private bool _isPKFire;

        private static readonly int ThrowAttackHash = Animator.StringToHash("ThrowAttack");
        private static readonly int JumpAttackHash = Animator.StringToHash("JumpAttack");
        private static readonly int DamageHash = Animator.StringToHash("Damage");
        private static readonly int StunHash = Animator.StringToHash("Stun");
        
        private void Start()
        {
            _pkFire = FindObjectOfType<PKFire>();
            _pkThunder = FindObjectOfType<PKThunder>();
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
            int random = Random.Range(0, 2);
            if (random == 0)
            {
                ThrowAnimation();
            }
            else
            {
                JumpAnimation();
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
            Debug.Log("炎をなげる");
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
                        UniTask.Delay(2000).Forget();
                        TimeManager.Instance.SlowDownTime();
                    })).SetLink(gameObject)
                .Append(this.transform.DOMove(endValue:
                        new Vector3(0f, 0.0f, 0), duration: 1.0f).SetEase(Ease.InSine)
                    .OnComplete(() =>
                    {
                        _pkThunder.OnPKThunder -= ThunderAttack;
                        TimeManager.Instance.ResetTime();
                        SelectAttack(7f);
                    })).SetLink(gameObject)
                .Append(this.transform.DOJump(
                    new Vector3(0f, 0f, 7f), jumpPower: 2f, numJumps: 1, duration: 2f));
        }

        private void ThunderAttack()
        {
            Debug.Log("サンダー攻撃");
            _pkThunder.OnPKThunder -= ThunderAttack;
            JumpTween.Kill();
            _player.SpawnFreezeBall();
        }

        public async UniTask DamageAnimation()
        {
            _animator.SetTrigger(DamageHash);
            DamageEffect();
            await UniTask.Delay(1000);
            StunAnimation();
        }

        public void DamageEffect()
        {
            Instantiate(fireImpactPrefab, EnemyPosition.Instance.chestTransform.position, Quaternion.identity);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(enemyMaterial.DOColor(Color.white, 0.1f))
                .Append(enemyMaterial.DOColor(Color.black, 0.1f))
                .Append(enemyMaterial.DOColor(Color.white, 0.1f))
                .Append(enemyMaterial.DOColor(Color.black, 0.1f));
        }
        
        public void StunAnimation()
        {
            _animator.SetTrigger(StunHash);
            StunEvent();
        }

        public async UniTask StunEvent()
        {
            _isPKFire = false;
            await UniTask.Delay(300);
            TimeManager.Instance.SlowDownTime();
            _pkFire.OnPKFire += PlayerFireAttack;
            await UniTask.Delay(300);
            if (!_isPKFire)
            {
                _pkFire.OnPKFire -= PlayerFireAttack;
                if (TimeManager.Instance.isSlowDown)
                {
                    TimeManager.Instance.ResetTime();
                }
            }
            
            SelectAttack(5f);
        }
        
        private void PlayerFireAttack()
        {
            _isPKFire = true;
            _pkFire.OnPKFire -= PlayerFireAttack;
            TimeManager.Instance.ResetTime();
            _player.SpawnPlayerFire();
        }

        public void BackAnimation()
        {
            this.transform.DOJump(
        new Vector3(0f, 0f, 7f), jumpPower: 2f, numJumps: 1, duration: 2f);
        }
    }
}

