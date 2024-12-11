using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tank
{
    public class LockedOnState : StateMachineBehaviour
    {
        GameObject player;
        Tower tower;

        // OnStateEnter는 상태 전이가 시작될 때 호출되고
        // 상태 기계는 이 상태를 평가하기 시작한다.
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            player = GameObject.FindWithTag("Player");
            tower = animator.gameObject.GetComponent<Tower>();
            tower.LockedOn = true;
        }

        // OnStateUpdate는 OnStateEnter와 OnStateExit 콜백 사이에서
        // 매 Update 프레임에 호출된다.
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.gameObject.transform.LookAt(player.transform);
        }

        // OnStateExit는 상태전이가 끝나고 상태 기계가
        // 이 상태에 대한 평가를 마쳤을 때 호출된다.
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.gameObject.transform.rotation = Quaternion.identity;
            tower.LockedOn = false;
        }
    }
}
