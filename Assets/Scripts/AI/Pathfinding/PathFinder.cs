using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PathFinder : MonoBehaviour
{

    PathRequestManager requestManager;

    private List<Node> draw;

    private void Awake() {
        requestManager = GetComponent<PathRequestManager>();
    }

    public void StartFindPath() {

    }

    IEnumerator FindPath(Vector3 startPos, Vector3 endPost) {
        System.Diagnostics.Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode;
        Node targetNode;
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
