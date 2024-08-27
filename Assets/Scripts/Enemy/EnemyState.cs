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
    protected bool triggerCalled;

    public EnemyState(Enemy _baseEnemy, EnemyStateMachinde _stateMachinde, string _animBoolName)
    {
        this.baseEnemy = _baseEnemy;
        this.stateMachinde = _stateMachinde;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter() 
    {
        triggerCalled = false;
        rb = baseEnemy.rb;
        baseEnemy.anim.SetBool(animBoolName, true);
    }
    public virtual void Exit()
    {
        baseEnemy.anim.SetBool(animBoolName, false);
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
