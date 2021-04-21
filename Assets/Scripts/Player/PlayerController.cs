using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Chronos;

public class PlayerController : MonoBehaviour {

    [Header("References")]
    public Animator _animator;
    private Rigidbody2D _rigidBody;
    private Timeline time;
    private BoxCollider2D playerCollider;
    public SpriteRenderer sprite;

    //Interaction Stuff
    public float interactionRadius = 2.0f;

    [Header("Collision Info")]
    //Raycast Stuff
    public float _rayLength = 1f;
    public LayerMask layerMask;
    const float skinWidth = .015f;

    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    float horizontalRaySpacing;
    float verticalRaySpacing;

    public CollisionInfo collisions;

    Ray ray;
    RaycastOrigins raycastOrigins;



    private void Awake() {
        playerCollider = GetComponent<BoxCollider2D>();
        _rigidBody = GetComponent<Rigidbody2D>();
        time = GetComponent<Timeline>();
        layerMask = LayerMask.GetMask("Environment");
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        CalculateRaySpacing();
    }

    private void Update() {


        // Remove all player control when we're in dialogue
        if (FindObjectOfType<DialogueRunner>().isDialogueRunning == true) {
            //Move(0);
            return;
        }
                          
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        MouseRay(ray);
    }

    #region Movement and Flipping
    public void Move(Vector3 velocity) {
        UpdateRaycastOrigins();
        collisions.Reset();

        if (velocity.x != 0) {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0) {
            VerticalCollisions(ref velocity);
        }
        transform.Translate(velocity);
    }

    //Flip Sprite Function
    private void Flip(float type) {
        switch (type) {
            case -1:
                sprite.flipX = true;
                break;
            case 1:
                sprite.flipX = false;
                break;
        }
    }
    #endregion

    #region Raycasts
    void VerticalCollisions(ref Vector3 velocity) {

        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;


        for (int i = 0; i < verticalRayCount; i++) {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, layerMask);
            Debug.DrawRay(rayOrigin + Vector2.right * verticalRaySpacing * i, Vector2.up * -2, Color.red);

            if (hit) {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }

    }

    void HorizontalCollisions(ref Vector3 velocity) {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;
        Flip(directionX);

        for (int i = 0; i < horizontalRayCount; i++) {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, layerMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit) {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
        }
    }
    #endregion

    #region NPC Check
    //Check if there is any NPC nearby we can talk to
    public void CheckForNearbyNPC() {
        var allParticipants = new List<NPC>(FindObjectsOfType<NPC>());
        var target = allParticipants.Find(delegate (NPC p) {
            return string.IsNullOrEmpty(p.talkToNode) == false && // has a conversation node?
            (p.transform.position - this.transform.position)// is in range?
            .magnitude <= interactionRadius;
        });
        if (target != null) {
            // Kick off the dialogue at this node.
            FindObjectOfType<DialogueRunner>().StartDialogue(target.talkToNode);
        }
    }
    #endregion

    #region Item Check
    //Check if there is anything nearby we can add to our Inventory
    public void CheckForNearbyInteractable() {
        var allParticipants = new List<Interactable>(FindObjectsOfType<Interactable>());
        var target = allParticipants.Find(delegate (Interactable p) {
            return (p.transform.position - this.transform.position).magnitude <= interactionRadius; //Is in range
        });
        if (target != null) {
            // Kick off the dialogue at this node.
            GetComponent<InventoryManager>().GiveItem(target.ID);
        }
    }

    private Collider2D currentHover;

    public void MouseRay(Ray ray) {
        RaycastHit2D hit;
        int mask = LayerMask.GetMask("Interactable");
        hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, mask);
        if (hit.collider == null) {
            //Debug.Log("nothing hit");
            if(currentHover != null) {
                currentHover.GetComponent<Interactable>().Glow(false);
                currentHover = null;
            }
        }
        else {
            currentHover = hit.collider;
            hit.collider.GetComponent<Interactable>().Glow(true);
        }
    }


    #endregion

    void UpdateRaycastOrigins() {
        Bounds bounds = playerCollider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);

    }

    void CalculateRaySpacing() {
        Bounds bounds = playerCollider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = (bounds.size.y / 2) / (horizontalRayCount - 1);
        verticalRaySpacing = (bounds.size.x / 2) / (verticalRayCount - 1);
    }

    struct RaycastOrigins {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo {
        public bool above, below;
        public bool left, right;

        public void Reset() {
            above = below = false;
            left = right = false;
        }
    }
}
