using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword_Thunder Strike Effect", menuName = "Data/Item Effect/Thunder Strike")]
public class Sword_ThunderStrikeEffect : ItemEffect
{

    [SerializeField] private GameObject thunderStrikePrefab;

    public override void ExecuteEffect(Transform _enemyTransform)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, _enemyTransform.position, Quaternion.identity);

        Destroy(newThunderStrike, 1f);
    }
}
