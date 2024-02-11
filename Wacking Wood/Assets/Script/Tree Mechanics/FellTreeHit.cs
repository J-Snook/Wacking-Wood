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
        _length = Mathf.FloorToInt(_trunk.transform.localPosition.y);
        if ( _length > 1 )
        {
            Vector3 sliceLocalPos = new Vector3(0, (2f / _length) - 1, 0);
            _slice = Instantiate(_sliceTargetPrefab, sliceLocalPos+transform.position, transform.rotation, transform);
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
        throw new System.NotImplementedException();
    }
}
