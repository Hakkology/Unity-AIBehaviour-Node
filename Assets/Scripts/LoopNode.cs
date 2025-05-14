using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopNode : Node
{
    RootNode dependency;
    public LoopNode(string name, RootNode nodeTree)
    {
        dependency = nodeTree;
        NodeName = name;
    }

    public override NodeState Process()
    {
        if (dependency.Process() == NodeState.FAILURE)
        {
            return NodeState.SUCCESS;
        }
        
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
