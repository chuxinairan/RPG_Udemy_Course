using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice and Fire", menuName = "Data/Item effect/Ice and Fire")]
public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] public GameObject IceAndFirePrefab;
    [SerializeField] public Vector2 velocity;

    public override void ExecuteEffect(Transform _respawnPosition)
    {
        Player player = PlayerManager.instance.player;
        if(player.attackState.comboCount == 2)
        {
            GameObject iceAndFire = Instantiate(IceAndFirePrefab, _respawnPosition.position, player.transform.rotation);
            iceAndFire.GetComponent<Rigidbody2D>().velocity = velocity * player.facingDir;
            Destroy(iceAndFire, 2f);
        }
    }
}