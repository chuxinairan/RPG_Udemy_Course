using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneAnimationTriggers : MonoBehaviour
{
    private Clone_Skill_Controller clone => GetComponentInParent<Clone_Skill_Controller>();

    public void AnimationFinishedTrigger() => clone.AnimationFinishTrigger();

    public void AttackTrigger() => clone.AttackTrigger();
}
