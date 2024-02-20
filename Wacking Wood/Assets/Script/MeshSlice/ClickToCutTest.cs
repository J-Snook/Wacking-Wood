using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CutMode
{
	Click,
	SetPlane,
	TriPoint
}
public class ClickToCutTest : MonoBehaviour
{
	public CutMode cutMode;
	public GameObject setPlane;
	public Vector3[] triPoints = new Vector3[3];
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
                        Cutter.Cut(victim, hit.point, Vector3.up, interiorMaterial, false);
					}
                    else if(cutMode == CutMode.SetPlane)
                    {
	                    Cutter.Cut(victim, hit.point, Vector3.up, interiorMaterial, true,generatePlaneFromGameObject(victim.transform, hit.point));
                    }
                    else if (cutMode == CutMode.TriPoint)
                    {
	                    Cutter.Cut(victim, hit.point, Vector3.up, interiorMaterial, true,generatePlaneFromVector3());
                    }
				}
			}

		}
	}

    private Plane generatePlaneFromGameObject(Transform victim, Vector3 hitPoint)
    {
	    Transform _trans = setPlane.transform;
	    Vector3 normal = _trans.up;
	    Vector3 pos = victim.InverseTransformPoint(_trans.position);
	    return new Plane(-normal, pos);
    }

    private Plane generatePlaneFromVector3()
    {
	    return new Plane(triPoints[0], triPoints[1], triPoints[2]);
    }
}