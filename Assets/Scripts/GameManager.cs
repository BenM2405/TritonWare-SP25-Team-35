using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public Transform player1;
    public Transform player2;
    public Transform enemyParent1;
    public Transform enemyParent2;
    public GameObject enemyPrefab;
    public Transform player1CaveRoot;
    public Transform player2CaveRoot;
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;


    void Awake()
    {
        Instance = this;
    }

    public void PlayerHitEnemy(PlayerController player, EnemyFloat enemy)
    {
        int senderID = player == player1.GetComponent<PlayerController>() ? 1 : 2;

        Destroy(enemy.gameObject);

        SpawnEnemyInOtherCave(senderID);
    }

    void SpawnEnemyInOtherCave(int senderID)
    {
        Transform targetParent = (senderID == 1) ? enemyParent2 : enemyParent1;

        float x = Random.Range(-3f, 3f);
        float y = Random.Range(-2f, 2f) + targetParent.position.y - 5f;

        GameObject e = Instantiate(enemyPrefab, new Vector3(targetParent.position.x + x, y, 0), Quaternion.identity, targetParent);
        EnemyFloat enemyScript = e.GetComponent<EnemyFloat>();
        enemyScript.ownerID = (senderID == 1) ? 2 : 1;
    }

    public void SendEnemyToOtherPlayer(GameObject enemy, GameObject hitPlayer)
    {
        bool fromPlayer1 = hitPlayer == player1.gameObject;
        Transform targetCave = fromPlayer1 ? player2CaveRoot : player1CaveRoot;

        Destroy(enemy);

        float x = Random.Range(-3f, 3f);
        float y = targetCave.position.y - 5f + Random.Range(-2f, 2f);

        GameObject newEnemy = Instantiate(enemyPrefab, new Vector3(targetCave.position.x + x, y, 0), Quaternion.identity);
        newEnemy.GetComponent<EnemyFloat>().ownerID = fromPlayer1 ? 2 : 1;
    }

    public void ResetPlayer(PlayerController player)
    {
        bool isPlayer1 = player == player1.GetComponent<PlayerController>();
        Transform caveRoot = isPlayer1 ? player1CaveRoot : player2CaveRoot;

        Vector3 spawnPos = isPlayer1 ? player1SpawnPoint.position : player2SpawnPoint.position;
        player.transform.position = spawnPos;
        
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb) rb.velocity = Vector2.zero;
    }
}
