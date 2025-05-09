using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    public Sequence(string name) => NodeName = name;
    public override NodeState Process()
    {
        NodeState childStatus = childNodes[currentChild].Process();

        if (childStatus == NodeState.RUNNING) return NodeState.RUNNING;

        if (childStatus == NodeState.FAILURE) 
        {
            currentChild = 0;
            foreach (var child in childNodes)
                child.Reset();
            return NodeState.FAILURE;
        }
        
        currentChild++;
        if (currentChild >= childNodes.Count) 
        {
            currentChild = 0;
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }
}
