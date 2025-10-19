using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Identity")]
    public int playerID = 1; // <-- NEW: Set this to 1 for Player 1, 2 for Player 2

    [Header("MovementSettings")]
    public float moveSpeed = 6f;
    public float maxFallSpeed = -25f;

    [Header("Jumping")]
    public float jumpVelocity = 15f;
    public float fallMultiplier = 3.5f;
    public float lowJumpMultiplier = 2.5f;

    [Header("GroundCheck (Setup Required)")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    private bool isGrounded;

    [Header("Controls")]
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode boostKey = KeyCode.W;
    public KeyCode sabotageKey = KeyCode.S; // <-- NEW: The key to sabotage the other player

    Rigidbody2D rb;
    float horiz = 0f;
    SpriteRenderer sr;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        horiz = 0f;
        if (Input.GetKey(leftKey)) horiz -= 1f;
        if (Input.GetKey(rightKey)) horiz += 1f;

        if (horiz < 0f)
        {
            sr.flipX = true;
        }
        else if (horiz > 0f)
        {
            sr.flipX = false;
        }

        if (Input.GetKeyDown(boostKey) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
        }

        // --- NEW SABOTAGE LOGIC ---
        if (Input.GetKeyDown(sabotageKey))
        {
            GameManager.Instance.SabotageOtherPlayer(playerID);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horiz * moveSpeed, rb.velocity.y);

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(boostKey))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

        if (rb.velocity.y < maxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyFloat enemy = collision.gameObject.GetComponent<EnemyFloat>();
        if (enemy != null)
        {
            GameManager.Instance.PlayerHitEnemy(this, enemy);
        }
    }
}