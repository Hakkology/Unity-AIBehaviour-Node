public class Selector : Node
{
    public Selector(string name)
    {
        NodeName = name;
    }

    public override NodeState Process()
    {
        NodeState childStatus = childNodes[currentChild].Process();
        if (childStatus == NodeState.RUNNING) return NodeState.RUNNING;
        if (childStatus == NodeState.SUCCESS) {
            currentChild = 0;
            return NodeState.SUCCESS;
        } 

        currentChild++;
        if (currentChild >= childNodes.Count) {
            currentChild = 0;
            return NodeState.FAILURE;
        }

        return NodeState.RUNNING;
    }
}