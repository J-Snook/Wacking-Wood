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
            //Animation Needs to go here for the swing
            Ray r = new Ray(_camera.position+_camera.forward,_camera.forward);
            if(Physics.Raycast(r,out RaycastHit hit,_hitSphereRange))
            {
                if(hit.collider.TryGetComponent(out IHitSystem hitSystem)) 
                {
                    hitSystem.Hit(this, hit);
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
