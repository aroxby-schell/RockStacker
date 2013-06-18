#if false
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchInput
{
	public interface TouchReceiver
	{
		void OnTouchBegin(int touchID);
		void OnTouchStay(int touchID);
		void OnTouchEnd(int touchID);
	}
	
	private List<int> touchMap;
	//Track the lowest free value, improves preformance
	private int nextAppID;
	private int touchCount;
	
	private bool waiting;
	
	public TouchInput(bool waitForTouchEnd = false)
	{
		waiting = waitForTouchEnd;
		touchMap = new List<int>();
		nextAppID = 0;
		touchCount = 0;
	}
	
	private int NextID()
	{
		int i = nextAppID;
		for(; i<touchMap.Count; i++)
		{
			if(!touchMap.Contains(i)) break;
		}
		nextAppID = i + 1;
		
		return i;
	}
	
	private void RemoveIndex(int idx)
	{
		touchMap.RemoveAt(idx);
	}
	
	public RaycastHit GetTouchWorldPos(int touch, int layerMask)
	{
		if(Application.platform!=RuntimePlatform.Android) return Utils.GetScreenWorldPos(Input.mousePosition, layerMask);
		return Utils.GetScreenWorldPos( Input.GetTouch(GetHardwareID(touch)).position, layerMask);
	}
	
	public int GetTouchCount()
	{
		if(Application.platform==RuntimePlatform.Android) return touchCount;
		//if(Application.platform==RuntimePlatform.Android) return Input.touchCount;
		if(Input.GetMouseButton(1)) return 2;
		if(Input.GetMouseButton(0)) return 1;
		return 0;
	}
	
	public int GetHardwareID(int touchID)
	{
		return touchMap.IndexOf(touchID);
	}
	
	public void Update(TouchReceiver receiver)
	{
		if(Application.platform==RuntimePlatform.Android) UpdateAndroid(receiver);
		else UpdateDesktop(receiver);
	}
	
	private void UpdateDesktop(TouchReceiver receiver)
	{
		for(int i = 0; i<2; i++)
		{
			if(Input.GetMouseButtonDown(i)) receiver.OnTouchBegin(i);
			if(Input.GetMouseButton(i)) receiver.OnTouchStay(i);
			if(Input.GetMouseButtonUp(i)) receiver.OnTouchEnd(i);
		}
	}
	
	//Even after all this crazyness, it's still not quite right
	private void UpdateAndroid(TouchReceiver receiver)
	{
		int appID;
		
		for(int i = 0; i<Input.touchCount; i++)
		{
			Touch t = Input.GetTouch(i);
			switch(t.phase)
			{
			case TouchPhase.Began:
				if(waiting) break;
				appID = NextID();
				touchMap.Add(appID);
				//print("Begin(b) " + i + ", " + appID + ", " + (touchMap.Count-1) );
				print("Touch: " + t.fingerId);
				touchCount++;
				receiver.OnTouchBegin(appID);
				break;
			case TouchPhase.Moved:
			//case TouchPhase.Stationary:
				if(waiting) break;
				appID = touchMap[i];
				receiver.OnTouchStay(touchMap[i]);
				break;
			case TouchPhase.Ended:
			case TouchPhase.Canceled:
				if(waiting)
				{
					waiting = false;
					//HACK, C# Y U no break from switch here?
					return;
				}
				touchCount--;
				appID = touchMap[i];
				//print("Ended(b) " + i + ", " + appID + ", " + (touchMap.Count-1) );
				receiver.OnTouchEnd(appID);
				if(appID<nextAppID) nextAppID = appID;
				RemoveIndex(i);
				break;
			}
		}
	}
	
	private void print(object msg)
	{
		Debug.Log(msg);
	}
}
#endif