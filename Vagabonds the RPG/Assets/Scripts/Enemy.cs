using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : NPC
{
    [SerializeField] private CanvasGroup healthGroup;

    private Transform target;

    public Transform Target { get => target; set => target = value; }

    protected override void Update()
    {
        FollowTarget();

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

    private void FollowTarget()
    {
        if (target!= null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}