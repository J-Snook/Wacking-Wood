using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractSystem
{
    public string promptText { get; }
    public void Interact(InteractionSystem player);
}

public class InteractionSystem : MonoBehaviour
{
    [SerializeField] private float _interactionRange=3;
    [SerializeField] private float _interactionSphereRadius;
    [SerializeField] private GameObject _debugSphere;
    private RaycastHit[] _interactionTargets;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.SphereCast(transform.position,_interactionSphereRadius,transform.forward,out RaycastHit hit,_interactionRange))
        {
            if(hit.transform.TryGetComponent(out IInteractSystem interactSystem))
            {
                if (interactSystem.promptText != string.Empty)
                {
                    Debug.Log(interactSystem.promptText);
                }
                if (Input.GetKeyDown(KeyCode.F))
                {
                    interactSystem.Interact(this);
                }
            }
            if (hit.transform == transform)
            {
                Debug.Log("Self");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position+transform.forward*(_interactionRange + _interactionSphereRadius));
    }
}
