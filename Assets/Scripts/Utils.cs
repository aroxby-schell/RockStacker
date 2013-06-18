using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utils
{
	public static RaycastHit GetScreenWorldPos(Vector3 screenPos, int layerMask)
	{
		screenPos.z = Camera.main.nearClipPlane;
		Ray r = Camera.main.ScreenPointToRay(screenPos);
		RaycastHit hit;
		Physics.Raycast(r, out hit, float.PositiveInfinity, layerMask);
		return hit;
	}
}
