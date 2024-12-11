using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tank
{
    public class LockedOnState : StateMachineBehaviour
    {
        GameObject player;
        Tower tower;

        // OnStateEnter�� ���� ���̰� ���۵� �� ȣ��ǰ�
        // ���� ���� �� ���¸� ���ϱ� �����Ѵ�.
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            player = GameObject.FindWithTag("Player");
            tower = animator.gameObject.GetComponent<Tower>();
            tower.LockedOn = true;
        }

        // OnStateUpdate�� OnStateEnter�� OnStateExit �ݹ� ���̿���
        // �� Update �����ӿ� ȣ��ȴ�.
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.gameObject.transform.LookAt(player.transform);
        }

        // OnStateExit�� �������̰� ������ ���� ��谡
        // �� ���¿� ���� �򰡸� ������ �� ȣ��ȴ�.
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.gameObject.transform.rotation = Quaternion.identity;
            tower.LockedOn = false;
        }
    }
}
