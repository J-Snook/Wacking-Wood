using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ObjectPooler;
using UnityEngine.Pool;

interface IPooledObject
{
    void OnPoolStart();
}

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton

    public static ObjectPooler Instance;

    #endregion

    public List<Pool> pools;
    public Dictionary<string,List<GameObject>> poolDictionary = new Dictionary<string,List<GameObject>>();
    private Dictionary<string,Transform> poolParents = new Dictionary<string,Transform>();

    private void Awake()
    {
        Instance= this;
    }

    void Start()
    {
        poolDictionary = new Dictionary<string, List<GameObject>>();
        foreach(Pool pool in pools)
        {
            Transform parentTransform = new GameObject(pool.tag).transform;
            parentTransform.parent = transform;
            poolParents.Add(pool.tag, parentTransform);
            List<GameObject> objectPool = new List<GameObject>();
            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.parent= parentTransform;
                objectPool.Add(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    void RegenPools()
    {
        foreach(Pool pool in pools)
        {
            Transform parentTransform = poolParents[pool.tag];
            for(int i = poolDictionary[pool.tag].Count; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.parent = parentTransform;
                poolDictionary[pool.tag].Add(obj);
            }

        }
    }

    public void ResetGO(string tag,GameObject GO)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with Tag " + tag + " doesn't exist");
            return;
        }
        if(!poolDictionary[tag].Contains(GO))
        {
            Debug.LogWarning("Pool with Tag " + tag + " doesn't have the gameObject");
            return;
        }
        GO.SetActive(false);
        GO.transform.parent = poolParents[tag];
    }

    public GameObject SpawnFromPool(string tag,Vector3 pos,Quaternion rot)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with Tag " + tag + " doesn't exist");
            return null;
        }

        GameObject obj = poolDictionary[tag][0];
        poolDictionary[tag].RemoveAt(0);
        obj.transform.position = pos;
        obj.transform.rotation = rot;
        obj.SetActive(true);
        if (obj.TryGetComponent(out IPooledObject pooledObjectScript))
        {
            pooledObjectScript.OnPoolStart();
        }
        poolDictionary[tag].Add(obj);
        return obj;
    }

    public void RemoveFromPool(string tag,GameObject GO)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with Tag " + tag + " doesn't exist");
            return;
        }
        if(!poolDictionary[tag].Contains(GO))
        {
            Debug.LogWarning("Pool with Tag " + tag + " doesn't have the gameObject");
            return;
        }
        poolDictionary[tag].Remove(GO);
        RegenPools();
    }

}
