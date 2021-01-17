using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class IdleState : IState
{
    private Enemy parent;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {

    }

    public void Update()
    {
        // TODO: Change the follow state if the player is close
        if (parent.Target != null) parent.ChangeState(new FollowState()); // If we have a target, then we need to follow it
    }
}