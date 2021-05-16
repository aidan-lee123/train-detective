using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PathFinder : MonoBehaviour
{

    PathRequestManager requestManager;
    NodeManager nodeManager;
    private List<Node> draw;

    private void Awake() {
        requestManager = GetComponent<PathRequestManager>();
        nodeManager = GetComponent<NodeManager>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 endPos) {

        System.Diagnostics.Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;
        Debug.Log("StartPos: " + startPos);
        Debug.Log("EndPos: " + endPos);

        Node startNode = nodeManager.NodeFromWorldspace(startPos);
        Node targetNode = nodeManager.NodeFromWorldspace(endPos);
        yield return null;
        //Debug.Log(startNode.neighbours.Count);

        if (startNode.neighbours.Count > 0 && targetNode != null) {
            //Debug.Log("startNode neigbours count is greater than 0 and target node is not null");
            //openList is the neighbors remaining
            //closedList is the nodes visited
            Heap<Node> openSet = new Heap<Node>(nodeManager.nodes.Count);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while(openSet.Count > 0) {
                //Debug.Log("Open Set Count is Greater than 0");
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if(currentNode == targetNode) {
                    sw.Stop();
                    Debug.Log("Completed in: " + sw.ElapsedMilliseconds + "ms");
                    pathSuccess = true;
                    break;
                }

                foreach(Node neighbour in currentNode.neighbours) {

                    if(neighbour.isLocked || closedSet.Contains(neighbour)) {
                        //Debug.Log("Neighbour is locked or in closed set");
                        continue;
                    }

                    float newMovementCostToNeighbout = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if(newMovementCostToNeighbout < neighbour.gCost || !openSet.Contains(neighbour)) {
                        neighbour.gCost = newMovementCostToNeighbout;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
        }

        yield return null;

        if (pathSuccess) {
            print("Path Success");
            waypoints = MakePath(startNode, targetNode);
        }

        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    public Vector3[] MakePath(Node start, Node end) {
        List<Node> path = new List<Node>();
        Node current = end;

        while (current != start) {
            path.Add(current);
            current = current.parent;
        }

        path.Add(start);
        start.parent = current;

        //and flip it so the next node in the path is at [0]
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);

        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path) {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++) {
            Vector2 directionNew = new Vector2(path[i - 1].worldPosition.x - path[i].worldPosition.x, path[i - 1].worldPosition.y - path[i].worldPosition.y);

            if (directionNew != directionOld) {
                waypoints.Add(path[i - 1].worldPosition);
            }
            directionOld = directionNew;
        }

        return waypoints.ToArray();
    }

    float GetDistance(Node nodeA, Node nodeB) {
        float distX = Mathf.Abs(nodeA.worldPosition.x - nodeB.worldPosition.x);
        float distY = Mathf.Abs(nodeA.worldPosition.y - nodeB.worldPosition.y);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);

    }
}
