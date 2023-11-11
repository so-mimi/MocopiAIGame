using DG.Tweening;
using UnityEngine;

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

        private static readonly int ThrowAttackHash = Animator.StringToHash("ThrowAttack");
        private static readonly int JumpAttackHash = Animator.StringToHash("JumpAttack");
        private static readonly int DamageHash = Animator.StringToHash("Damage");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            ThrowAnimation();
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

            var jumpSequence = DOTween.Sequence(); //Sequence生成
            //Tweenをつなげる
            jumpSequence.Append(this.transform.DOMove(endValue:
                            new Vector3(0f, 5.0f, 0), duration: 2.0f).SetEase(Ease.InOutQuad)
                                .OnComplete(TimeManager.Instance.SlowDownTime)).SetLink(gameObject)
                        .Append(this.transform.DOMove(endValue:
                            new Vector3(0f, 0.0f, 0), duration: 1.0f).SetEase(Ease.InSine)
                                .OnComplete(TimeManager.Instance.ResetTime)).SetLink(gameObject);
        }

        private void DamageAnimation()
        {
            _animator.SetTrigger(DamageHash);
        }
    }
}

