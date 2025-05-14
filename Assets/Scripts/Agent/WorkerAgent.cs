using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerAgent : AgentBehaviour
{
    public GameObject Office;
    // Start is called before the first frame update
    public override Node ConfigureSequence()
    {
        Leaf goToPatron = new Leaf("Go To Patron", GoToPatron);
        Leaf goToOffice = new Leaf("Go To Patron", GoToOffice);

        Selector beWorker = new Selector("Be Worker");
        beWorker.AddChild(goToPatron);
        beWorker.AddChild(goToOffice);

        return beWorker;
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
