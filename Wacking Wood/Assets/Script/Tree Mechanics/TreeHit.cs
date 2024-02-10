using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeHit : MonoBehaviour,IHitSystem
{
    [SerializeField] private Transform _trunk;
    [SerializeField] private GameObject _leaves;
    [SerializeField] private GameObject _treeTargetPrefab;
    public int _treeMaxHealth=1;
    [SerializeField] private float _treeHitTimeoutDiff=5f;
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
        Destroy(_leaves);
        _treeRigidBody =  gameObject.AddComponent<Rigidbody>();
        _treeRigidBody.AddForceAtPosition(-_currentTarget.transform.position, _currentTarget.transform.position);

        Debug.Log("Tree Fell");
    }

    public void Hit(HitSystem player,RaycastHit target)
    {
        if (_currentTarget == null)
        {
            _treeHealth--;
        }
        else if (target.transform==_currentTarget)
        {
            Destroy(_currentTarget.gameObject);
            _treeHealth--;
        }
        else
        {
            return;
        }
        if ( _treeHealth <= 0)
        {
            FallTree();
            StopAllCoroutines();
            _treeTimeOutActive = false;
        }
        else
        {
            float timeoutWait = _treeHitTimeoutDiff * (_treeMaxHealth - _treeHealth);
            if (!_treeTimeOutActive)
            {
                StartCoroutine(HitTimeout(timeoutWait));
            }
            GameObject _newTarget = Instantiate(_treeTargetPrefab,generateTargetLocation(player.transform.position),Quaternion.identity);
            _newTarget.transform.parent = transform;
            _currentTarget = _newTarget.transform;
        }
        
    }

    private Vector3 generateTargetLocation(Vector3 hitPos)
    {
        float radius = _trunk.localScale.x / 2;
        float hitAngle = Vector3.SignedAngle(Vector3.forward, hitPos-transform.position, Vector3.up);
        hitAngle = (hitAngle >0 ) ? hitAngle : 360f+hitAngle;
        hitAngle += Random.Range(_treeTargetHorizontalChange / -2f, _treeTargetHorizontalChange / 2f);
        hitAngle = Mathf.Deg2Rad * hitAngle;
        Vector2 heightRange = new Vector2(hitPos.y - (_treeTargetVerticalRange / 2), hitPos.y + (_treeTargetVerticalRange / 2));
        Vector2 dir = new Vector2(Mathf.Sin(hitAngle), Mathf.Cos(hitAngle));
        Vector2 radiusDir = new Vector2(_trunk.position.x, _trunk.position.z) + (dir * radius);
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