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
    private Node target = null; 

    void Awake() {
        cabins = CabinManager.GetCabins();
        int env = LayerMask.GetMask("Environment");
        int door = LayerMask.GetMask("Door");

        layerMask = door | env;

    }
    private void Start() {

        BuildLinks();
    }
    private void Update() {

    }
    [Button("Update Target")]
    public void UpdateTarget(Vector3 pos) {

        Vector3 test = GameObject.Find("Test Target").transform.position;
        target = NodeFromWorldspace(pos);
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
        Cabin foundCabin = null;
        position.z = 0;

        foreach(Cabin cabin in cabins) {
            if (cabin.cabinBounds.collider.bounds.Contains(position)) {
                //print(cabin.cabinName + " contains positon " + position);
                foundCabin = cabin;
            }
        }

        /* TODO
         * Get 2 closest nodes to each other and then remove them from each other's neighbour nodes list
         * Maybe do this by looping through the neighbours list and if it equals the other one then remove it from both
         * then add this new node as neighbours to both of them 
         * That way this is slipped into the middle of both of them
         * 
         * So need to add in a new second node var for secondClosestNode
         * */


        if(foundCabin != null) {
            float closestDist = float.MaxValue;
            float secondDist = float.MaxValue;
            Node closestNode = null;
            Node secondNode = null;
            foreach (Node node in foundCabin.nodes) {
                Node hit = RaycastToNode(position, node.worldPosition);
                float dist = Vector3.Distance(position, node.worldPosition);


                if (hit == null || hit != node) {
                    //print("Hit Null or Does Not Equal " + node.NodeName);
                    continue;
                }

                //If there is more than one node in the cabin find both Closest and Second Closest Nodes
                if (foundCabin.nodes.Count == 1) {
                    //print("Only 1 Node");
                    closestDist = dist;
                    closestNode = node;
                    continue;
                }


                if(dist < closestDist) {

                    //Closest node has been updated
                    secondDist = closestDist;
                    secondNode = closestNode;
                    closestDist = dist;
                    closestNode = node;
                    
                }
                //If the distance is in between the closest and second closest then update the second closest
                else if (dist < secondDist && dist != closestDist) {
                    secondDist = dist;
                    secondNode = node;
                }
            }

            Node newNode = new Node(new Vector3(position.x, closestNode.worldPosition.y, 0));
            newNode.NodeName = (foundCabin.cabinName + " - " + foundCabin.nodes.Count);

            closestNode.AddNeighbour(newNode);
            newNode.AddNeighbour(closestNode);

            //if there is more than one node that is in LoS to this new node
            if (secondNode != null) {

                closestNode.RemoveNeighbour(secondNode);
                secondNode.RemoveNeighbour(closestNode);
                secondNode.AddNeighbour(newNode);
                newNode.AddNeighbour(secondNode);

            }


            //print(newNode.worldPosition);
            foundCabin.AddNode(newNode);
            nodes.Add(newNode);

            print("Created New Node, " + newNode.NodeName + ", at position: " + newNode.worldPosition);
            return newNode;
        }


        return null;
        //throw new System.NotImplementedException();
    }

    public Node RaycastToNode(Vector3 pos, Vector3 node) {

        RaycastHit2D hit = Physics2D.Linecast(pos, node, layerMask);
        if (hit) {
            
            if(hit.collider.gameObject.layer == 13) {
                return hit.collider.gameObject.GetComponent<Door>().node;
            }
        }
        return null;
    }

    public void BuildLinks() {
        nodes = new List<Node>();
        //Foreach Cabin's doors we create the nodes.
        foreach (Cabin cabin in cabins) {
            foreach (Door door in cabin.doors) {
                Node node = new Node(door.transform.position, door, door.locked);
                nodes.Add(node);
                door.node = node;
                node.NodeName = cabin.name + " - " + door.name;
                cabin.AddNode(node);
            }

            //Loop through the nodes to create links
            foreach (Node node in cabin.nodes) {

                //If a node's door has a linked door then we know that they are neighbours and can link them
                if (node.door.link != null && !node.door.locked && !node.door.link.locked) {
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
                    Node hit = RaycastToNode(node.worldPosition, otherNode.worldPosition);
                    if(hit != null) {
                        //Make sure that the node's neighbour list doesn't already contain this node.
                        if (!node.neighbours.Contains(otherNode)) {
                            //print("Node " + node.NodeName + " collided with " + hitGo.GetComponent<Door>().node.NodeName);
                            node.AddNeighbour(hit);
                        }
                    }
                }

            }
        }


        /* TODO - FIGURE OUT WHY THIS IS HAPPENING
         * We have to do another loop through the first cabin's node's because nothing is created when it first
         * gets initalized. That's not true though which is weird. There is also for some reason a non existant 
         * Node that is placed at 0,0,0 that messes with the neigbour system. This node doesn't actually exist
         * it's not in cabin's node list or being created anywhere but for some reason screws everything up.
         * I'm not sure if this is a 0,0,0 issue or another issue. I haven't tested moving things out of the
         * 0,0,0 range though. 
         * */

        //Foreach node in the first cabin of the list
        foreach(Node node in cabins[0].nodes) {

            //Clears the node's neighbours
            node.ClearNeighbours();

            //Does exactly the same as what we're doing above but only for the first cabin.
            foreach(Node otherNode in cabins[0].nodes) {

                //If a node's door has a linked door then we know that they are neighbours and can link them
                if (node.door.link != null && !node.door.locked && !node.door.link.locked) {
                    node.AddNeighbour(node.door.link.GetComponent<Door>().node);
                }

                Node hit = RaycastToNode(node.worldPosition, otherNode.worldPosition);
                if (hit != null) {
                    //Make sure that the node's neighbour list doesn't already contain this node.
                    if (!node.neighbours.Contains(otherNode)) {
                        //print("Node " + node.NodeName + " collided with " + hitGo.GetComponent<Door>().node.NodeName);
                        node.AddNeighbour(hit);
                    }
                }
            }

        }
    }

    private void OnDrawGizmos() {
        if (nodes == null)
            return;
        foreach (Node node in nodes) {
            if (node.neighbours.Count == 1)
                Gizmos.color = Color.grey;
            else
                Gizmos.color = Color.white;

            Gizmos.DrawWireSphere(node.worldPosition, .2f);
            foreach(Node neighbour in node.neighbours) {
                if(neighbour != null) {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(node.worldPosition, neighbour.worldPosition);
                }
            }
        }

        if(target != null) {
            Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(target.worldPosition, .4f);
        }
    }


}
