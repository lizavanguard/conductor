using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.View
{
    public class ActorViewBase : MonoBehaviour
    {
        Animator animator;

        void Awake()
        {
            animator = gameObject.GetComponent<Animator>();
        }

        public void UpdateRotationByDirection(Vector3 direction)
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.forward, direction);
        }

        public void PlayAttackAnimation()
        {
            animator.SetTrigger("Attack");
        }

        public void PlayIdleAnimation()
        {
        }
    }
}
