using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum ActionState {IDLE, WORKING};

public class AgentBehaviour : MonoBehaviour
{

    protected ActionState state = ActionState.IDLE;
    protected RootNode tree;
    protected NodeState treeStatus = NodeState.RUNNING;
    protected NavMeshAgent agent;

    WaitForSeconds NodeStateCheck;
    Vector3 rememberedLocation;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        tree = new RootNode();
        tree.AddChild(ConfigureSequence());

        tree.Process();
        tree.PrintTree();

        NodeStateCheck = new WaitForSeconds(Random.Range(0.1F, 0.5F));
        StartCoroutine(Behave());
    }

    public virtual Node ConfigureSequence()
    {
        return new Node("Default");
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
            state = ActionState.WORKING;
        }
        else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2)
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

    public NodeState CanSee(Vector3 target, string tag, float distance, float maxAngle)
    {
        Vector3 directionToTarget = target - this.transform.position;
        float angle = Vector3.Angle(directionToTarget, this.transform.forward);

        if (angle <= maxAngle && directionToTarget.magnitude <= distance)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(this.transform.position, directionToTarget, out hitInfo))
            {
                if (hitInfo.collider.gameObject.CompareTag(tag))
                {
                    return NodeState.SUCCESS;
                }
            }
        }
        return NodeState.FAILURE;
    }

    public NodeState Flee(Vector3 locationOfFear, float distanceFear)
    {
        if (state == ActionState.IDLE)
        {
            rememberedLocation = this.transform.position + (transform.position - locationOfFear).normalized * distanceFear;
        }
        return GoToLocation(rememberedLocation);
    }

    public NodeState GoToDoor(GameObject door)
    {
        NodeState state = GoToLocation(door.transform.position);
        if (state == NodeState.SUCCESS)
        {
            if (!door.GetComponent<Lock>().isLocked)
            {
                door.GetComponent<NavMeshObstacle>().enabled = false;
                return NodeState.SUCCESS;
            }

            return NodeState.FAILURE;
        }
        else
        {
            return state;
        }
    }
    
    public NodeState IsOpen()
    {
        if (Blackboard.Instance.timeOfDay < Blackboard.Instance.openTime || Blackboard.Instance.timeOfDay > Blackboard.Instance.closeTime)
            return NodeState.FAILURE;
        else
            return NodeState.SUCCESS;
    }

}