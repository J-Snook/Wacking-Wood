using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitSystem
{
    public void Hit(HitSystem player,RaycastHit hit);
}

public class HitSystem : MonoBehaviour
{
    [SerializeField] private float _hitSphereRange;
    [SerializeField] private Transform _camera;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("1");
            //Animation Needs to go here for the swing
            Ray r = new Ray(_camera.position+_camera.forward,_camera.forward);
            if(Physics.Raycast(r,out RaycastHit hit,_hitSphereRange))
            {
                Debug.Log("2");
                if(hit.transform.parent!= null)
                {
                    Debug.Log("3");
                    if(hit.transform.parent.TryGetComponent(out IHitSystem hitSystem)) 
                    {
                        Debug.Log("4");
                        hitSystem.Hit(this, hit);
                    }
                    else if (hit.transform.parent.parent != null)
                    {
                        Debug.Log("5");
                        if(hit.transform.parent.parent.TryGetComponent(out IHitSystem fellTreeHit))
                        {
                            Debug.Log("6");
                            fellTreeHit.Hit(this, hit);
                        }
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_camera.position + _camera.forward, _camera.position+(_camera.forward+_camera.forward*_hitSphereRange));
    }
}
