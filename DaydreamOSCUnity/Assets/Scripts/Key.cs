using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

	public HandController handControllerScript;
	public int note;

	// Use this for initialization
	void Start () 
	{
		//handControllerScript = GetComponent<HandController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision)
	{
		//handControllerScript.keyTrigger (31, 100);
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log ("Name: " + other.name);
		if (other.name == "Tip") 
		{
			handControllerScript.keyTrigger (note, 100);
		}

	}

}
