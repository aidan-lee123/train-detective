using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : SerializedMonoBehaviour {
    public List<Cabin> cabins;
    [SerializeField]
    public List<Node> nodes = new List<Node>();
    private LayerMask layerMask;

    public Dictionary<Cabin, Node> nodeDictionary = new Dictionary<Cabin, Node>();

    void Awake() {
        cabins = CabinManager.GetCabins();
        int env = LayerMask.GetMask("Environment");
        int door = LayerMask.GetMask("Door");

        layerMask = door | env;

    }
    private void Start() {

        BuildLinks();

        Vector3 test = new Vector3(-20, 4, 0);
        NodeFromWorldspace(test);
    }
    private void Update() {

    }

    /*void SetupNodes() {
        print("SETTING UP NODES");

        foreach(Cabin cabin in cabins) {
            //print("Cabin " + cabin.name);
            //Add 
            foreach(Door door in cabin.doors) {
                Node node = new Node(door.transform.position, door.locked);
                //Need to connect nodes together (door connections)

                /*TODO
                 * Send raycast out from each node / door in a cabin.
                 * If it Hits a wall they can't be connected as walkable nodes
                 * If they don't they aren't connected but will need to look at the 
                 * connected door to see if it is a link
                 *
                //Bounds bounds = door.GetComponent<BoxCollider2D>().bounds;

                if (!nodes.Contains(node)) {
                    print("Nodes did not contain Node");
                    nodes.Add(node);
                    foreach (Door d in cabin.doors) {

                        //print("Cabin " + cabin.name + " door " + d.name);
                        if (door.gameObject == d.gameObject) {
                            break;
                        }
                        //Vector3 closestBound = bounds.ClosestPoint(d.transform.position);

                        RaycastHit2D hit = Physics2D.Linecast(door.transform.position, d.transform.position, layerMask);

                        //If it hit's either another door or a wall
                        if (hit) {
                            print("Hit object " + hit.collider.name);
                            //print(hit.collider.gameObject.layer);

                            GameObject hitGo = hit.collider.gameObject;
                            Door hitDoor = hitGo.GetComponent<Door>();

                            //If the hit Object's Layer is 13 (Doors)
                            if (hitGo.layer == 13) {

                                Debug.DrawLine(door.transform.position, hit.transform.position, Color.green);

                                //Find to see if the neighbour node exists in our node list already
                                Node neighbourNode = nodes.Find(x => x.worldPosition == hitGo.transform.position);

                                //If it doesn't exist we create it and it to the parent node's neighbour nodes
                                if(neighbourNode == null) {
                                    Node newNode = new Node(hitGo.transform.position, hitDoor.locked);
                                    nodes.Add(newNode);
                                    node.AddNeighbour(newNode);
                                }
                                else {
                                    node.AddNeighbour(neighbourNode);
                                }

                            }
                            //Otherwise it's a wall so we can't connect to it.
                        }
                        //If it doesn't hit a wall or a door then 
                        else {
                            Node neighbourNode = nodes.Find(x => x.worldPosition == d.transform.position);

                            if(neighbourNode == null) {
                                Node newNode = new Node(d.transform.position, d.locked);
                                nodes.Add(newNode);
                                node.AddNeighbour(newNode);
                            }
                            else {
                                node.AddNeighbour(neighbourNode);
                            }

                            //Debug.DrawLine(door.transform.position, d.transform.position, Color.white);
                        }
                    }
                }


                //nodes.Add(node);
            }

        }

        Debug.Log("Nodes Setup");
    }*/

    public Node NodeFromWorldspace(Vector3 position) {

        foreach(Cabin cabin in cabins) {
            if (cabin.cabinBounds.collider.bounds.Contains(position)) {
                print(cabin.cabinName + " contains positon " + position);
            }
        }


        return null;
        //throw new System.NotImplementedException();
    }

    void BuildLinks() {

        //Foreach Cabin's doors we create the nodes.
        foreach (Cabin cabin in cabins) {
            foreach (Door door in cabin.doors) {
                Node node = new Node(door.transform.position, door, door.locked);
                nodes.Add(node);
                door.node = node;
                node.NodeName = cabin.name + " - " + door.name;
                cabin.AddNode(node);
            }

            /* TODO
             * Rather than looping through each node we loop through each cabin's nodes so that there
             * is less overhead. Just need to figure out how to group Nodes and Cabins together. 
             * <Cabin, Node> Dictionary?
             * */

            //Loop through the nodes to create links
            foreach (Node node in cabin.nodes) {

                //If a node's door has a linked door then we know that they are neighbours and can link them
                if (node.door.link != null) {
                    node.AddNeighbour(node.door.link.GetComponent<Door>().node);
                }

                //Otherwise loop through each node
                foreach (Node otherNode in cabin.nodes) {
                    //Skip itself
                    if (otherNode == node) {
                        //print(otherNode.NodeName + " equals " + node.NodeName);
                        continue;
                    }
                    //Linecast from the original node to the othernodes around it. If it interacts with a another door
                    //We add it as a neigbour otherwise we don't do anything
                    RaycastHit2D hit = Physics2D.Linecast(node.worldPosition, otherNode.worldPosition, layerMask);
                    if (hit) {
                        GameObject hitGo = hit.collider.gameObject;

                        //TODO
                        //Consider changing this from layer to tag.

                        //If it hits the door layer "13"
                        if (hitGo.layer == 13) {

                            //Make sure that the node's neighbour list doesn't already contain this node.
                            if (!node.neighbours.Contains(otherNode)) {
                                //print("Node " + node.NodeName + " collided with " + hitGo.GetComponent<Door>().node.NodeName);
                                node.AddNeighbour(hitGo.GetComponent<Door>().node);
                            }

                        }
                    }
                }

                //print("Setup node " + node.NodeName);
            }
        }


        



    }

    private void OnDrawGizmos() {
        if (nodes == null)
            return;
        foreach (Node node in nodes) {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(node.worldPosition, .2f);
            foreach(Node neighbour in node.neighbours) {
                if(neighbour != null) {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(node.worldPosition, neighbour.worldPosition);
                }
            }
        }
    }


}
