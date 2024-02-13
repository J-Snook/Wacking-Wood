using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FellTreeHit : MonoBehaviour
{
    private GameObject _sliceTargetPrefab;
    private GameObject _itemLogPrefab;
    private GameObject _slice;
    private float _length;
    private float _radius;
    // Start is called before the first frame update
    public void Setup(GameObject targetPrefab,GameObject logPrefab)
    {
        _sliceTargetPrefab = targetPrefab;
        _itemLogPrefab = logPrefab;
        _radius = transform.localScale.x;
        GenerateSliceTarget();
        transform.localScale = new Vector3(transform.localScale.x, transform.localPosition.y, transform.localScale.x);
    }

    void GenerateSliceTarget()
    {
        _length = transform.localScale.y;
        if(_length > 1)
        {
            Vector3 sliceLocalPos = new Vector3(0, (2f / _length) - 1f, 0);
            _slice = Instantiate(_sliceTargetPrefab, Vector3.zero, transform.rotation, transform);
            _slice.transform.localPosition = sliceLocalPos;
            _slice.transform.localScale = new Vector3(1.01f, (1/_length)*_slice.transform.localScale.y, 1.01f);
        }
        else
        {
            GameObject log = Instantiate(_itemLogPrefab, transform.position, transform.rotation);
            log.transform.localScale = new Vector3(_radius, 1f, _radius);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SliceHit()
    {
        Destroy(_slice);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - 1f, transform.localScale.z);
        Vector3 logPosition = transform.position + (transform.up * ((_length*-1f)+1f));
        transform.localPosition = new Vector3(0, transform.localPosition.y+1.05f,0);
        GameObject log = Instantiate(_itemLogPrefab, logPosition, transform.rotation);
        log.transform.localScale = new Vector3(_radius, 1f, _radius);
        GenerateSliceTarget();
    }
}
