using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerAgent : AgentBehaviour
{
    public GameObject Office;
    // Start is called before the first frame update
    public override Node ConfigureSequence()
    {
        return base.ConfigureSequence();
    }

    public NodeState GoToPatron(){
        if(Blackboard.Instance.patron == null) return NodeState.FAILURE;
        NodeState state = GoToLocation(Blackboard.Instance.patron.transform.position);
        if(state == NodeState.SUCCESS){
            Blackboard.Instance.patron.GetComponent<PatronBehaviour>().ticket = true;
            Blackboard.Instance.DeregisterPatron();
        }
        return state;
    }

    public NodeState GoToOffice() => GoToLocation(Office.transform.position);

}
