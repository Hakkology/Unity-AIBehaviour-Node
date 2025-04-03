using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ActionState {IDLE, WORKING};

public class RobberBehaviour : MonoBehaviour
{
    public GameObject Diamond;
    public GameObject Van;
    public GameObject BackDoor;
    public GameObject FrontDoor;

    [Range(0, 1000)]
    public int money = 800;


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

        Leaf hasGotMoney = new Leaf("Has Money", HasMoney);
        Selector openDoor = new Selector("Open the Door");
        Leaf goToBackDoor = new Leaf("Go To Back Door", GoToBackDoor);
        Leaf goToFrontDoor = new Leaf("Go To Back Door", GoToFrontDoor);

        Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);

        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackDoor);

        steal.AddChild(hasGotMoney);
        steal.AddChild(openDoor);
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
        if (treeStatus != NodeState.SUCCESS)
            treeStatus = tree.Process();
    }

    public NodeState GoToDiamond() => GoPickUpDiamond(Diamond);
    public NodeState GoToVan() => DeliverDiamond(Van);
    public NodeState GoToBackDoor() => GoToDoor(BackDoor);
    public NodeState GoToFrontDoor() => GoToDoor(FrontDoor);

    public NodeState HasMoney()
    {
        if (money >= 500) 
            return NodeState.FAILURE;
        return NodeState.SUCCESS;
    }
    public NodeState GoPickUpDiamond(GameObject Diamond)
    {
        NodeState state = GoToLocation(Diamond.transform.position);
        if (state == NodeState.SUCCESS)
        {
            Diamond.transform.parent = this.gameObject.transform;
        }
        
        return state;
    }

    public NodeState DeliverDiamond(GameObject Diamond)
    {
        NodeState state = GoToLocation(Van.transform.position);
        if (state == NodeState.SUCCESS)
        {
            Diamond.SetActive(false);
            money += 300;
        }
        
        return state;
    }

    public NodeState GoToDoor(GameObject door)
    {
        NodeState state = GoToLocation(door.transform.position);
        if (state == NodeState.SUCCESS)
        {
            if (!door.GetComponent<Lock>().isLocked)
            {
                door.SetActive(false);
                return NodeState.SUCCESS;
            }

            return NodeState.FAILURE;
        }
        else
        {
            return state;
        }
    }

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
