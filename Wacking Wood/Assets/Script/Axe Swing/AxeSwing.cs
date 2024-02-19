using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeSwing : MonoBehaviour
{

    public GameObject axe;

    private float swingCoolDown;
    private bool readyToSwing;
    
    private void Start()
    {
        readyToSwing = true;
        swingCoolDown = 2f;
    }

    // Update is called once per frame
    private void Update()
    {
        Swing();
    }

    private void Swing()
    {
        if (Input.GetMouseButtonDown(0) && readyToSwing)
        {
            StartCoroutine(SwingAnimation());
            readyToSwing = false;
            Invoke(nameof(SwingReload),swingCoolDown);
        }
    }

    IEnumerator SwingAnimation()
    {
        axe.GetComponent<Animator>().Play("AxeSwing");
        yield return new WaitForSeconds(1f);
        axe.GetComponent<Animator>().Play("NewState");
    }

    private void SwingReload()
    {
        readyToSwing = true;
    }
}
