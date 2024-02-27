using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManagement : MonoBehaviour
{
    #region Singleton
    public static ObjectManagement Instance;
    #endregion

    [SerializeField] private Transform player;
    [SerializeField] private float updateDistance;
    [SerializeField] private float disableDistance;
    private List<GameObject> objects = new List<GameObject>();
    private Vector3 previousPlayerPosition = Vector3.zero;

    private void Awake()
    {
        Instance= this;
    }

    public void attachObject(GameObject obj)
    {
        if(objects.Contains(obj)) { Debug.Log("GameObject is already attached to the management script"); return; }
        objects.Add(obj);
    }

    public void detachObject(GameObject obj)
    {
        if(!objects.Contains(obj)) { Debug.Log("GameObject isnt attached to the management script"); return; }
        objects.Remove(obj);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = player.position;
        float sqrDist = (playerPos - previousPlayerPosition).sqrMagnitude;
        if (sqrDist > updateDistance*updateDistance)
        {
            previousPlayerPosition= playerPos;
            for(int i = 0; i < objects.Count; i++)
            {
                bool isActive = (playerPos - objects[i].transform.position).sqrMagnitude <= disableDistance*disableDistance;
                objects[i].SetActive(isActive);
            }
        }
    }
}
