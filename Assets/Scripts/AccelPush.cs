using UnityEngine;
using System.Collections;

public class AccelPush : MonoBehaviour
{
	public Transform compass;
	public TextMesh magOutput;
	
	private const float keySpeed = 10000f;
	private const float sensorSpeed = keySpeed;
	
	void FixedUpdate()
	{	
		Vector3 push;
		if(Application.platform==RuntimePlatform.WindowsEditor) push = GetKeyboardVector();			
		else push = GetAccelVector();
		
		Push(push);
	}
	
	private Vector3 GetKeyboardVector()
	{
		/*
	 	Vector3 dir = Vector3.zero;
		if(Input.GetKey(KeyCode.W)) dir.z += 1f;
		if(Input.GetKey(KeyCode.S)) dir.z -= 1f;
		if(Input.GetKey(KeyCode.A)) dir.x -= 1f;
		if(Input.GetKey(KeyCode.D)) dir.x += 1f;
		*/
		
		Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
		
		return dir*keySpeed;
	}
	
	private Vector3 GetAccelVector()
	{
		Vector3 inputVec = Input.acceleration;
		Vector3 dir = new Vector3(inputVec.x, 0f, inputVec.y);
		
		//magOutput.text = dir.magnitude.ToString("F2");
		magOutput.text = inputVec.x.ToString("F2") + ", " + inputVec.y.ToString("F2") + ", " + inputVec.z.ToString("F2");
		compass.transform.forward = dir.normalized;
		
		return dir*sensorSpeed;
	}
	
	private void Push(Vector3 vel)
	{
		//rigidbody.velocity = vel * Time.deltaTime;
		rigidbody.velocity = Vector3.zero;
		rigidbody.AddForce(vel * Time.deltaTime * rigidbody.mass);
	}
}
