using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour {
    static PathRequestManager instance;
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathFinder pathFinder;
    NodeManager nodeManager;
    PathRequest currentPathRequest;
    bool isProcessingPath;


    Cabin destinationCabin;

    private void Awake() {
        instance = this;
        pathFinder = GetComponent<PathFinder>();
        nodeManager = GetComponent<NodeManager>();
    }

    // Update is called once per frame
    void Update() {

    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback) {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);

        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext() {
        if (!isProcessingPath && pathRequestQueue.Count > 0) {
            //Get the first item and remove it from the queue
            PathRequest previousRequest = currentPathRequest;
            currentPathRequest = pathRequestQueue.Dequeue();

            if (currentPathRequest.pathEnd == previousRequest.pathEnd) {
                if (nodeManager.NodeFromWorldspace(new Vector3(currentPathRequest.pathEnd.x + 1, currentPathRequest.pathEnd.y)).isLocked) {
                    currentPathRequest.pathEnd = new Vector3(currentPathRequest.pathEnd.x - 1, currentPathRequest.pathEnd.y);
                }
                else {
                    currentPathRequest.pathEnd = new Vector3(currentPathRequest.pathEnd.x + 1, currentPathRequest.pathEnd.y);
                }

            }

            //Now we are processing a path
            isProcessingPath = true;
            pathFinder.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }



    public void FinishedProcessingPath(Vector3[] path, bool success) {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    //Path Request Struct
    struct PathRequest {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback) {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }

}