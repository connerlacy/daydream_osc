using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTrigger : MonoBehaviour {

	public HandController handControllerScript;

	/*
	public GameObject index_ps;
	public GameObject middle_ps;
	public GameObject ring_ps;
	public GameObject pinky_ps;
	List<GameObject> ps;
*/
	public int note;
	List<string> names;// = new List<string> (); 

	// Use this for initialization
	void Start () 
	{
		names = new List<string> ();
		names.Add ("TipThumb");
		names.Add ("TipIndex");
		names.Add ("TipMiddle");
		names.Add ("TipRing");
		names.Add ("TipPinky");

		/*
		ps = new List<GameObject> ();
		ps.Add (index_ps);
		ps.Add (middle_ps);
		ps.Add (ring_ps);
		ps.Add (pinky_ps);
		*/
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
		/*
		int i = names.IndexOf (other.name);

		Debug.Log ("Name: " + other.name + " " + i);

		if (i > -1)
		{
			//handControllerScript.keyTrigger (i, 100);

			if (i > 0)
			{
				GameObject go = Instantiate (ps[i -1], other.transform.position, Quaternion.Euler(new Vector3(45,0,0))) as GameObject;
				Destroy (go, 2);
			}
		}
		*/
	}
}
