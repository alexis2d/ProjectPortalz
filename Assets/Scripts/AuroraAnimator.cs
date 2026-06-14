using System;
using UnityEngine;

namespace cowsins2D
{

    public class AuroraAnimator : PlayerAnimator
    {
        protected override void ChangeAnimationState(string newState)
        {
            if (currentState == newState) return;

            if(!String.IsNullOrEmpty(currentState)) animator?.ResetTrigger(currentState);
            animator?.Play(newState);
            
            currentState = newState;
        }
    }
}