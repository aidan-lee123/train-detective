using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool isLocked;
    public Vector3 worldPosition;
    int heapIndex;
    //gCost is the cost to travel from the starting node to this node
    //hCost is the heuristic estimating the cost from this node to the goal
    //In this case, hCost measures Manhattan Distance from this node to the goal
    public int gCost, hCost;
    public int fCost { get { return (gCost + hCost); } }

    //parent is used to generate the path back from the goal to the starting node
    public Node parent;

    public Node connectedNode;

    public Node(bool locked, Vector3 _worldPosition) {
        isLocked = locked;

        worldPosition = _worldPosition;
    }

    public Node FindNearest() {


        throw new System.NotImplementedException();
    }

    public int HeapIndex {
        get {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare) {
        int compare = fCost.CompareTo(nodeToCompare.fCost);

        if (compare == 0) {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }

    /*
    private void OnDrawGizmos() {
        if (neighbours == null)
            return;
        Gizmos.color = new Color(0f, 0f, 0f);

        foreach(var neighbour in neighbours) {
            if(neighbour != null) {
                Gizmos.DrawLine(transform.position, neighbour.transform.position);
            }
        }
    }
    */
}
