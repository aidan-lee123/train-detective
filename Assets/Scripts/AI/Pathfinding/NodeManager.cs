using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public List<Cabin> cabins;
    public List<Node> nodes;


    void Awake() {
        cabins = new List<Cabin>(GetComponents<Cabin>());
    }

    void SetupNodes() {

        foreach(Cabin cabin in cabins) {

            //Add 
            foreach(Door door in cabin.doors) {
                Node node = new Node(door.locked, door.transform.position);
                //Need to connect nodes together (door connections)
            
                /*TODO
                 * Send raycast out from each node / door in a cabin.
                 * If it Hits a wall they can't be connected as walkable nodes
                 * If they don't they aren't connected but will need to look at the 
                 * connected door to see if it is a link
                 **/
               

                nodes.Add(node);
            }

        }

        Debug.Log("Nodes Setup");
    }

}
