using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;
	private int       count = 0;
	public  float     speed = 10.0f;
	public  Text      countText;
	public  Text      winText;
	public static AndroidJavaClass pluginClass;

	void Start()
	{
		pluginClass = new AndroidJavaClass("connerlacy.oscaarplugin.OscAarPlugin");
		pluginClass.CallStatic ("startOSC");
		rb = GetComponent<Rigidbody> ();
		setCountText ();
		winText.text = "";

	}


	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical   = Input.GetAxis ("Vertical");
		Vector3 movement = new Vector3(moveHorizontal,0.0f,moveVertical);
		rb.AddForce (movement * speed);

		if ((Time.fixedTime / 2.0 % 1) == 0)
		{
			//winText.text = Time.fixedTime.ToString();
			//AndroidJavaClass pluginClass = new AndroidJavaClass("connerlacy.oscaarplugin.OscAarPlugin");
			//pluginClass.CallStatic("printLog");
			//AndroidJavaClass pluginClass = new AndroidJavaClass("connerlacy.oscaarplugin.OscAarPlugin");
			//winText.text = pluginClass.CallStatic<string>("getMessage");
			int numMessages = pluginClass.CallStatic<int>("getNumMessages");
			string addr = "";
			float  val  = 0.0f;

			for (int i = 0; i < numMessages; i++)
			{
				addr = pluginClass.CallStatic<string> ("getMessageAddress", i);
				val  = pluginClass.CallStatic<float> ("getMessageFloat", i);
			}

			winText.text = addr + " " + val.ToString();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("PickUp"))
		{
			other.gameObject.SetActive (false);
			count++;
		}

		setCountText ();
	}

	void setCountText()
	{
		countText.text = "Count: " + count.ToString ();

		if (count >= 10) {
			winText.text = "You win!";
		}
	}
}
