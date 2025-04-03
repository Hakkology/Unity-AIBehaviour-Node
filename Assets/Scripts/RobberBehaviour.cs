using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ActionState {IDLE, WORKING};

public class RobberBehaviour : MonoBehaviour
{
    public GameObject Diamond;
    public GameObject Van;
    public GameObject Door;


    ActionState state = ActionState.IDLE;
    RootNode tree;
    NodeState treeStatus = NodeState.RUNNING;
    NavMeshAgent agent;

    void Awake() 
    {
        agent = GetComponent<NavMeshAgent>();
    }
    
    void Start()
    {
        tree = new RootNode();
        Sequence steal = new Sequence("Steal Something");

        Leaf goToDoor = new Leaf("Go To The Door", GoToDoor);
        Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);

        steal.AddChild(goToDoor);
        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);
        tree.AddChild(steal);
        tree.Process();

        // Node eat = new Node("Eat Something");
        // Node pizza = new Node("Go To Pizza Shop");
        // Node buy = new Node("Buy Pizza");

        // eat.AddChild(pizza);
        // eat.AddChild(buy);
        // tree.AddChild(eat);

        tree.PrintTree();
    }

    void Update() {
        if (treeStatus == NodeState.RUNNING)
            treeStatus = tree.Process();
    }

    public NodeState GoToDiamond() => GoToLocation(Diamond.transform.position);
    public NodeState GoToVan() => GoToLocation(Van.transform.position);
    public NodeState GoToDoor() => GoToLocation(Door.transform.position);
    NodeState GoToLocation(Vector3 destination)
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
