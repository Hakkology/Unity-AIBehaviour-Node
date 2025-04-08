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
        if (childStatus == NodeState.FAILURE) 
            return NodeState.SUCCESS;
        else
            return NodeState.FAILURE;
    }
}
