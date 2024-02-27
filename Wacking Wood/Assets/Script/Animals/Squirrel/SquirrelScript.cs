using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelScript : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform targetObj;
    [SerializeField, Range(0, 90)] private float jumpAngle=45f;
    [SerializeField] private float jumpPower=1f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            MoveToPos(targetObj.position);
        }
    }

    private void MoveToPos(Vector3 targetLoc)
    {
        Vector3 Dir = targetLoc - transform.position;
        float y = Mathf.Tan(Mathf.Deg2Rad * jumpAngle) * Dir.x / Mathf.Cos(Mathf.Atan2(Dir.z, Dir.x));
        Dir = new Vector3(Dir.x, y, Dir.z).normalized;
        rb.AddRelativeForce(Dir);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 Dir = targetObj.transform.position - transform.position;
        float y = Mathf.Tan(Mathf.Deg2Rad * jumpAngle) * Dir.x / Mathf.Cos(Mathf.Atan2(Dir.z, Dir.x));
        Dir = new Vector3(Dir.x,y,Dir.z);
        Gizmos.DrawRay(transform.position, Dir.normalized);
    }
}
