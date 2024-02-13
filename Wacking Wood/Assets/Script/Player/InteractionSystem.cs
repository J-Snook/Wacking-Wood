using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitSystem
{
    public void Hit(InteractionSystem player, RaycastHit hit);
}
interface IInteractSystem
{
    public string promptText { get; }
    public void Interact(InteractionSystem player);
}

public class InteractionSystem : MonoBehaviour
{
    [SerializeField] private float _hitRange;
    [SerializeField] private float _interactionRange = 3f;
    [SerializeField] private float _distanceFromCamera = 1f;
    [SerializeField] private Transform _camera;
    [SerializeField] private PlayerUI _scriptPlayerUI;
    private bool _canSeePrompt;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_canSeePrompt)
        {
            _canSeePrompt = _scriptPlayerUI.SetInteractionPrompt(string.Empty);
        }
        Ray r = new Ray(_camera.position + (_camera.forward * _distanceFromCamera), _camera.forward);
        if (Input.GetMouseButtonDown(0))
        {
            //Animation Needs to go here for the swing
            if (Physics.Raycast(r, out RaycastHit hit, _hitRange))
            {
                if (hit.collider.TryGetComponent(out IHitSystem hitSystem))
                {
                    hitSystem.Hit(this, hit);
                }
            }
        }
        if (Physics.Raycast(r, out RaycastHit interactHit, _interactionRange))
        {
            if (interactHit.transform.TryGetComponent(out IInteractSystem interactSystem))
            {
                _canSeePrompt = _scriptPlayerUI.SetInteractionPrompt(interactSystem.promptText);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    interactSystem.Interact(this);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_camera.position + (_camera.forward * _distanceFromCamera), _camera.position + (_camera.forward * _distanceFromCamera) + (_camera.forward * _hitRange));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(_camera.position + (_camera.forward * _distanceFromCamera), _camera.position + (_camera.forward * _distanceFromCamera) + (_camera.forward * _interactionRange));
    }
}
