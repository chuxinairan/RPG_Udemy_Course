using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected Rigidbody2D rb;

    protected EnemyStateMachinde stateMachinde;
    protected Enemy baseEnemy;

    private string animBoolName;

    protected float stateTimer;
    protected bool animationFinishedTrigger;

    public EnemyState(Enemy _baseEnemy, EnemyStateMachinde _stateMachinde, string _animBoolName)
    {
        this.baseEnemy = _baseEnemy;
        this.stateMachinde = _stateMachinde;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter() 
    {
        animationFinishedTrigger = false;
        rb = baseEnemy.rb;
        baseEnemy.anim.SetBool(animBoolName, true);
        Debug.Log(baseEnemy.gameObject.name + " enter in " + this.GetType());
    }
    public virtual void Exit()
    {
        baseEnemy.anim.SetBool(animBoolName, false);
        baseEnemy.SetLastAnimBoolName(animBoolName);
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public void AnimationFinishTrigger()
    {
        animationFinishedTrigger = true;
    }
}
