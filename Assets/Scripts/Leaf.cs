using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : Node
{
    public delegate NodeState Tick();
    public Tick ProcessMethod;

    public Leaf() {}
    public Leaf(string name, Tick processMethod) 
    {
        NodeName = name;
        ProcessMethod = processMethod;
    }

    public override NodeState Process()
    {
        if (ProcessMethod != null) return ProcessMethod();
        return NodeState.FAILURE;
    }
}
