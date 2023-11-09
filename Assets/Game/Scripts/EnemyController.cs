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

        private void Start()
        {
            _animator = GetComponent<Animator>();
            JumpAttack();
        }

        /// <summary>
        /// 炎を投げる攻撃
        /// </summary>
        private void ThrowAttack()
        {
            _animator.SetTrigger(ThrowAttackHash);
        }

        private void ThrowEffect()
        {
            Debug.Log("炎をなげる");
            Instantiate(fireBall, rightHandTransform.position, Quaternion.identity);
        }
        
        private void JumpAttack()
        {
            _animator.SetTrigger(JumpAttackHash);
            
        }
    }
}

