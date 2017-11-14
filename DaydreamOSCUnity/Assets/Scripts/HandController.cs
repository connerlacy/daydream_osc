using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class Finger
{
	public Transform knuckleT;
	public Transform j1T;
	public Transform j2T;
	public bool onOff = false;
	public int index = -1;
} 


public class HandController : MonoBehaviour 
{
	enum Joints {PINKY_TOP, PINKY_BOTTOM, RING_TOP, RING_BOTTOM, MIDDLE_TOP, MIDDLE_BOTTOM, INDEX_TOP, INDEX_BOTTOM, THUMB_TOP, THUMB_BOTTOM, NUM_JOINTS};

	public static AndroidJavaClass pluginClass;
	private Dictionary<string, int> dictionary;
	public WindZone wind;

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

	public float xThresh;

	public GameObject hand;
	private Vector3 imu;

	public GameObject thumb_ps;
	public GameObject index_ps;
	public GameObject middle_ps;
	public GameObject ring_ps;
	public GameObject pinky_ps;
	List<GameObject> ps;

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
		pinky.index = 4;
		pinky.knuckleT = hand.transform.Find("Pinky");
		pinky.j1T = hand.transform.Find("Pinky/Joint0");
		pinky.j2T = hand.transform.Find("Pinky/Joint0/Joint1");
		fingers.Add (pinky);

		ring = new Finger ();
		ring.index = 3;
		ring.knuckleT = hand.transform.Find("Ring");
		ring.j1T = hand.transform.Find("Ring/Joint0");
		ring.j2T = hand.transform.Find("Ring/Joint0/Joint1");
		fingers.Add (ring);

		middle = new Finger ();
		middle.index = 2;
		middle.knuckleT = hand.transform.Find("Middle");
		middle.j1T = hand.transform.Find("Middle/Joint0");
		middle.j2T = hand.transform.Find("Middle/Joint0/Joint1");
		fingers.Add (middle);

		index = new Finger ();
		index.index = 1;
		index.knuckleT = hand.transform.Find("Index");
		index.j1T = hand.transform.Find("Index/Joint0");
		index.j2T = hand.transform.Find("Index/Joint0/Joint1");
		fingers.Add (index);

		thumb = new Finger ();
		thumb.index = 0;
		thumb.knuckleT = hand.transform.Find("Thumb");
		thumb.j1T = hand.transform.Find("Thumb/Joint0");
		thumb.j2T = hand.transform.Find("Thumb/Joint0/Joint1");
		fingers.Add (thumb);

		ps = new List<GameObject> ();
		ps.Add (thumb_ps);
		ps.Add (index_ps);
		ps.Add (middle_ps);
		ps.Add (ring_ps);
		ps.Add (pinky_ps);

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

			if (dictionary.ContainsKey (addr)) 
			{
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
					wind.windTurbulence = val * 20.0f;
					yaw = val;
					continue;
				}
			}
		}
			
		pluginClass.CallStatic ("clearMessages");

	}

	void Update()
	{
		//this.transform.eulerAngles = new Vector3(-roll * radToDeg, pitch * radToDeg, yaw * radToDeg );
		this.transform.rotation = Quaternion.Euler ( pitch * radToDeg, yaw * radToDeg, -roll * radToDeg);
	}

	void setFingerValue(int id, float value)
	{
		// Set values
		if (id / 2 == 4)
		{
			//Thumb
			// If odd, it's the knuckle
			if ( (id % 2) == 1) 
			{
				fingers [id / 2].knuckleT.localEulerAngles = new Vector3 (0, value / 10.0f, 0);
			}
			else
			{
				Vector3 v = new Vector3 (0, value / 2.0f, 0);
				fingers [id / 2].j1T.localEulerAngles = v;
				fingers [id / 2].j2T.localEulerAngles = v;
			}
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

		// Check our values
		foreach (var f in fingers)
		{
			float xSum;//= f.knuckleT.localEulerAngles.x + f.j1T.localEulerAngles.x + f.j2T.localEulerAngles.x;

			if (f.index == 0) 
			{ //Thumb
				xSum = f.knuckleT.localEulerAngles.y + f.j1T.localEulerAngles.y + f.j2T.localEulerAngles.y;	
			}
			else
			{
				xSum = f.knuckleT.localEulerAngles.x + f.j1T.localEulerAngles.x + f.j2T.localEulerAngles.x;
			}

			if (!f.onOff && xSum > xThresh) {
				f.onOff = true;
				keyTrigger (f.index, 100);
			} 
			else if (f.onOff && xSum < (xThresh - 10))
			{
				f.onOff = false;
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
		//if (note < 4)
		{
			GameObject go = Instantiate (ps[note], fingers[fingers.Count - (note + 1)].j2T.position, Quaternion.Euler(new Vector3(45,0,0))) as GameObject;
			Destroy (go, 2);
		}
	}
}
