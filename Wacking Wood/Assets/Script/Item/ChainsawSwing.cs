using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;

public class ChainsawSwing : MonoBehaviour
{
    [SerializeField] private float swingCoolDown = 0.5f;
    [SerializeField] private float swingFuelCost = 5f;
    public GameObject CS;
    private bool readyToSwing;
    private bool isUsing = false;
    public bool CanSwing { get { return readyToSwing; } }

    private PlayerAttributes _player;
    
    private void Start()
    {
        _player = PlayerAttributes.instance;
    }

    public void Swing()
    {
        CanSwingCheck();
        if(readyToSwing)
        {
            StartCoroutine(SwingAnimation());
            _player.Fuel -= swingFuelCost;
        }
    }
    private void CanSwingCheck()
    {
        readyToSwing = (_player.Fuel >= swingFuelCost) && !isUsing;
    }

    IEnumerator SwingAnimation()
    {
        isUsing = true;
        CS.GetComponent<Animator>().Play("Swing");
        yield return new WaitForSeconds(1f);
        CS.GetComponent<Animator>().Play("Idle");
        yield return new WaitForSeconds(swingCoolDown);
        isUsing = false;
    }
}
