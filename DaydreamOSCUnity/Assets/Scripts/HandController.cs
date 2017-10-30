using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandController : MonoBehaviour 
{
	public static AndroidJavaClass pluginClass;

	public float pitch;
	public float roll;
	public float yaw;

	private float radToDeg = 180.0f / Mathf.PI;

	public  Text winText;

	public GameObject hand;

	void Start()
	{
		pluginClass = new AndroidJavaClass("connerlacy.oscaarplugin.OscAarPlugin");
		pluginClass.CallStatic ("startOSC");
	}

	void FixedUpdate()
	{
		int numMessages = pluginClass.CallStatic<int>("getNumMessages");
		string addr = "";
		float  val  = 0.0f;

		for (int i = 0; i < numMessages; i++)
		{
			addr = pluginClass.CallStatic<string>("getMessageAddress", i);

			if (addr == "/pitch")
			{
				val  = pluginClass.CallStatic<float> ("getMessageFloat", i);
				pitch = val;
			}

			if (addr == "/roll")
			{
				val  = pluginClass.CallStatic<float> ("getMessageFloat", i);
				roll = val;
			}

			if (addr == "/yaw")
			{
				val  = pluginClass.CallStatic<float> ("getMessageFloat", i);
				yaw = val;
			}
		}
			
		pluginClass.CallStatic ("clearMessages");

	}

	void Update()
	{
		transform.eulerAngles = new Vector3 (pitch * radToDeg, yaw * radToDeg, 0);//new Vector3( yaw * radToDeg, 0);
		hand.transform.eulerAngles = new Vector3(pitch * radToDeg, yaw * radToDeg, -roll * radToDeg);
	}
}
