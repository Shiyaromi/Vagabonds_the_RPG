using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Enemy parent;

    private float attackCoolDown = 3;

    private float extraRange = .1f;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {

    }

    public void Update()
    {
        if (parent.MyAttackTime >= attackCoolDown && !parent.IsAttacking)
        {
            parent.MyAttackTime = 0;

            parent.StartCoroutine(Attack());
        }

        if (parent.Target != null)
        {
            float distance = Vector2.Distance(parent.Target.position, parent.transform.position);

            if (distance >= parent.MyAttackRange + extraRange && !parent.IsAttacking) 
                parent.ChangeState(new FollowState());

            // We need to check range and attack
        }
        else
        {
            parent.ChangeState(new IdleState());
        }
    }

    public IEnumerator Attack()
    {
        parent.IsAttacking = true;

        parent.MyAnimator.SetBool("attackBool", true);

        parent.MyAnimator.SetTrigger("attackTrigger");

        yield return new WaitForSeconds(parent.MyAnimator.GetCurrentAnimatorStateInfo(2).length + 1); // FOS

        parent.IsAttacking = false;
        parent.MyAnimator.SetBool("attackBool", false);
    }
}
