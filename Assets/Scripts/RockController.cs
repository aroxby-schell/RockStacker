using UnityEngine;
using System.Collections;

public class RockController : MonoBehaviour, TouchInput.TouchReceiver
{	
	private Vector3[] lastPos = new Vector3[2];
	private int layerMask;
	private bool dragging = false;
	private bool frozen = false;
	private  float freezeTime = float.PositiveInfinity;
	private TouchInput touchInput = new TouchInput(false);
	
	private float stillTime = 0f;
	private const float timeToFreeze = 2f;
	
	public delegate void FreezeCallBack();
	private FreezeCallBack freezeCallBack = null;
	
	private Bounds gameBounds;
	
	private const float accelSensitivity = 1000f;
	private Vector3 lastGrav = Vector3.zero;
	
	private TimerProgress restClock = null;
	
	void Start()
	{
		layerMask = 1<<LayerMask.NameToLayer("Rock Placement Z Plane");		
		rigidbody.useGravity = !dragging;
		lastGrav = Input.acceleration;
	}
	
	public void SetZ(float z)
	{
		Vector3 position = transform.position;
		position.z = z;
		transform.position = position;
	}
	
	public void SetFreezeCallBack(FreezeCallBack cb)
	{
		freezeCallBack = cb;
	}
	
	public void SetBounds(Bounds b)
	{
		gameBounds = b;
	}
	
	public void SetRestClock(TimerProgress clock)
	{
		restClock = clock;
		restClock.ResetTime(timeToFreeze);
	}
	
	void FixedUpdate()
	{
		if(frozen)
		{
			return;
		}
		
		if(dragging)
		{
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
			stillTime = 0f;
			if(restClock) restClock.ResetTime(timeToFreeze);
		}
		else if(rigidbody.velocity.sqrMagnitude <= 0.01f)
		{
			stillTime += Time.deltaTime;
			if(stillTime >= timeToFreeze)
			{
				SetFrozen(true);
				if(freezeCallBack!=null) freezeCallBack();
				rigidbody.angularVelocity = Vector3.zero;
				rigidbody.velocity = Vector3.zero;
			}
		}
		else
		{
			stillTime -= Time.deltaTime;
			if(stillTime<0f) stillTime = 0f;
			if(restClock) restClock.SetProgress(stillTime);
		}
	}
	
	private Vector3 GetTouchWorldPos(int touch)
	{
		return touchInput.GetTouchWorldPos(touch, layerMask).point;
	}
	
	private void SetDragging(bool isDragging)
	{
		dragging = isDragging;
		rigidbody.useGravity = !dragging;
	}
	
	private Vector3 updateDragDistance(int touch)
	{
		Vector3 followPos = GetTouchWorldPos(touch);
		Vector3 dist = followPos - lastPos[touch];
		lastPos[touch] = followPos;
		return dist;
	}
	
	public void SetFrozen(bool freeze)
	{
		frozen = freeze;
	}
	
	void Update()
	{
		if(frozen) return;
		
		touchInput.Update(this);
		
		Vector3 gravDiff = Input.acceleration - lastGrav;
		
		Vector3 movement = new Vector3(gravDiff.y, 0f, -gravDiff.x)*accelSensitivity;
		transform.Rotate(movement*Time.deltaTime, Space.World);
		lastGrav = Input.acceleration;
		
		if(freezeTime<=Time.timeSinceLevelLoad)
		{
			SetFrozen(true);
		}
	}
	
	public void OnTouchBegin(int touchID)
	{
		if(touchID>=2) return;
		SetDragging(true);
		lastPos[touchID] = GetTouchWorldPos(touchID);
	}
	
	public void OnTouchStay(int touchID)
	{
		if(touchID>=2) return;
		Vector3 dist = updateDragDistance(touchID);
		if(touchID==0)
		{
			Bounds myBounds = collider.bounds;
			Vector3 maxPos = myBounds.max + dist;
			Vector3 minPos = myBounds.min + dist;
			
			if( gameBounds.Contains(maxPos) && gameBounds.Contains(minPos) )
				transform.Translate(dist, Space.World);
		}
		else if(touchID==1)
		{
			transform.RotateAround(Vector3.up, -dist.x);
			//transform.RotateAround(Vector3.right, dist.y);
			transform.RotateAround(Vector3.forward, dist.y);
		}
	}
	
	public void OnTouchEnd(int touchID)
	{
		int tc = touchInput.GetTouchCount();
		if(tc<=1) SetDragging(false);
	}
}
