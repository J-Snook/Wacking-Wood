using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeHit : MonoBehaviour,IHitSystem
{
    [SerializeField] private GameObject _leaves;
    [SerializeField] private GameObject _treeTargetPrefab;
    [SerializeField] private GameObject _sliceTargetPrefab;
    [SerializeField] private GameObject _itemLogPrefab;
    public int _treeMaxHealth=1;
    [SerializeField] private float _treeHitTimeoutDiff=30f;
    [SerializeField,Range(0f,360f)] private float _treeTargetHorizontalChange=0.5f;
    [SerializeField] private float _treeTargetVerticalRange = 1f;
    private int _treeHealth;
    private Transform _currentTarget;
    private Rigidbody _treeRigidBody;
    private bool _treeTimeOutActive=false;
    // Start is called before the first frame update
    void Start()
    {
        _treeHealth = _treeMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FallTree()
    {
        //_trunk.transform.localScale = new Vector3(_trunk.transform.localScale.x, _trunk.transform.localPosition.y, _trunk.transform.localScale.z);
        _treeRigidBody =  transform.parent.gameObject.AddComponent<Rigidbody>();
        _treeRigidBody.AddForceAtPosition(-_currentTarget.transform.position, _currentTarget.transform.position);
        FellTreeHit ftH = gameObject.AddComponent<FellTreeHit>();
        ftH.Setup(_sliceTargetPrefab,_itemLogPrefab);
        Destroy(_leaves);
        Destroy(this);
    }

    public void Hit(HitSystem player,RaycastHit target)
{
        if (_currentTarget == null)
        {
            _treeHealth--;
        }
        else if (Vector3.Distance(_currentTarget.position,target.point)<=0.2)
        {
            Destroy(_currentTarget.gameObject);
            _treeHealth--;
        }
        else
        {
            return;
        }

        if (_treeHealth <= 0)
        {
            FallTree();
            StopAllCoroutines();
            _treeTimeOutActive = false;
        }
        else
        {
            if (!_treeTimeOutActive)
            {
                StartCoroutine(HitTimeout(_treeHitTimeoutDiff));
            }
            GameObject _newTarget = Instantiate(_treeTargetPrefab, generateTargetLocation(player.transform.position), Quaternion.identity);
            _newTarget.transform.parent = transform;
            _currentTarget = _newTarget.transform;
        }
    }

    private Vector3 generateTargetLocation(Vector3 hitPos)
    {
        float radius = transform.localScale.x / 2;
        float hitAngle = Vector3.SignedAngle(Vector3.forward, hitPos-transform.position, Vector3.up);
        hitAngle = (hitAngle >0 ) ? hitAngle : 360f+hitAngle;
        hitAngle += Random.Range(_treeTargetHorizontalChange / -2f, _treeTargetHorizontalChange / 2f);
        hitAngle = Mathf.Deg2Rad * hitAngle;
        Vector2 heightRange = new Vector2(hitPos.y - (_treeTargetVerticalRange / 2), hitPos.y + (_treeTargetVerticalRange / 2));
        Vector2 dir = new Vector2(Mathf.Sin(hitAngle), Mathf.Cos(hitAngle));
        Vector2 radiusDir = new Vector2(transform.position.x, transform.position.z) + (dir * radius);
        Vector3 newLocation = new Vector3(radiusDir.x, Random.Range(heightRange.x,heightRange.y), radiusDir.y);
        return newLocation;
    }

    private IEnumerator HitTimeout(float hitTimeout)
    {
        _treeTimeOutActive = true;
        int i = 1;
        while (_treeTimeOutActive)
        {
            yield return new WaitForSeconds(hitTimeout*i);
            if (_treeHealth > 0 && _treeHealth < _treeMaxHealth)
            {
                _treeHealth++;
            } else
            {
                _treeTimeOutActive = false;
            }
            i++;
        }
    }
}
