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
        Leaf allocatePatron = new Leaf("Allocate Patron", AllocatePatron);
        Leaf goToOffice = new Leaf("Go To Patron", GoToOffice);
        Leaf patronStillWaiting = new Leaf("Is Patron Waiting?", PatronWaiting);

        Sequence getPatron = new Sequence("Find a Patron");
        getPatron.AddChild(allocatePatron);

        RootNode waiting = new RootNode();
        waiting.AddChild(patronStillWaiting);
        DependencySequence moveToPatron = new DependencySequence("Moving To Patron", waiting, agent);
        moveToPatron.AddChild(goToPatron);

        getPatron.AddChild(moveToPatron);

        Selector beWorker = new Selector("Be Worker");
        beWorker.AddChild(getPatron);
        beWorker.AddChild(goToOffice);

        return beWorker;
    }

    public NodeState PatronWaiting()
    {
        if (patron == null) return NodeState.FAILURE;
        if (patron.GetComponent<PatronBehaviour>().isWaiting)
            return NodeState.SUCCESS;
        return NodeState.FAILURE;
    }

    public NodeState GoToPatron()
    {

        if (patron == null) return NodeState.FAILURE;
        NodeState state = GoToLocation(patron.transform.position);

        if (state == NodeState.SUCCESS)
        {
            patron.GetComponent<PatronBehaviour>().ticket = true;
            patron = null;
        }
        return state;
    }

    public NodeState AllocatePatron()
    {
        if (Blackboard.Instance.Patrons.Count == 0) return NodeState.FAILURE;
        patron = Blackboard.Instance.Patrons.Pop();
        
        if (patron == null) return NodeState.FAILURE;
        return NodeState.SUCCESS;
    }

    public NodeState GoToOffice() => GoToLocation(Office.transform.position);

}
