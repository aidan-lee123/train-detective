using Chronos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    NPC npc;

    [Header("Collision Info")]
    public BoxCollider2D npcCollider;
    //Raycast Stuff
    public float _rayLength = 1f;
    public LayerMask layerMask;
    const float skinWidth = .015f;

    public float gravity = -10f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    float horizontalRaySpacing;
    float verticalRaySpacing;
    private Timeline time;
    Vector2 velocity;
    public CollisionInfo collisions;

    Ray ray;
    RaycastOrigins raycastOrigins;

    // Start is called before the first frame update
    void Start()
    {
        npc = GetComponent<NPC>();

        npcCollider = GetComponent<BoxCollider2D>();
        time = GetComponent<Timeline>();
        CalculateRaySpacing();
    }

    public void Follow(Transform target) {
        //print(Vector2.SignedAngle(transform.position, target.position));
        float angle = Vector2.SignedAngle(transform.position, target.position);
        float move = Mathf.Clamp(angle, -1, 1);
        float distance = Vector2.Distance(transform.position, target.position);
        Vector2 velocity = new Vector2(move * (npc.moveSpeed), 0);
        //print(velocity);
        if (distance > 0.01f) {
            Move(velocity);

        }
        else {
            print("MADE IT");
        }
    }

    public void TraverseDoor(Door currentDoor, Door targetDoor, float speed) {
        Door linkedDoor = currentDoor.link;
        print("TRAVERSE DOOR");
        //PLAY ANIMATION, DELAY HOWEVER LONG, TELEPORT NPC
        currentDoor.MoveCharacter(gameObject);
        //transform.position = targetDoor.transform.position;
        //transform.position = Vector3.MoveTowards(transform.position, linkedDoor.transform.position, speed * Time.deltaTime);
    }

    IEnumerator WaitAnimationLength(AnimationClip animation) {


        yield return new WaitForSeconds(animation.length);
    }

    public void MoveTowards(Vector2 target) {
        //Get the location of the node in relation to the current NPC (transform)
        Vector2 targetRelative = transform.InverseTransformPoint(target);

        //If it is Negative it will be -1 if it is positive it will be 1
        float move = Mathf.Sign(targetRelative.x);


        if (collisions.above || collisions.below) {
            velocity.y = 0;
        }


        float distance = Vector2.Distance(new Vector2(transform.position.x, target.y), target);
        velocity = new Vector2(move * (npc.moveSpeed), 0);
        velocity.y += gravity * time.deltaTime;

        //print("Heading to " + target + " with move of " + move);
        //print(velocity);
        print(distance);
        if (distance > 0.1f) {
            Move(velocity);

        } else {
            print("MADE IT");
        }


    }


    public void Move(Vector2 velocity) {
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

    private void Flip(float type) {
        switch (type) {
            case -1:
                npc.spriteRenderer.flipX = true;
                break;
            case 1:
                npc.spriteRenderer.flipX = false;
                break;
        }
    }

    void VerticalCollisions(ref Vector2 velocity) {

        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;


        for (int i = 0; i < verticalRayCount; i++) {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, layerMask);


            if (hit) {
                Debug.DrawRay(rayOrigin + Vector2.right * verticalRaySpacing * i, Vector2.up * -2, Color.red);
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
            else {
                Debug.DrawRay(rayOrigin + Vector2.right * verticalRaySpacing * i, Vector2.up * -2, Color.white);
            }
        }

    }

    void HorizontalCollisions(ref Vector2 velocity) {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;
        Flip(directionX);

        for (int i = 0; i < horizontalRayCount; i++) {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, layerMask);



            if (hit) {
                Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
            else {
                Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.white);
            }
        }
    }

    void UpdateRaycastOrigins() {
        Bounds bounds = npcCollider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);

    }

    void CalculateRaySpacing() {
        Bounds bounds = npcCollider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
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
