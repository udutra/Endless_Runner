using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRunAnimationState : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //TODO: Remover o assumption que o player é sempre o pai do animator
        PlayerController player = animator.transform.parent.GetComponent<PlayerController>();
        if (player != null) {
            player.enabled = true;
        }
    }
}