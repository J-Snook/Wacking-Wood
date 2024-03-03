using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfRestock : MonoBehaviour
{
    [SerializeField] private float restockDelay;

    public IEnumerator restock(GameObject item)
    {
        item.transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(restockDelay);
        item.transform.localScale = Vector3.one *0.1f;
    }
}

