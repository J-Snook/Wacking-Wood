using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FellTreeHit : MonoBehaviour, IHitSystem
{
    private GameObject _sliceTargetPrefab;
    private GameObject _trunk;
    private GameObject _slice;
    private float _length;
    // Start is called before the first frame update
    public void Setup(GameObject trunk,GameObject targetPrefab)
    {
        _trunk = trunk;
        _sliceTargetPrefab = targetPrefab;
        GenerateSliceTarget();
    }

    void GenerateSliceTarget()
    {
        _length = _trunk.transform.localPosition.y + 0.5f;
        if(_length > 1)
        {
            Vector3 sliceLocalPos = new Vector3(0, (2f / _length) - 1f, 0);
            _slice = Instantiate(_sliceTargetPrefab, Vector3.zero, _trunk.transform.rotation, _trunk.transform);
            _slice.transform.localPosition = sliceLocalPos;
            _slice.transform.localScale = new Vector3(1.01f, _slice.transform.localScale.y, 1.01f);
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(HitSystem player, RaycastHit hit)
    {
        Debug.Log("TEST2");
        if (hit.transform==_slice.transform )
        {
            Destroy(_slice);
            _trunk.transform.localScale = new Vector3(_trunk.transform.localScale.x, _trunk.transform.localScale.y - 1.125f, _trunk.transform.localScale.z);
            GenerateSliceTarget();
        }
    }
}
