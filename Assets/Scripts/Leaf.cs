using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : Node
{
    public delegate NodeState Tick();
    public Tick ProcessMethod;

    public delegate NodeState TickM(int val);
    public TickM ProcessMethodM;

    public int index;


    public Leaf() {}
    public Leaf(string name, Tick processMethod) 
    {
        NodeName = name;
        ProcessMethod = processMethod;
    }

    public Leaf(string name, int i, TickM processMethod) 
    {
        NodeName = name;
        index = i;
        ProcessMethodM = processMethod;
    }

    public Leaf(string name, Tick processMethod, int order) 
    {
        NodeName = name;
        ProcessMethod = processMethod;
        sortOrder = order;
    }

    public override NodeState Process()
    {
        if (ProcessMethod != null) return ProcessMethod();
        else if (ProcessMethodM != null) return ProcessMethodM(index);
        return NodeState.FAILURE;
    }
}
