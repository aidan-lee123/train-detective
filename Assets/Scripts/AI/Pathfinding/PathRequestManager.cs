using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    static PathRequestManager instance;
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();


    Cabin destinationCabin;

    private void Awake() {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void RequestPath() {

    }

    void TryProcessNext() {

    }

    public void FinishProcessingPath() {

    }

    //Path Request Struct
    struct PathRequest {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;
        public bool buildPath;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback, bool _buildPath) {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
            buildPath = _buildPath;
        }
    }
}
