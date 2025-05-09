using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NodeLevel{
    public int level;
    public Node Node;
}
public enum NodeState 
{
    SUCCESS,
    RUNNING,
    FAILURE
}

public class Node
{
    public NodeState Status;
    public string NodeName;

    public List<Node> childNodes = new List<Node>();
    public int sortOrder;
    public int currentChild = 0;

    public Node() {}
    public Node(string name) =>  NodeName = name;
    public Node(string name, int order) {
        this.NodeName = name;
        this.sortOrder = order;
    }
    
    public void AddChild(Node node) => childNodes.Add(node);
    public virtual NodeState Process() => childNodes[currentChild].Process();
    public void Reset() 
    {
        foreach (var n in childNodes){
            n.Reset();
        }
        currentChild = 0;
    }
    
}
