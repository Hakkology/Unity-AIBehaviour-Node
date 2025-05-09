using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DependencySequence : Node
{
    RootNode dependancy;
    NavMeshAgent agent;
    public DependencySequence(string name, RootNode rootNode, NavMeshAgent agent)
    {
        NodeName = name;
        dependancy = rootNode;
        this.agent = agent;
    }

    public override NodeState Process()
    {
        if (dependancy.Process() == NodeState.FAILURE)
        {
            agent.ResetPath();
            foreach (var node in childNodes)
                node.Reset();
            return NodeState.FAILURE;
        }

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
