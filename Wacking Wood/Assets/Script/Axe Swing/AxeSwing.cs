using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeSwing : MonoBehaviour
{

    public GameObject axe;

    [SerializeField] private float swingCoolDown=0.5f;
    [SerializeField] private float swingStaminaCost = 10f;
    private bool readyToSwing;
    private bool isSwinging=false;
    public bool CanSwing { get { return readyToSwing; } }
    
    protected Transform player;
    protected PlayerAttributes _player;
    
    private void Start()
    {
        readyToSwing = true;
        
        player = GameObject.Find("Player").transform;
        _player = player.gameObject.GetComponent<PlayerAttributes>();
    }

    // Update is called once per frame
    private void Update()
    {
        Swing();
    }

    private void Swing()
    {
        CanSwingCheck();
        if (Input.GetMouseButtonDown(0) && readyToSwing)
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
