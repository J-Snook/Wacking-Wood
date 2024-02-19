using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FellTreeSliceHit : MonoBehaviour, IHitSystem
{

    public void Hit(InteractionSystem player, RaycastHit hit)
    {
        if (transform.parent.TryGetComponent(out FellTreeHit trunkScript))
        {
            trunkScript.SliceHit();
        }
    }
}