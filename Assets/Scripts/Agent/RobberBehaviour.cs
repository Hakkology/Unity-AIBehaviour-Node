using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : AgentBehaviour
{
    public GameObject Diamond;
    public GameObject MonaLisaPainting;
    public GameObject Van;
    public GameObject BackDoor;
    public GameObject FrontDoor;

    private GameObject currentObject;

    [Range(0, 1000)]
    public int money = 800;

    public override Sequence ConfigureSequence()
    {
        Sequence steal = new Sequence("Steal Something");

        Leaf hasGotMoney = new Leaf("Has Money", HasMoney);
        Selector openDoor = new Selector("Open the Door");
        Selector stealSomething = new Selector("Steal Something");
        Inverter invertMoney = new Inverter("Invert Money");
        Leaf goToBackDoor = new Leaf("Go To Back Door", GoToBackDoor);
        Leaf goToFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor);

        Leaf goToMonaLisaPainting = new Leaf("Go To Mona Lisa Painting", GoToMonaLisaPainting);
        Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);

        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackDoor);

        stealSomething.AddChild(goToDiamond);
        stealSomething.AddChild(goToMonaLisaPainting);

        invertMoney.AddChild(hasGotMoney);

        steal.AddChild(invertMoney);
        steal.AddChild(openDoor);
        steal.AddChild(stealSomething);
        steal.AddChild(goToVan);

        return steal;
    }

    public NodeState GoToDiamond() => GoPickUpDiamond();
    public NodeState GoToMonaLisaPainting() => GoPickUpMonaLisaPainting();
    public NodeState GoToVan() => DeliverGoods();
    public NodeState GoToBackDoor() => GoToDoor(BackDoor);
    public NodeState GoToFrontDoor() => GoToDoor(FrontDoor);

    public NodeState HasMoney()
    {
        if (money < 500) 
            return NodeState.FAILURE;
        return NodeState.SUCCESS;
    }

    private NodeState GoPickUpMonaLisaPainting()
    {
        if (!MonaLisaPainting.activeSelf) return NodeState.FAILURE;
        NodeState state = GoToLocation(MonaLisaPainting.transform.position);
        if (state == NodeState.SUCCESS)
        {
            currentObject = MonaLisaPainting;
            MonaLisaPainting.transform.parent = this.gameObject.transform;
        }
        
        return state;
    }

    public NodeState GoPickUpDiamond()
    {
        if (!Diamond.activeSelf) return NodeState.FAILURE;
        NodeState state = GoToLocation(Diamond.transform.position);
        if (state == NodeState.SUCCESS)
        {
            currentObject = Diamond;
            Diamond.transform.parent = this.gameObject.transform;
        }
        
        return state;
    }

    public NodeState DeliverGoods()
    {
        NodeState state = GoToLocation(Van.transform.position);
        if (state == NodeState.SUCCESS)
        {
            currentObject.SetActive(false);
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
}
