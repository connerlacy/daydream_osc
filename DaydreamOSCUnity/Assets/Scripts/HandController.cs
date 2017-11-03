using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class Finger
{
	public Transform knuckleT;
	public Transform j1T;
	public Transform j2T;
} 


public class HandController : MonoBehaviour 
{
	enum Joints {PINKY_TOP, PINKY_BOTTOM, RING_TOP, RING_BOTTOM, MIDDLE_TOP, MIDDLE_BOTTOM, INDEX_TOP, INDEX_BOTTOM, THUMB_TOP, THUMB_BOTTOM, NUM_JOINTS};

	public static AndroidJavaClass pluginClass;
	private Dictionary<string, int> dictionary;

	public float pitch;
	public float roll;
	public float yaw;

	private Finger thumb;
	private Finger index;
	private Finger middle;
	private Finger ring;
	private Finger pinky;

	List<Finger> fingers;

	private float radToDeg = 180.0f / Mathf.PI;

	public  Text winText;

	public GameObject hand;
	private Vector3 imu;

	void Start()
	{
		dictionary = new Dictionary<string, int> ();

		for (int i = 0; i < (int)Joints.NUM_JOINTS; i++) 
		{
			dictionary.Add ("/" + i, i);
		}
			

		pluginClass = new AndroidJavaClass("connerlacy.oscaarplugin.OscAarPlugin");
		pluginClass.CallStatic ("startOSC");

		fingers = new List<Finger> ();

		pinky = new Finger ();
		pinky.knuckleT = hand.transform.Find("Pinky");
		pinky.j1T = hand.transform.Find("Pinky/Joint0");
		pinky.j2T = hand.transform.Find("Pinky/Joint0/Joint1");
		fingers.Add (pinky);

		ring = new Finger ();
		ring.knuckleT = hand.transform.Find("Ring");
		ring.j1T = hand.transform.Find("Ring/Joint0");
		ring.j2T = hand.transform.Find("Ring/Joint0/Joint1");
		fingers.Add (ring);

		middle = new Finger ();
		middle.knuckleT = hand.transform.Find("Middle");
		middle.j1T = hand.transform.Find("Middle/Joint0");
		middle.j2T = hand.transform.Find("Middle/Joint0/Joint1");
		fingers.Add (middle);

		index = new Finger ();
		index.knuckleT = hand.transform.Find("Index");
		index.j1T = hand.transform.Find("Index/Joint0");
		index.j2T = hand.transform.Find("Index/Joint0/Joint1");
		fingers.Add (index);

		thumb = new Finger ();
		thumb.knuckleT = hand.transform.Find("Thumb");
		thumb.j1T = hand.transform.Find("Thumb/Joint0");
		thumb.j2T = hand.transform.Find("Thumb/Joint0/Joint1");
		fingers.Add (thumb);

		//imu = new Vector3 ();
	}

	void FixedUpdate()
	{
		int numMessages = pluginClass.CallStatic<int>("getNumMessages");
		string addr = "";
		float  val  = 0.0f;

		for (int i = 0; i < numMessages; i++)
		{
			addr = pluginClass.CallStatic<string>("getMessageAddress", i);
			val  = pluginClass.CallStatic<float> ("getMessageFloat", i);

			if (dictionary.ContainsKey (addr)) {
				setFingerValue (dictionary [addr], val);
			}
			else
			{
				if (addr == "/pitch")
				{
					val  = pluginClass.CallStatic<float> ("getMessageFloat", i);
					pitch = val;
					continue;
				}

				if (addr == "/roll")
				{
					val  = pluginClass.CallStatic<float> ("getMessageFloat", i);
					roll = val;
					continue;
				}

				if (addr == "/yaw")
				{
					val  = pluginClass.CallStatic<float> ("getMessageFloat", i);
					yaw = val;
					continue;
				}
			}
		}
			
		pluginClass.CallStatic ("clearMessages");

	}

	void Update()
	{
		hand.transform.localEulerAngles = new Vector3(pitch * radToDeg, yaw * radToDeg, -roll * radToDeg);
	}

	void setFingerValue(int id, float value)
	{
		if (id / 2 == 4)
		{
			//Thumb
		}
		else
		{
			// If odd, it's the knuckle
			if ( (id % 2) == 1) 
			{
				fingers [id / 2].knuckleT.localEulerAngles = new Vector3 (value, 0, 0);
			}
			else
			{
				Vector3 v = new Vector3 (value, 0, 0);
				fingers [id / 2].j1T.localEulerAngles = v;
				fingers [id / 2].j2T.localEulerAngles = v;
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{

		Debug.Log("collission entered");
		foreach (ContactPoint contact in collision.contacts)
		{
			
		}
		//if (collision.relativeVelocity.magnitude > 2)
		//	audioSource.Play();
	}

	public void keyTrigger(int note, int velocity)
	{
		pluginClass.CallStatic ("sendTriggerMessage", note, velocity);
	}
}
