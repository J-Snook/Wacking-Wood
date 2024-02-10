using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGrowth : MonoBehaviour
{
    [SerializeField, Range(0f, 6f)] private float _treeStage;
    [SerializeField] private GameObject _trunk;
    [SerializeField] private GameObject _leaves;
    private TreeHit _treeHit;
    // Start is called before the first frame update
    void Start()
    {
        _treeHit = gameObject.GetComponent<TreeHit>();
        if (_treeStage == 0)
        {
            _treeStage = Random.Range(4f, 6f);
        }
        SetTreeStage(_treeStage);
        //StartCoroutine(Colerps());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetTreeStage(float treeStage)
    {
        _trunk.transform.localScale = new Vector3 (Mathf.Pow(treeStage,0.25f), treeStage+0.5f, Mathf.Pow(treeStage, 0.25f));
        _trunk.transform.localPosition = new Vector3(_trunk.transform.localPosition.x, treeStage, _trunk.transform.localPosition.z);
        _leaves.transform.localScale = new Vector3(treeStage+1f,treeStage,treeStage+1f);
        _leaves.transform.localPosition = new Vector3(_leaves.transform.localPosition.x, 3f * treeStage, _leaves.transform.localPosition.z);
        _treeHit._treeMaxHealth = Mathf.CeilToInt(treeStage) + 1;
    }
}
