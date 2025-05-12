using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatronBehaviour : AgentBehaviour
{
    public GameObject[] art;
    public GameObject frontDoor;
    public GameObject homeBase;

    [Range(0, 1000)]
    public int boredom = 0;

    protected override void Start()
    {
        base.Start();
    }

    public override Node ConfigureSequence()
    {
        RandomSelector selectObject = new RandomSelector("Select Art to View");
        for (int i = 0; i < art.Length; i++)
        {
            Leaf gta = new Leaf("Go To " + art[i].name, i, GoToArt);
            selectObject.AddChild(gta);
        }

        Leaf goToFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor);
        Leaf goToHome = new Leaf("Go Home", GoHome);
        Leaf isBored = new Leaf("Is Bored?", IsBored);

        Sequence viewArt = new Sequence("View Art");
        viewArt.AddChild(isBored);
        viewArt.AddChild(goToFrontDoor);
        viewArt.AddChild(selectObject);
        viewArt.AddChild(goToHome);

        Selector bePatron = new Selector("Be An Art Patron");
        bePatron.AddChild(viewArt);
        return bePatron;
    }

    private NodeState GoToArt(int index)
    {
        if (!art[index].activeSelf) return NodeState.FAILURE;
        NodeState state = GoToLocation(art[index].transform.position);
        return state;
    }

    public NodeState GoToFrontDoor()
    {
        NodeState state = GoToDoor(frontDoor);
        return state;
    }

    public NodeState GoHome()
    {
        NodeState state = GoToLocation(homeBase.transform.position);
        return state;
    }

    public NodeState IsBored()
    {
        if (boredom < 100)
            return NodeState.FAILURE;
        else
            return NodeState.SUCCESS;
    }


}
