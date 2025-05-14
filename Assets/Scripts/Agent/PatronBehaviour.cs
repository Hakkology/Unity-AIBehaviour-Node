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
    public bool ticket = false;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(nameof(IncreaseBoredom));
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
        Leaf isOpen = new Leaf("Is Open?", IsOpen);

        Sequence viewArt = new Sequence("View Art");
        
        viewArt.AddChild(isOpen);
        viewArt.AddChild(isBored);
        viewArt.AddChild(goToFrontDoor);

        Leaf noTicket = new Leaf("Wait For Ticket", NoTicket);
        Leaf isWaiting = new Leaf("Waiting For Worker", IsWaiting);
        RootNode waitForTicket = new RootNode();
        waitForTicket.AddChild(noTicket);
        LoopNode getTicket = new LoopNode("Ticket", waitForTicket);
        getTicket.AddChild(isWaiting);

        viewArt.AddChild(getTicket);

        RootNode whileBored = new RootNode();
        whileBored.AddChild(isBored);
        LoopNode lookAtPaintings = new LoopNode("Look", whileBored);
        lookAtPaintings.AddChild(selectObject);
        viewArt.AddChild(lookAtPaintings);

        viewArt.AddChild(goToHome);

        RootNode galleryOpenCondition = new RootNode();
        galleryOpenCondition.AddChild(isOpen);

        DependencySequence bePatron = new DependencySequence("Be An Art Patron", galleryOpenCondition, agent);
        bePatron.AddChild(viewArt);

        Selector viewArtWithFallback = new Selector("View Art with Fallback");
        viewArtWithFallback.AddChild(bePatron);
        viewArtWithFallback.AddChild(goToHome);

        return viewArtWithFallback;
    }

    IEnumerator IncreaseBoredom(){
        while (true)
        {
            boredom = Mathf.Clamp(boredom + 40, 0, 1000);
            yield return new WaitForSeconds(Random.Range(1,5));
        }
    }

    private NodeState GoToArt(int index)
    {
        if (!art[index].activeSelf) return NodeState.FAILURE;
        NodeState state = GoToLocation(art[index].transform.position);
        if (state == NodeState.SUCCESS)
        {
            boredom = Mathf.Clamp(boredom - 150, 0, 1000);
        }
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

    public NodeState IsOpen()
    {
        if (Blackboard.Instance.timeOfDay < 9 || Blackboard.Instance.timeOfDay > 17)
            return NodeState.FAILURE;
        else
            return NodeState.SUCCESS;
    }

    public NodeState NoTicket(){
        if(ticket || IsOpen() == NodeState.FAILURE)
        {
            return NodeState.FAILURE;
        }
        else
        {
            return NodeState.SUCCESS;
        }
    }

    public NodeState IsWaiting() 
    {
        if (Blackboard.Instance.RegisterPatron(this.gameObject) == this.gameObject)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
