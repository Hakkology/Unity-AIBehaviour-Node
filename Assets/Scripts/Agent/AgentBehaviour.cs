using UnityEngine;
using UnityEngine.AI;

public class AgentBehaviour : MonoBehaviour {
    
    protected ActionState state = ActionState.IDLE;
    protected RootNode tree;
    protected NodeState treeStatus = NodeState.RUNNING;
    protected NavMeshAgent agent;

    void Awake() 
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        tree = new RootNode();
        tree.AddChild(Behave());

        tree.Process();
        tree.PrintTree();
    }

    public virtual Sequence Behave()
    {
        return new Sequence("Default");
    }

    void Update() {
        if (treeStatus != NodeState.SUCCESS)
            treeStatus = tree.Process();
    }

    protected NodeState GoToLocation(Vector3 destination)
    {
        float distanceToTarget = Vector3.Distance(destination, this.transform.position);

        if (state == ActionState.IDLE)
        {
            agent.SetDestination(destination);
            state =ActionState.WORKING;
        }
        else if(Vector3.Distance(agent.pathEndPosition, destination) >= 2)
        {
            state = ActionState.IDLE;
            return NodeState.FAILURE;
        }
        else if (distanceToTarget < 2)
        {
            state = ActionState.IDLE;
            return NodeState.SUCCESS;
        }
        return NodeState.RUNNING;
    }
}