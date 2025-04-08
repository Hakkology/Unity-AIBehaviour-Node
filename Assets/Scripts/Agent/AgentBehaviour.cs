using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AgentBehaviour : MonoBehaviour {
    
    protected ActionState state = ActionState.IDLE;
    protected RootNode tree;
    protected NodeState treeStatus = NodeState.RUNNING;
    protected NavMeshAgent agent;

    WaitForSeconds NodeStateCheck;

    void Awake() 
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        tree = new RootNode();
        tree.AddChild(ConfigureSequence());

        tree.Process();
        tree.PrintTree();

        NodeStateCheck = new WaitForSeconds(Random.Range(0.1F, 0.5F));
        StartCoroutine(Behave());
    }

    public virtual Sequence ConfigureSequence()
    {
        return new Sequence("Default");
    }

    IEnumerator Behave()
    {
        while (true)
        {
            treeStatus = tree.Process();
            yield return NodeStateCheck;
        }
    }

    // void Update() {
    //     if (treeStatus != NodeState.SUCCESS)
    //         treeStatus = tree.Process();
    // }

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