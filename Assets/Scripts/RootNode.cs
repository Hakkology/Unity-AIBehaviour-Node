using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootNode : Node
{
    private string debugName;
    public RootNode() => NodeName = "Root";
    public RootNode(string name)  => NodeName = name;
    public void PrintTree()
    {
        debugName = "";
        PrintTreeRecursive(this, "");
        Debug.Log(debugName);
    }
    
    private void PrintTreeRecursive(Node node, string indent)
    {
        debugName += indent + node.NodeName + "\n";
        foreach (Node child in node.childNodes)
        {
            PrintTreeRecursive(child, indent + "  ");
        }
    }
}
