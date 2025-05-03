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
    // public GameObject Gothic;

    public GameObject[] art;

    Leaf goToBackDoor;
    Leaf goToFrontDoor;

    private GameObject currentObject;

    [Range(0, 1000)]
    public int money = 800;

    public override Sequence ConfigureSequence()
    {
        Sequence steal = new Sequence("Steal Something");

        Leaf hasGotMoney = new Leaf("Has Money", HasMoney);
        PrioritySelector openDoor = new PrioritySelector("Open the Door");
        RandomSelector stealSomething = new RandomSelector("Steal Something");
        Inverter invertMoney = new Inverter("Invert Money");
        goToBackDoor = new Leaf("Go To Back Door", GoToBackDoor, 2);
        goToFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor, 1);

        // Leaf goToMonaLisaPainting = new Leaf("Go To Mona Lisa Painting", GoToMonaLisaPainting,1);
        // Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond, 2);
        Leaf goToArt1 = new Leaf("GoToArt1", GoToArt1);
        Leaf goToArt2 = new Leaf("GoToArt2", GoToArt2);
        Leaf goToArt3 = new Leaf("GoToArt3", GoToArt3);
        Leaf goToArt4 = new Leaf("GoToArt4", GoToArt4);
        Leaf goToArt5 = new Leaf("GoToArt5", GoToArt5);
        Leaf goToArt6 = new Leaf("GoToArt6", GoToArt6);
        Leaf goToArt7 = new Leaf("GoToArt7", GoToArt7);
        
        Leaf goToVan = new Leaf("Go To Van", GoToVan);

        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackDoor);

        // stealSomething.AddChild(goToDiamond);
        // stealSomething.AddChild(goToMonaLisaPainting);
        stealSomething.AddChild(goToArt1);
        stealSomething.AddChild(goToArt2);
        stealSomething.AddChild(goToArt3);
        stealSomething.AddChild(goToArt4);
        stealSomething.AddChild(goToArt5);
        stealSomething.AddChild(goToArt6);
        stealSomething.AddChild(goToArt7);

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
    public NodeState GoToBackDoor()
    {
        NodeState resultState = GoToDoor(BackDoor);
        if (resultState == NodeState.FAILURE)
            goToBackDoor.sortOrder = 10;
        else
            goToBackDoor.sortOrder = 1;
        return resultState;
    }

    public NodeState GoToFrontDoor()
    {
        NodeState resultState = GoToDoor(FrontDoor);
        if (resultState == NodeState.FAILURE)
            goToFrontDoor.sortOrder = 10;
        else
            goToFrontDoor.sortOrder = 1;
        return resultState;
    }

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

    private NodeState GoToArt1()
    {
        if (!art[0].activeSelf) return NodeState.FAILURE;
        NodeState state = GoToLocation(art[0].transform.position);
        if (state == NodeState.SUCCESS)
        {
            currentObject = art[0];
            art[0].transform.parent = this.gameObject.transform;
        }
        
        return state;
    }

    private NodeState GoToArt2()
    {
        if (!art[1].activeSelf) return NodeState.FAILURE;
        NodeState state = GoToLocation(art[1].transform.position);
        if (state == NodeState.SUCCESS)
        {
            currentObject = art[1];
            art[1].transform.parent = this.gameObject.transform;
        }
        
        return state;
    }

    private NodeState GoToArt3()
    {
        if (!art[2].activeSelf) return NodeState.FAILURE;
        NodeState state = GoToLocation(art[2].transform.position);
        if (state == NodeState.SUCCESS)
        {
            currentObject = art[2];
            art[2].transform.parent = this.gameObject.transform;
        }
        
        return state;
    }

    private NodeState GoToArt4()
    {
        if (!art[3].activeSelf) return NodeState.FAILURE;
        NodeState state = GoToLocation(art[3].transform.position);
        if (state == NodeState.SUCCESS)
        {
            currentObject = art[3];
            art[3].transform.parent = this.gameObject.transform;
        }
        
        return state;
    }
    private NodeState GoToArt5()
    {
        if (!art[4].activeSelf) return NodeState.FAILURE;
        NodeState state = GoToLocation(art[4].transform.position);
        if (state == NodeState.SUCCESS)
        {
            currentObject = art[4];
            art[4].transform.parent = this.gameObject.transform;
        }
        
        return state;
    }

    private NodeState GoToArt6()
    {
        if (!art[5].activeSelf) return NodeState.FAILURE;
        NodeState state = GoToLocation(art[5].transform.position);
        if (state == NodeState.SUCCESS)
        {
            currentObject = art[5];
            art[5].transform.parent = this.gameObject.transform;
        }
        
        return state;
    }

    private NodeState GoToArt7()
    {
        if (!art[6].activeSelf) return NodeState.FAILURE;
        NodeState state = GoToLocation(art[6].transform.position);
        if (state == NodeState.SUCCESS)
        {
            currentObject = art[6];
            art[6].transform.parent = this.gameObject.transform;
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
