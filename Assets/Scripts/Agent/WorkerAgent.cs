using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerAgent : AgentBehaviour
{
    public GameObject Office;
    private GameObject patron;
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
        
        if(Blackboard.Instance.Patrons.Count == 0) return NodeState.FAILURE;

        patron = Blackboard.Instance.Patrons.Pop();
        NodeState state = GoToLocation(patron.transform.position);
        
        if (state == NodeState.SUCCESS)
        {
            patron.GetComponent<PatronBehaviour>().ticket = true;
            patron = null;
        }
        return state;
    }

    public NodeState GoToOffice() => GoToLocation(Office.transform.position);

}
