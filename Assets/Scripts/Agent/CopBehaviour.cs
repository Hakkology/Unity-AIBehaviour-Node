using UnityEngine;

public class CopBehaviour : AgentBehaviour
{
    public GameObject[] patrolPoints;

    public override Node ConfigureSequence()
    {
        Sequence selectPatrolPoint = new Sequence("Select Patrol Point");
        foreach (var patrolPoint in patrolPoints)
        {

        }

        return selectPatrolPoint;
    }


}