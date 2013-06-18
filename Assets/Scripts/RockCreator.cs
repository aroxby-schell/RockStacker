using UnityEngine;
using System.Collections;

public class RockCreator : MonoBehaviour
{
	public HeightTracker height;
	public Transform stackPlane;
	public Collider gameBounds;
	public TimerProgress rockClock;
	
	public void OnButtonClicked(object o)
	{
		if(o!=gameObject) CreateRock( (GameObject)o );
		else
		{
			//HACK
			GUIButton.SetGUIEnabled(true);
			Application.LoadLevel( Application.loadedLevel );
		}
	}
	
	public void CreateRock(GameObject template)
	{
		GameObject rock = (GameObject)Instantiate(template, transform.position, Quaternion.identity);
		RockController follow = rock.GetComponent<RockController>();
		follow.enabled = true;
		follow.SetFreezeCallBack( ()=>GUIButton.SetGUIEnabled(true) );
		follow.SetZ(stackPlane.position.z);
		follow.SetBounds(gameBounds.bounds);
		follow.SetRestClock(rockClock);
		
		height.SetTracked(rock.transform);
	}
}
