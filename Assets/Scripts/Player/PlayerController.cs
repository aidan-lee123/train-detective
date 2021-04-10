using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerController : MonoBehaviour {

    [Header("References")]
    public Animator _animator;
    private Rigidbody2D _rigidBody;
    private BoxCollider2D playerCollider;

    [Header("Inputs")]
    //Movement Stuff
    private Vector3 _velocity = Vector3.zero;
    public float _movementSmoothing = 0f;
    public float _runSpeed = 20f;
    float _horizontalMove = 0f;
    bool canMove = true;
    private bool _facingRight = false;

    //Interaction Stuff
    public float interactionRadius = 2.0f;

    //Raycast Stuff
    private float _rayLength = 1f;
    private float _rayOffset = 0.5f;
    private int _rayLayerMask;

    Ray ray;
    RaycastHit2D hit;

    private void Awake() {
        playerCollider = GetComponent<BoxCollider2D>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _rayLayerMask = 8;
    }

    private void Update() {


        // Remove all player control when we're in dialogue
        if (FindObjectOfType<DialogueRunner>().isDialogueRunning == true) {
            Move(0);
            return;
        }

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        MouseRay(ray, hit);

    }


    private void FixedUpdate() {
        _horizontalMove = Input.GetAxisRaw("Horizontal") * _runSpeed;
        if (canMove)
            Move(_horizontalMove * Time.fixedDeltaTime);
    }

    #region Movement and Flipping
    public void Move(float move) {

        Vector3 targetVelocity = new Vector2(move * 10f, _rigidBody.velocity.y);

        _animator.SetFloat("Speed", Mathf.Abs(move));

        _rigidBody.velocity = Vector3.SmoothDamp(_rigidBody.velocity, targetVelocity, ref _velocity, _movementSmoothing);

        if (move > 0 && !_facingRight) {
            Flip();
        }
        else if (move < 0 && _facingRight) {
            Flip();
        }
    }

    private void Flip() {
        _facingRight = !_facingRight;


        if (_facingRight == true) {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
        else {
            transform.eulerAngles = new Vector3(0, 0, 0);
        
        }
        /*
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;*/
    }
    #endregion

    #region Obsolete Raycasts
    public GameObject CheckForward() {

        Vector2 direction = new Vector2(1, 0);
        if (!_facingRight) {
            direction *= -1;

        }
        float directionOffset = _rayOffset * direction.x;

        Vector2 startingPosition = new Vector2(transform.position.x + directionOffset, transform.position.y);

        RaycastHit2D hit = Physics2D.Raycast(startingPosition, direction, _rayLength, _rayLayerMask);

        if (hit.collider) {
            Debug.DrawRay(startingPosition, direction, Color.red);
            return hit.collider.gameObject;
        }

        Debug.DrawRay(startingPosition, direction, Color.white);
        return null;
    }

    public GameObject CheckBack() {
        Vector2 direction = new Vector2(-1, 0);
        if (!_facingRight) {
            direction *= -1;

        }
        float directionOffset = _rayOffset * direction.x;

        Vector2 startingPosition = new Vector2(transform.position.x + directionOffset, transform.position.y);

        RaycastHit2D hit = Physics2D.Raycast(startingPosition, direction, _rayLength, _rayLayerMask);

        if (hit.collider) {
            Debug.DrawRay(startingPosition, direction, Color.red);
            return hit.collider.gameObject;
        }

        Debug.DrawRay(startingPosition, direction, Color.white);
        return null;
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

    public void MouseRay(Ray ray, RaycastHit2D hit) {
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

}
