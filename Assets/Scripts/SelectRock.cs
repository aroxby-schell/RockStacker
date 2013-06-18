using UnityEngine;
using System.Collections;

public class SelectRock : MonoBehaviour
{
	public Transform pointSphere;
	public Transform centerSphere;
	
	private int layerMask;
	private float resetTime = float.PositiveInfinity;
	private Vector3 zAdjust = Vector3.forward * -1f;
	private RockController lastSelection = null;
	
	public static Vector3 rotationAxis = Vector3.right;

	void Start()
	{
		layerMask = 1<<LayerMask.NameToLayer("Stackable Rocks");
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			if(rotationAxis==Vector3.right) rotationAxis = Vector3.forward;
			else rotationAxis = Vector3.right;
		}
		
		if(Input.GetMouseButtonDown(2))
		{
			RaycastHit hit = Utils.GetScreenWorldPos(Input.mousePosition, layerMask);
			pointSphere.position = hit.point+zAdjust;
			try
			{
				centerSphere.position = hit.transform.position+zAdjust;
			}
			catch(System.NullReferenceException)
			{
			}
			
			RockController thisSelection = hit.transform.GetComponent<RockController>();
			if(lastSelection!=null) lastSelection.enabled = false;
			lastSelection = thisSelection;
			if(lastSelection!=null) lastSelection.enabled = true;
			
			resetTime = Time.timeSinceLevelLoad + 0.5f;
		}
		
		if(resetTime<=Time.timeSinceLevelLoad)
		{
			ResetPoints();
		}
	}
	
	private void ResetPoints()
	{
		Vector3 pos = transform.position;
		centerSphere.transform.position = pos;
		pointSphere.transform.position = pos;
		resetTime = float.PositiveInfinity;
	}
}
