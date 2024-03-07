using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CutMode
{
	Click,
	SetPlane
}
public class ClickToCutTest : MonoBehaviour
{
	public CutMode cutMode;
	public GameObject setPlane;
	public Material interiorMaterial;

    void Update(){

		if(Input.GetMouseButtonDown(0)){
			RaycastHit hit;

			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)){
				GameObject victim = hit.collider.gameObject;
				if(victim.tag != "Safe")
				{
                   
                    if(cutMode == CutMode.Click)
					{
                        Cutter.Cut(victim, hit.point, interiorMaterial, false);
					}
                    else if(cutMode == CutMode.SetPlane)
                    {
	                    Cutter.Cut(victim, hit.point, interiorMaterial, true,generatePlaneFromGameObject(victim.transform, hit.point));
                    }
				}
			}
		}
		if (Input.GetMouseButtonDown(1))
		{
			cutMode = cutMode == CutMode.Click ? CutMode.SetPlane : CutMode.Click;
		}
	}

    /// <summary>
    /// Turn a game object plane into the Plane type
    /// </summary>
    /// <param name="victim">What are we hitting</param>
    /// <param name="hitPoint">Where did we hit</param>
    /// <returns></returns>
    private Plane generatePlaneFromGameObject(Transform victim, Vector3 hitPoint)
    {
	    Transform _trans = setPlane.transform;
	    Vector3 normal = _trans.up;
	    Vector3 pos = victim.InverseTransformPoint(_trans.position);
	    return new Plane(-normal, pos);
    }
}