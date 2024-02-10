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
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Animation Needs to go here for the swing
            Ray r = new Ray(transform.position,transform.forward);
            if(Physics.Raycast(r,out RaycastHit hit,_hitSphereRange))
            {
                if(hit.transform.parent.TryGetComponent(out IHitSystem hitSystem))
                {
                    hitSystem.Hit(this,hit);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position,transform.position+(transform.forward*_hitSphereRange));
    }
}
