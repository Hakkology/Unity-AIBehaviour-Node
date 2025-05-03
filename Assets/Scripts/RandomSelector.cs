using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomSelector : Node
{
    Node [] nodeArray;
    private bool isShuffled = false;
    public RandomSelector(string name)
    {
        NodeName = name;
    }

    void RandomizeNodes()
    {
        // nodeArray = childNodes.ToArray();
        // Sort(nodeArray, 0, childNodes.Count-1);
        // childNodes = new List<Node>(nodeArray);
        // childNodes = childNodes.OrderBy(n => n.sortOrder).ToList();
        childNodes = childNodes.OrderBy(n => Random.value).ToList();
    }

    public override NodeState Process()
    {
        if (!isShuffled)
        {
            RandomizeNodes();
            isShuffled = true;
        }

        NodeState childStatus = childNodes[currentChild].Process();
        if (childStatus == NodeState.RUNNING) return NodeState.RUNNING;
        if (childStatus == NodeState.SUCCESS) {
            // childNodes[currentChild].sortOrder = 1;
            currentChild = 0;
            isShuffled = false;
            return NodeState.SUCCESS;
        } 
        // else
        // {
        //     childNodes[currentChild].sortOrder = 10;
        // }

        currentChild++;
        if (currentChild >= childNodes.Count) {
            currentChild = 0;
            isShuffled = false;
            return NodeState.FAILURE;
        }

        return NodeState.RUNNING;
    }

    void Sort(Node[] array, int low, int high)
    {
        // LINQ ile sıralama
        var ordered = array.Skip(low).Take(high - low + 1)
                        .OrderBy(n => n.sortOrder)
                        .ToArray();

        for (int i = 0; i < ordered.Length; i++)
        {
            array[low + i] = ordered[i];
        }
    }

    void Randomize()
    {
        int n = childNodes.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            Node temp = childNodes[k];
            childNodes[k] = childNodes[n];
            childNodes[n] = temp;
        }
    }

    // int Partition(Node[] array, int low, int high)
    // {
    //     Node pivot = array[high];

    //     int lowIndex = (low - 1);

    //     //2. Reorder the collection.
    //     for (int j = low; j < high; j++)
    //     {
    //         if (array[j].sortOrder <= pivot.sortOrder)
    //         {
    //             lowIndex++;

    //             Node temp = array[lowIndex];
    //             array[lowIndex] = array[j];
    //             array[j] = temp;
    //         }
    //     }

    //     Node temp1 = array[lowIndex + 1];
    //     array[lowIndex + 1] = array[high];
    //     array[high] = temp1;

    //     return lowIndex + 1;
    // }

    // void Sort(Node[] array, int low, int high)
    // {
    //     if (low < high)
    //     {
    //         int partitionIndex = Partition(array, low, high);
    //         Sort(array, low, partitionIndex - 1);
    //         Sort(array, partitionIndex + 1, high);
    //     }
    // }
}