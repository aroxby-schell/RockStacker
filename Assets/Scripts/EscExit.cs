using UnityEngine;
using System.Collections;

public class EscExit : MonoBehaviour
{
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			//if(Application.isEditor) UnityEditor.EditorApplication.isPlaying = false;
			Application.Quit();
		}
	}
}
