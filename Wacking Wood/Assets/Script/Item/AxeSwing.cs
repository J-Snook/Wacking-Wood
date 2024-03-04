using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeSwing : MonoBehaviour
{

    public GameObject axe;

    [SerializeField] private float swingCoolDown=0.5f;
    [SerializeField] private float swingStaminaCost = 5f;
    private bool readyToSwing;
    private bool isSwinging=false;
    public bool CanSwing { get { return readyToSwing; } }
    private PlayerAttributes _player;

    
    
    
    
    private void Start()
    {
        _player = PlayerAttributes.instance;
    }

    public void Swing()
    {
        CanSwingCheck();
        if (readyToSwing)
        {
            StartCoroutine(SwingAnimation());
            _player.Stamina -= swingStaminaCost;
        }
    }

    private void CanSwingCheck()
    {
        readyToSwing = (_player.Stamina >= swingStaminaCost) && !isSwinging;
    }

    IEnumerator SwingAnimation()
    {
        isSwinging = true;
        axe.GetComponent<Animator>().Play("AxeSwing");
        yield return new WaitForSeconds(1f);
        axe.GetComponent<Animator>().Play("NewState");
        yield return new WaitForSeconds(swingCoolDown);
        isSwinging = false;
    }
}
