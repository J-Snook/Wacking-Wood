using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FellTreeHit : MonoBehaviour
{
    private GameObject _sliceTargetPrefab;
    private GameObject _itemLogPrefab;
    private GameObject _slice;
    private float _length;
    private float _radius;
    private int _hitCount;
    private AxeSwing axeSwing;
    private ChainsawSwing csSwing;
    private bool _isHit;
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
            _hitCount= 0;
        }
        else
        {
            GameObject log = Instantiate(_itemLogPrefab, transform.position, transform.rotation);
            log.transform.localScale = new Vector3(_radius, 1f, _radius);
            ObjectManagement.Instance.detachObject(transform.parent.gameObject);
            Destroy(transform.parent.gameObject);
        }
    }

    private void logGenFromHits()
    {
        Destroy(_slice);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - 1f, transform.localScale.z);
        Vector3 logPosition = transform.position + (transform.up * ((_length * -1f) + 1f));
        transform.localPosition = new Vector3(0, transform.localPosition.y + 1.05f, 0);
        GameObject log = Instantiate(_itemLogPrefab, logPosition, transform.rotation);
        log.transform.localScale = new Vector3(_radius, 1f, _radius);
        GenerateSliceTarget();
    }

    public void SliceHit(InteractionSystem player,GameObject heldItem)
    {
        if(axeSwing == null)
        {
            axeSwing = player.transform.GetComponent<AxeSwing>();
        }
        if(csSwing == null)
        {
            csSwing = player.transform.GetComponent<ChainsawSwing>();
        }
        if(!_isHit)
        {
            if(heldItem == axeSwing.axe && axeSwing.CanSwing)
            {
                StartCoroutine(axeSwingDelay());
            }
            else if(heldItem == csSwing.CS && csSwing.CanSwing)
            {
                StartCoroutine(chainsawDelay());
            }
        }
    }

    IEnumerator axeSwingDelay(float delay=0.5f)
    {
        _isHit = true;
        yield return new WaitForSeconds(delay);
        if(_hitCount>=1)
        {
            logGenFromHits();
        } else
        {
            _hitCount++;
        }
        _isHit= false;
    }

    IEnumerator chainsawDelay(float delay = 0.5f)
    {
        _isHit = true;
        yield return new WaitForSeconds(delay);
        logGenFromHits();
        _isHit = false;
    }
}
