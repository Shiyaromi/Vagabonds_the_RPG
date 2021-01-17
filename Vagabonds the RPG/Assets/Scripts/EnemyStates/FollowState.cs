using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class FollowState : IState
{
    private Enemy parent;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
    }

    public void Update()
    {
        if (parent.Target != null)
        {
            // TODO: if we out of the target range, the enemy will goes our last known position and if it still cant find us, it will return its own position.
            parent.Direction = (parent.Target.transform.position - parent.transform.position).normalized;

            parent.transform.position = Vector2.MoveTowards(parent.transform.position, parent.Target.position, parent.Speed * Time.deltaTime);

            float distance = Vector2.Distance(parent.Target.position, parent.transform.position);

            if (distance <= parent.MyAttackRange)
            {
                parent.ChangeState(new AttackState());
            }
        }
        else
        {
            parent.ChangeState(new IdleState());
        }
    }
}
