using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAnimationState : StateMachineBehaviour {



    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

       

        //Olha a duracao da animacao do pulo
        AnimatorClipInfo[] clips = animator.GetNextAnimatorClipInfo(layerIndex);

        if (clips.Length > 0) {
            AnimatorClipInfo jumpClipInfo = clips[0];
            
            //Olha a duracao do pulo do gameplay
            //Assumindo que o Player Controller esta no objeto pai. Resolver isso.
            PlayerController player = animator.transform.parent.GetComponent<PlayerController>();


            //Setar o JumpMultiplier para que a duracao final da animacao do pulo seja = a duracao do pulo no gameplay.
            float multiplier = jumpClipInfo.clip.length / player.JumpDuration;
            animator.SetFloat(PlayerAnimationConstants.JumpMultiplier, multiplier);
        }
    }

}
