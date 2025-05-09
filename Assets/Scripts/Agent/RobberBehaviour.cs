using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : AgentBehaviour
{
    // public GameObject Diamond;
    // public GameObject MonaLisaPainting;
    public GameObject Van;
    public GameObject BackDoor;
    public GameObject FrontDoor;
    public GameObject CopAgent;
    // public GameObject Gothic;

    public GameObject[] art;

    Leaf goToBackDoor;
    Leaf goToFrontDoor;

    private GameObject currentObject;

    [Range(0, 1000)]
    public int money = 800;

    public override Node ConfigureSequence()
    {

        // s4
        Leaf goToVan = new Leaf("Go To Van", GoToVan);

        // s3
        RandomSelector stealSomething = new RandomSelector("Steal Something");
        for (int i = 0; i < art.Length; i++)
        {
            Leaf gta = new Leaf("Go To " + art[i].name, i, GoToArt);
            stealSomething.AddChild(gta);
        }

        // s2
        PrioritySelector openDoor = new PrioritySelector("Open the Door");
        goToBackDoor = new Leaf("Go To Back Door", GoToBackDoor, 2);
        goToFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor, 1);
        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackDoor);

        // s1
        Leaf hasGotMoney = new Leaf("Has Money", HasMoney);
        Inverter invertMoney = new Inverter("Invert Money");
        invertMoney.AddChild(hasGotMoney);

        // kaçış
        Sequence runAway = new Sequence("Cop Interaction");
        Leaf canSeeCop = new Leaf("Can See Cop", CanSeeCop);
        Leaf fleeFromCop = new Leaf("Run Away", FleeFromCop);
        runAway.AddChild(canSeeCop);
        runAway.AddChild(fleeFromCop);
        
        Inverter cantSeeCop = new Inverter("Cant See Cop");
        cantSeeCop.AddChild(canSeeCop);

        Sequence s1 = new Sequence ("s1");
        s1.AddChild(invertMoney);

        Sequence s2 = new Sequence ("s2");
        s2.AddChild(cantSeeCop);
        s2.AddChild(openDoor);

        Sequence s3 = new Sequence ("s3");
        s3.AddChild(cantSeeCop);
        s3.AddChild (stealSomething);

        Sequence s4 = new Sequence ("s4");
        s4.AddChild(cantSeeCop);
        s4.AddChild (goToVan);

        RootNode stealConditions = new RootNode("Steal Conditions");
        Sequence conditions = new Sequence ("Conditions");
        conditions.AddChild(cantSeeCop);
        conditions.AddChild(invertMoney);
        stealConditions.AddChild(conditions);
        DependencySequence steal = new DependencySequence("Steal Something", stealConditions, agent);

        // steal.AddChild(s1);
        // steal.AddChild(s2);
        // steal.AddChild(s3);
        // steal.AddChild(s4);

        // steal.AddChild(invertMoney);
        steal.AddChild(openDoor);

        Selector stealWithFallBack = new Selector("Steal with fall back");
        stealWithFallBack.AddChild(stealSomething);
        stealWithFallBack.AddChild (goToVan);

        Selector beThief = new Selector("Be a thief");
        beThief.AddChild(stealWithFallBack);
        beThief.AddChild(runAway);

        return beThief;
    }

    // public NodeState GoToDiamond() => GoPickUpDiamond();
    // public NodeState GoToMonaLisaPainting() => GoPickUpMonaLisaPainting();
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

    // private NodeState GoPickUpMonaLisaPainting()
    // {
    //     if (!MonaLisaPainting.activeSelf) return NodeState.FAILURE;
    //     NodeState state = GoToLocation(MonaLisaPainting.transform.position);
    //     if (state == NodeState.SUCCESS)
    //     {
    //         currentObject = MonaLisaPainting;
    //         MonaLisaPainting.transform.parent = this.gameObject.transform;
    //     }
        
    //     return state;
    // }

    private NodeState GoToArt(int index)
    {
        if (!art[index].activeSelf) return NodeState.FAILURE;
        NodeState state = GoToLocation(art[index].transform.position);
        if (state == NodeState.SUCCESS)
        {
            currentObject = art[index];
            art[index].transform.parent = this.gameObject.transform;
        }
        
        return state;
    }

    public NodeState CanSeeCop()
    {
        return CanSee(CopAgent.transform.position, "Cop", 10, 120);
    }

    public NodeState FleeFromCop()
    {
        return Flee(CopAgent.transform.position, 20);
    }

    // private NodeState GoToArt2()
    // {
    //     if (!art[1].activeSelf) return NodeState.FAILURE;
    //     NodeState state = GoToLocation(art[1].transform.position);
    //     if (state == NodeState.SUCCESS)
    //     {
    //         currentObject = art[1];
    //         art[1].transform.parent = this.gameObject.transform;
    //     }
        
    //     return state;
    // }

    // private NodeState GoToArt3()
    // {
    //     if (!art[2].activeSelf) return NodeState.FAILURE;
    //     NodeState state = GoToLocation(art[2].transform.position);
    //     if (state == NodeState.SUCCESS)
    //     {
    //         currentObject = art[2];
    //         art[2].transform.parent = this.gameObject.transform;
    //     }
        
    //     return state;
    // }

    // private NodeState GoToArt4()
    // {
    //     if (!art[3].activeSelf) return NodeState.FAILURE;
    //     NodeState state = GoToLocation(art[3].transform.position);
    //     if (state == NodeState.SUCCESS)
    //     {
    //         currentObject = art[3];
    //         art[3].transform.parent = this.gameObject.transform;
    //     }
        
    //     return state;
    // }
    // private NodeState GoToArt5()
    // {
    //     if (!art[4].activeSelf) return NodeState.FAILURE;
    //     NodeState state = GoToLocation(art[4].transform.position);
    //     if (state == NodeState.SUCCESS)
    //     {
    //         currentObject = art[4];
    //         art[4].transform.parent = this.gameObject.transform;
    //     }
        
    //     return state;
    // }

    // private NodeState GoToArt6()
    // {
    //     if (!art[5].activeSelf) return NodeState.FAILURE;
    //     NodeState state = GoToLocation(art[5].transform.position);
    //     if (state == NodeState.SUCCESS)
    //     {
    //         currentObject = art[5];
    //         art[5].transform.parent = this.gameObject.transform;
    //     }
        
    //     return state;
    // }

    // private NodeState GoToArt7()
    // {
    //     if (!art[6].activeSelf) return NodeState.FAILURE;
    //     NodeState state = GoToLocation(art[6].transform.position);
    //     if (state == NodeState.SUCCESS)
    //     {
    //         currentObject = art[6];
    //         art[6].transform.parent = this.gameObject.transform;
    //     }
        
    //     return state;
    // }

    // public NodeState GoPickUpDiamond()
    // {
    //     if (!Diamond.activeSelf) return NodeState.FAILURE;
    //     NodeState state = GoToLocation(Diamond.transform.position);
    //     if (state == NodeState.SUCCESS)
    //     {
    //         currentObject = Diamond;
    //         Diamond.transform.parent = this.gameObject.transform;
    //     }
        
    //     return state;
    // }

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
