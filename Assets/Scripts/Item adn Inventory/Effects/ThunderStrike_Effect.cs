using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunder strike", menuName = "Data/Item effect/Thunder strike")]
public class ThunderStrike_Effect : ItemEffect
{
    [SerializeField] public GameObject thunderStrikePrefab;
    public override void ExecuteEffect(Transform _enemyTransform)
    {
        GameObject thunderObject = Instantiate(thunderStrikePrefab, _enemyTransform.position, Quaternion.identity);
        Destroy(thunderObject, 1f);
    }
}
