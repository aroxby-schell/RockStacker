using UnityEngine;
using System.Collections;

public class HeightTracker : MonoBehaviour
{	
	public TextMesh heightText;
	public Transform ground;
	
	private Transform trackedRock = null;
	
	private float towerMaxHeight = float.NegativeInfinity;
	private float groundHeight;
	
	void Start()
	{
		groundHeight = ground.collider.bounds.max.y;
	}
	
	public void SetTracked(Transform t)
	{
		if(trackedRock)
		{
			float rockMaxHeight = getRockHeight();
			if(rockMaxHeight>towerMaxHeight) towerMaxHeight = rockMaxHeight;
		}
		
		trackedRock = t;
	}
	
	void Update()
	{
		float heightToDisplay = towerMaxHeight;
		
		if(trackedRock)
		{
			float rockMaxHeight = getRockHeight();
			if(rockMaxHeight>heightToDisplay) heightToDisplay = rockMaxHeight;
		}
		
		float height = (heightToDisplay-groundHeight);
		if(float.IsInfinity(height)) height = 0f;
		heightText.text = "Height: " + height.ToString("F1") + "m";
	}
	
	private float getRockHeight()
	{
		return trackedRock.collider.bounds.max.y;
	}
}
