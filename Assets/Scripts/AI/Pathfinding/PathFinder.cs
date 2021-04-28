using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PathFinder : MonoBehaviour
{

    PathRequestManager requestManager;
    NodeManager nodeManager;
    private List<Node> draw;

    private void Awake() {
        requestManager = GetComponent<PathRequestManager>();
        nodeManager = GetComponent<NodeManager>();
    }

    public void StartFindPath() {

    }

    IEnumerator FindPath(Vector3 startPos, Vector3 endPos) {
        System.Diagnostics.Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = nodeManager.NodeFromWorldspace(startPos);
        Node targetNode = nodeManager.NodeFromWorldspace(endPos);
        yield return null;
    }

    public Vector3[] MakePath() {

        return null; 
    }

    Vector3[] SimplifyPath() {

        return null;
    }

    int GetDistance() {

        return 0;
    }
}
