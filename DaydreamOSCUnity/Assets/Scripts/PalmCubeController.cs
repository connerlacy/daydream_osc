using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PalmCubeController : MonoBehaviour 
{
	public static AndroidJavaClass pluginClass;

	public float pitch;
	public float roll;
	public float yaw;

	public  Text winText;

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
				winText.text = addr + " " + val.ToString();
				break;
			}
		}



		pluginClass.CallStatic ("clearMessages");



	}

	void Update()
	{
		transform.eulerAngles = new Vector3(0, 0, pitch * 180.0f);

	}
		
}
