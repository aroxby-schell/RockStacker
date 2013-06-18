using UnityEngine;
using System.Collections;

public class EscapeText : MonoBehaviour
{
	private readonly string[] escape = {"\\r", "\\n", "\\t", "\\\\"};
	private readonly string[] characters = {"\r", "\n", "\t", "\\"};
	
	void Start()
	{
		TextMesh mesh = GetComponent<TextMesh>();
		string theText = mesh.text;
		for(int i = 0; i<escape.Length; i++)
		{
			theText = theText.Replace(escape[i], characters[i]);
		}
		mesh.text = theText;
	}
}
