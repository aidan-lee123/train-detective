using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingTest : MonoBehaviour
{
    bool isWalking = false;

    Node[] path;
    int targetIndex;
    public float walkSpeed = 5f;
    NodeManager nodeManager;
    Vector3 position, target;

    // Start is called before the first frame update
    void Start()
    {
        nodeManager = FindObjectOfType<NodeManager>();
    }

    //replace Update method in your class with this one
    void Update() {

        if (Input.GetMouseButtonDown(0)) {
            //create a ray cast and set it to the mouses cursor position in game
            Debug.Log("Clicked");
            Vector3 pos = Input.mousePosition;
            pos.z = 0;
            pos = Camera.main.ScreenToWorldPoint(pos);

            FindPath(pos);

            //nodeManager.UpdateTarget(pos);
        }
    }

    public void FindPath(Vector3 _target) {
        print("Finding Path");

        Vector3 currentPos = new Vector3(transform.position.x, transform.position.y, 0);
        Vector3 targetPos = new Vector3(_target.x, _target.y, 0);
        target = targetPos;
        position = currentPos;

        PathRequestManager.RequestPath(currentPos, targetPos, OnPathFound);
        isWalking = true;
    }

    public void OnPathFound(Node[] newPath, bool pathSuccessful) {
        print("Path Found");
        if (pathSuccessful) {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
            isWalking = false;
        }
        else {
            Debug.Log("Could not Path to Node");
            StopCoroutine("FollowPath");
            isWalking = false;
        }
    }

    IEnumerator FollowPath() {
        Node currentNode = path[0];
        Debug.Log("Beginning Follow Path");
        while (true) {
            if (transform.position == currentNode.worldPosition) {
                targetIndex++;
                if (targetIndex >= path.Length) {
                    targetIndex = 0;
                    path = new Node[0];
                    yield break;
                }

                currentNode = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentNode.worldPosition, walkSpeed * Time.deltaTime);


            yield return null;
        }
    }

    public void OnDrawGizmos() {
        if(position != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(target, .4f);
            Gizmos.DrawWireSphere(position, .4f);
        }

        if (path != null) {
            for (int i = targetIndex; i < path.Length; i++) {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i].worldPosition, Vector3.one);
                if (i == targetIndex) {
                    Gizmos.DrawLine(transform.position, path[i].worldPosition);
                }
                else {
                    Gizmos.DrawLine(path[i - 1].worldPosition, path[i].worldPosition);
                }
            }
        }
    }
}
