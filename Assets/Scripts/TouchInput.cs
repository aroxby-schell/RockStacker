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
	
	private bool waiting;
	
	public TouchInput(bool waitForTouchEnd = false)
	{
		waiting = waitForTouchEnd;
	}
	
	public RaycastHit GetTouchWorldPos(int touch, int layerMask)
	{
		if(Application.platform!=RuntimePlatform.Android) return Utils.GetScreenWorldPos(Input.mousePosition, layerMask);
		return Utils.GetScreenWorldPos( Input.GetTouch(GetHardwareID(touch)).position, layerMask);
	}
	
	public int GetTouchCount()
	{
		if(Application.platform==RuntimePlatform.Android) return Input.touchCount;
		if(Input.GetMouseButton(1)) return 2;
		if(Input.GetMouseButton(0)) return 1;
		return 0;
	}
	
	public int GetHardwareID(int touchID)
	{
		for(int i = 0; i<Input.touchCount; i++)
		{
			if(Input.GetTouch(i).fingerId==touchID) return i;
		}
		
		return -1;
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
	
	private void UpdateAndroid(TouchReceiver receiver)
	{
		for(int i = 0; i<Input.touchCount; i++)
		{
			Touch t = Input.GetTouch(i);
			switch(t.phase)
			{
			case TouchPhase.Began:
				if(waiting) break;
				receiver.OnTouchBegin(t.fingerId);
				break;
			case TouchPhase.Moved:
			//case TouchPhase.Stationary:
				if(waiting) break;
				receiver.OnTouchStay(t.fingerId);
				break;
			case TouchPhase.Ended:
			case TouchPhase.Canceled:
				if(waiting)
				{
					waiting = false;
					//HACK, C# Y U no break from switch here?
					return;
				}
				receiver.OnTouchEnd(t.fingerId);
				break;
			}
		}
	}
	
	private void print(object msg)
	{
		Debug.Log(msg);
	}
}
