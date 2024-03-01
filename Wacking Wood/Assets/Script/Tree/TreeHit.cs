using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TreeHit : MonoBehaviour, IHitSystem
{
    [SerializeField] private GameObject _leaves;
    [SerializeField] private GameObject _treeTargetPrefab;
    [SerializeField] private GameObject _sliceTargetPrefab;
    [SerializeField] private GameObject _itemLogPrefab;
    [SerializeField] private float _hitTimeDelay=0.5f;
    public int _treeMaxHealth = 1;
    [SerializeField] private float _treeHitTimeoutDiff = 30f;
    [SerializeField, Range(0f, 360f)] private float _treeTargetHorizontalChange = 0.5f;
    [SerializeField] private float _treeTargetVerticalRange = 1f;
    private int _treeHealth;
    private Transform _currentTarget;
    private AxeSwing axeSwing;
    private bool _treeTimeOutActive = false;
    private bool _isHit = false;
    public Vector3 initPoint;
    // Start is called before the first frame update
    public void ResetTree()
    {
        StopAllCoroutines();
        _treeHealth = _treeMaxHealth;
        _isHit = false;
        _treeTimeOutActive = false;
        if (_currentTarget != null)
        {
            Destroy(_currentTarget.gameObject);
        }
    }

    private void FallTree()
    {
        Vector3 force = new Vector3(transform.position.x,_currentTarget.position.y, transform.position.z);
        force = (force - _currentTarget.transform.position).normalized;
        if(transform.parent.parent.TryGetComponent(out TreeGenerationMesh tgm) && initPoint!=Vector3.zero)
        {
            tgm.RemovePoint(initPoint);
        }
        Rigidbody rb = transform.parent.gameObject.AddComponent<Rigidbody>();
        rb.AddForceAtPosition(-force, _currentTarget.position);
        FellTreeHit ftH = gameObject.AddComponent<FellTreeHit>();
        ftH.Setup(_sliceTargetPrefab, _itemLogPrefab);
        string treeTag = transform.parent.name.Split(':')[0];
        ObjectPooler.Instance.RemoveFromPool(treeTag,transform.parent.gameObject);
        transform.parent.parent = null;
        ObjectManagement.Instance.attachObject(transform.parent.gameObject);
        Destroy(_leaves);
        Destroy(this);
    }

    public void Hit(InteractionSystem player, RaycastHit target, GameObject heldItem)
    {
        axeSwing = player.transform.GetComponentInChildren<AxeSwing>();
        if(heldItem == axeSwing.axe && axeSwing.CanSwing && !_isHit)
        {
            StartCoroutine(AxeHitSwingDelayed(player,target, _hitTimeDelay));
        }
    }

    private Vector3 generateTargetLocation(Vector3 hitPos)
    {
        float radius = transform.localScale.x / 2;
        float hitAngle = Vector3.SignedAngle(Vector3.forward, hitPos - transform.position, Vector3.up);
        hitAngle = (hitAngle > 0) ? hitAngle : 360f + hitAngle;
        hitAngle += Random.Range(_treeTargetHorizontalChange / -2f, _treeTargetHorizontalChange / 2f);
        hitAngle = Mathf.Deg2Rad * hitAngle;
        Vector2 heightRange = new Vector2(hitPos.y - (_treeTargetVerticalRange / 2), hitPos.y + (_treeTargetVerticalRange / 2));
        Vector2 dir = new Vector2(Mathf.Sin(hitAngle), Mathf.Cos(hitAngle));
        Vector2 radiusDir = new Vector2(transform.position.x, transform.position.z) + (dir * radius);
        Vector3 newLocation = new Vector3(radiusDir.x, Random.Range(heightRange.x, heightRange.y), radiusDir.y);
        return newLocation;
    }

    private IEnumerator HitTimeout(float hitTimeout)
    {
        _treeTimeOutActive = true;
        int i = 1;
        while(_treeTimeOutActive)
        {
            yield return new WaitForSeconds(hitTimeout * i);
            if(_treeHealth > 0 && _treeHealth < _treeMaxHealth)
            {
                _treeHealth++;
            } else
            {
                _treeTimeOutActive = false;
            }
            i++;
        }
        Destroy(_currentTarget.gameObject);
    }

    IEnumerator AxeHitSwingDelayed(InteractionSystem player, RaycastHit target,float delay=0.5f)
    {
        _isHit = true;
        yield return new WaitForSeconds(delay);
        if(_currentTarget == null)
        {
            _treeHealth--;
        }
        else if(Vector3.Distance(_currentTarget.position, target.point) <= 0.2)
        {
            Destroy(_currentTarget.gameObject);
            _treeHealth--;
        }
        if (_currentTarget == null || Vector3.Distance(_currentTarget.position, target.point) <= 0.2)
        {
            if(_treeHealth <= 0)
            {
                FallTree();
                StopAllCoroutines();
                _treeTimeOutActive = false;
            }
            else
            {
                if(!_treeTimeOutActive)
                {
                    StartCoroutine(HitTimeout(_treeHitTimeoutDiff));
                }
                GameObject _newTarget = Instantiate(_treeTargetPrefab, generateTargetLocation(player.transform.position), Quaternion.identity);
                _newTarget.transform.parent = transform;
                _currentTarget = _newTarget.transform;
            }
        }
        _isHit = false;
    }
}
