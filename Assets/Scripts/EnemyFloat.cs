using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFloat : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float hoverAmplitude = 0.5f;
    public float hoverFrequency = 2f;
    public float followRange = 10f;
    public float chaseSpeed = 3f;

    public int ownerID = 0;

    private Vector2 startPos;
    private float timeOffset;
    private Transform targetPlayer;

    void Start()
    {
        startPos = transform.position;
        timeOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        string targetName = (ownerID == 1) ? "player1" : "player2";
        GameObject targetObj = GameObject.Find(targetName);

        if (targetObj)
        {
            float dist = Vector2.Distance(targetObj.transform.position, transform.position);
            if (dist <= followRange)
                targetPlayer = targetObj.transform;
            else
                targetPlayer = null;
        }


        float hoverY = Mathf.Sin(Time.time * hoverFrequency + timeOffset) * hoverAmplitude;
        transform.position += new Vector3(Mathf.Sin(Time.time * 0.5f + timeOffset) * moveSpeed * Time.deltaTime, hoverY * Time.deltaTime, 0);

        if (targetPlayer)
        {
            Vector2 dir = (targetPlayer.position - transform.position).normalized;
            transform.position += (Vector3)dir * chaseSpeed * Time.deltaTime;
        } 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController hitPlayer = other.GetComponent<PlayerController>();

            GameManager.Instance.SendEnemyToOtherPlayer(this.gameObject, other.gameObject);

            GameManager.Instance.ResetPlayer(hitPlayer);
        }
    }

}
