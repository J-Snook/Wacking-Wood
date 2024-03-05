using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainsawSwing : MonoBehaviour
{
    public GameObject CS;

    private float swingCoolDown;
    private bool readyToSwing;
    
    protected Transform player;
    protected PlayerAttributes _player;
    
    private void Start()
    {
        readyToSwing = true;
        swingCoolDown = 2f;
        
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
        if (Input.GetMouseButtonDown(0) && readyToSwing && _player.Stamina > 1)
        {
            StartCoroutine(SwingAnimation());
            _player.Stamina -= 20f; 
            _player.RefillTime = 3.0f;
            readyToSwing = false;
            Invoke(nameof(SwingReload),swingCoolDown);
        }
    }

    IEnumerator SwingAnimation()
    {
        CS.GetComponent<Animator>().Play("ChainSwing");
        yield return new WaitForSeconds(1f);
        CS.GetComponent<Animator>().Play("NewState");
    }

    private void SwingReload()
    {
        readyToSwing = true;
    }
}
