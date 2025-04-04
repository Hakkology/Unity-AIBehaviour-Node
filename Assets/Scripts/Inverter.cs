using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Node
{
    public Inverter(string name) => NodeName = name;
    public override NodeState Process()
    {
        NodeState childStatus = childNodes[currentChild].Process();
        if (childStatus == NodeState.RUNNING) return NodeState.RUNNING;
        if (childStatus == NodeState.FAILURE) return childStatus;
        
        currentChild++;
        if (currentChild >= childNodes.Count) 
        {
            currentChild = 0;
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }
}
