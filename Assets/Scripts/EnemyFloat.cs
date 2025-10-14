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
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform nearest = null;
        float nearestDist = Mathf.Infinity;
        
        foreach (var p in players)
        {
            float dist = Vector2.Distance(p.transform.position, transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = p.transform;
            }
        }

        targetPlayer = (nearestDist <= followRange) ? nearest : null;

        float hoverY = Mathf.Sin(Time.time * hoverFrequency + timeOffset) * hoverAmplitude;
        transform.position += new Vector3(Mathf.Sin(Time.time * 0.5f + timeOffset) * moveSpeed * Time.deltaTime, hoverY * Time.deltaTime, 0);

        if (targetPlayer)
        {
            Vector2 dir = (targetPlayer.position - transform.position).normalized;
            transform.position += (Vector3)dir * chaseSpeed * Time.deltaTime;
        }
    }
}
