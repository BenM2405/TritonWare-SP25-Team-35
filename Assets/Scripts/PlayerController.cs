using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("MovementSettings")]
    public float moveSpeed = 6f;
    public float boostSpeed = 7f;
    public float maxFallSpeed = -25f;

    [Header("Controls")]
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode boostKey = KeyCode.W;

    Rigidbody2D rb;
    float horiz = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horiz = 0f;
        if (Input.GetKey(leftKey)) horiz -= 1f;
        if (Input.GetKey(rightKey)) horiz += 1f;

        if (Input.GetKeyDown(boostKey))
        {
            rb.AddForce(Vector2.up * boostSpeed, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horiz * moveSpeed, rb.velocity.y);

        if (rb.velocity.y < maxFallSpeed) {
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
