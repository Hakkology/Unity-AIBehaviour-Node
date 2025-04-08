using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ActionState {IDLE, WORKING};

public class RobberBehaviour : AgentBehaviour
{
    public GameObject Diamond;
    public GameObject Van;
    public GameObject BackDoor;
    public GameObject FrontDoor;

    [Range(0, 1000)]
    public int money = 800;

    public override Sequence ConfigureSequence()
    {
        Sequence steal = new Sequence("Steal Something");

        Leaf hasGotMoney = new Leaf("Has Money", HasMoney);
        Selector openDoor = new Selector("Open the Door");
        Inverter invertMoney = new Inverter("Invert Money");
        Leaf goToBackDoor = new Leaf("Go To Back Door", GoToBackDoor);
        Leaf goToFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor);

        Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);

        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackDoor);

        // steal.AddChild(hasGotMoney);
        invertMoney.AddChild(hasGotMoney);

        steal.AddChild(invertMoney);
        steal.AddChild(openDoor);
        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);

        return steal;
    }

    public NodeState GoToDiamond() => GoPickUpDiamond(Diamond);
    public NodeState GoToVan() => DeliverDiamond(Van);
    public NodeState GoToBackDoor() => GoToDoor(BackDoor);
    public NodeState GoToFrontDoor() => GoToDoor(FrontDoor);

    public NodeState HasMoney()
    {
        if (money < 500) 
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

    public NodeState DeliverDiamond(GameObject van)
    {
        NodeState state = GoToLocation(van.transform.position);
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
}
