using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootNode : Node
{
    private string debugName;
    public RootNode() => NodeName = "Root";
    public RootNode(string name)  => NodeName = name;
    public override NodeState Process()
    {
        if(childNodes.Count == 0) return NodeState.SUCCESS;
        return childNodes[currentChild].Process();
    }

    public void PrintTree()
    {
        debugName = "";
        NodeLevel startNode = new NodeLevel { level = 0, Node = this };
        PrintTreeRecursive(startNode);
        Debug.Log(debugName);
    }
    
    private void PrintTreeRecursive(NodeLevel nodeLevel)
    {
        debugName += "Level " + nodeLevel.level + ": " + nodeLevel.Node.NodeName + "\n";
        
        foreach (Node child in nodeLevel.Node.childNodes)
        {
            NodeLevel childLevel = new NodeLevel { level = nodeLevel.level + 1, Node = child };
            PrintTreeRecursive(childLevel);
        }
    }
}
