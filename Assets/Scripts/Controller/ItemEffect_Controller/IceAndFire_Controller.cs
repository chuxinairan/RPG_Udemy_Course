using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAndFire_Controller : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            Debug.Log("Thunder Hit");
            EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();
            PlayerManager.instance.player.GetComponent<PlayerStats>().DoMagicDamage(enemyTarget);
        }
    }
}
