using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : NPC
{
    [SerializeField] private CanvasGroup healthGroup;

    private Transform target;

    private IState currentState;

    public float MyAttackRange { get; set; }

    public float MyAttackTime { get; set; }

    public Transform Target { get => target; set => target = value; }

    protected override void Awake()
    {
        MyAttackRange = 1.5f;

        ChangeState(new IdleState());

        base.Awake();
    }

    protected override void Update()
    {
        if (!IsAttacking)
        {
            MyAttackTime += Time.deltaTime;
        }

        currentState.Update();

        base.Update();
    }

    public override Transform Select()
    {
        healthGroup.alpha = 1;

        return base.Select();
    }

    public override void DeSelect()
    {
        healthGroup.alpha = 0;

        base.DeSelect();
    }
	
	public override void TakeDamage(float damage)
	{
		base.TakeDamage(damage);
		
		OnHealthChanged(health.MyCurrentValue);
	}

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter(this);
    }
}
