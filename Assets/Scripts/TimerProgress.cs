using UnityEngine;
using System.Collections;

public class TimerProgress : MonoBehaviour
{
	public Color startColor;
	public Color endColor;
	public Transform emptyBar;
	public Transform fullBar;
	
	private float duration = 0f;
	private float startTime = 0f;
	
	public void ResetTime(float time)
	{
		duration = time;
		startTime = Time.timeSinceLevelLoad;
	}
	
	public void SetProgress(float time)
	{
		startTime = Time.timeSinceLevelLoad - time;
	}
	
	void Update()
	{
		float t = (Time.timeSinceLevelLoad-startTime)/duration;
		t = Mathf.Clamp(t, 0f, 1f);
		
		Color cur = Color.Lerp(startColor, endColor, t);
		SetColor(cur);
		
		Vector3 scale = emptyBar.localScale;
		float newX = scale.x * t;
		//Seriously, unity, CHILL OUT!!
		if( float.IsInfinity(newX) || float.IsNaN(newX) ) newX = 0f;
		scale.x = newX;
		fullBar.localScale = scale;
		
		newX = emptyBar.position.x + emptyBar.localScale.x/2f - newX/2f;
		Vector3 pos = emptyBar.localPosition;
		pos.x = newX;
		pos.z -= 0.01f;
		fullBar.localPosition = pos;
	}
	
	private void SetColor(Color c)
	{
		renderer.material.SetColor("_Emission", c);
	}
}