using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D _rigidBody;
    [SerializeField] private bool _facingRight = true;

    private Vector3 _velocity = Vector3.zero;
    public float _movementSmoothing = 0f;

    private float _rayLength = 1f;
    private float _rayOffset = 0.5f;
    private int _rayLayerMask;

    public float interactionRadius = 2.0f;

    public Animator _animator;


    private void Awake() {

        _rigidBody = GetComponent<Rigidbody2D>();
        _rayLayerMask = 8;
    }

    private void Update() {
        // Remove all player control when we're in dialogue
        if (FindObjectOfType<DialogueRunner>().isDialogueRunning == true) {
            Move(0);
            return;
        }

    }

    public void Move(float move) {

        Vector3 targetVelocity = new Vector2(move * 10f, _rigidBody.velocity.y);

        if (FindObjectOfType<DialogueRunner>().isDialogueRunning == true) {
            targetVelocity = Vector3.zero;
        }
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

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

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

}
