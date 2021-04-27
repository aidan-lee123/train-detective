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

    public List<Node> neighbours = new List<Node>();
    public string NodeName { get; set; }
    public Door door;

    //Used so that we can iterate backwards and get the route
    public Node parent;

    public Node(Vector3 _worldPosition, bool locked ) {
        isLocked = locked;
        worldPosition = _worldPosition;
    }

    public Node(Vector3 _worldPosition, Door _door, bool locked) {
        isLocked = locked;
        door = _door;
        worldPosition = _worldPosition;
    }

    public Node(Vector3 _worldPosition) {
        worldPosition = _worldPosition;
    }

    public List<Node> AddNeighbour(Node node) {
        neighbours.Add(node);
        return neighbours;
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

    public override int GetHashCode() {
        return base.GetHashCode();
    }

    public override bool Equals(object obj) {
        if (obj == null) return false;
        Node other = obj as Node;
        if (other == null) return false;
        return Equals(other);
    }

    public bool Equals(Node other) {
        if (other == null) return false;
        return (this.worldPosition.Equals(other.worldPosition));

    }


    

}
