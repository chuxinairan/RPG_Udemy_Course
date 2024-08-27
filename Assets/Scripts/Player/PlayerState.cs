using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected float xInput;
    protected float yInput;

    protected Rigidbody2D rb;

    protected Player player;
    protected PlayerStateMachine stateMachine;
    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;
    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter() 
    {
        rb = player.GetComponent<Rigidbody2D>();
        player.anim.SetBool(animBoolName, true);
        triggerCalled = false;
        Debug.Log("Player enter in " + this.GetType());
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        stateTimer -= Time.deltaTime;
        player.anim.SetFloat("yValue", rb.velocity.y);
    }
    

    public void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
