using UnityEngine;

public class CopBehaviour : AgentBehaviour
{
    public GameObject[] patrolPoints;
    public GameObject robber;
    float chaseDistance = 10;
    private Vector3 chaseLocation;

    public override Node ConfigureSequence()
    {
        Sequence selectPatrolPoint = new Sequence("Select Patrol Point");
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            Leaf pp = new Leaf("Go To " + patrolPoints[i].name, i, GoToPoint);
            selectPatrolPoint.AddChild(pp);
        }

        Sequence chaseRobber = new Sequence("Chase robber");
        Leaf canSee = new Leaf("Can See Robber?", CanSeeRobber);
        Leaf chase = new Leaf("Chase Robber", ChaseRobber);

        chaseRobber.AddChild(canSee);
        chaseRobber.AddChild(chase);

        Inverter cantSeeRobber = new Inverter("Cant See Robber");
        cantSeeRobber.AddChild(canSee);

        RootNode patrolConditions = new RootNode();
        Sequence condition = new Sequence("Patrol Conditions");
        condition.AddChild(cantSeeRobber);
        patrolConditions.AddChild(condition);

        DependencySequence patrol = new DependencySequence("Patrol until See Robber", patrolConditions, agent);
        patrol.AddChild(selectPatrolPoint);

        Selector beCop = new Selector("Be A Cop");
        beCop.AddChild(patrol);
        beCop.AddChild(chaseRobber);

        return beCop;
    }

    public NodeState GoToPoint(int index)
    {
        NodeState state = GoToLocation(patrolPoints[index].transform.position);
        return state;
    }

    public NodeState CanSeeRobber()
    {
        return CanSee(robber.transform.position, "Robber", 5, 60);
    }

    public NodeState ChaseRobber()
    {
        if (state == ActionState.IDLE)
        {
            chaseLocation = this.transform.position - (transform.position - robber.transform.position).normalized * chaseDistance;
        }
        return GoToLocation(chaseLocation);
    }
}