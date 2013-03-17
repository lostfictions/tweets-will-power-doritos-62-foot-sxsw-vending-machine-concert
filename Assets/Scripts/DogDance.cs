using UnityEngine;
using System.Collections;

public class DogDance : MonoBehaviour {


	public Rigidbody[] legs;

	public float torque = 20f;

	public float amp = 0.2f;
	public float period = 1f;

	//uh i guess this was supposed to be FixedUpdate but this gives hilarious results anyway so
	void Update () {

		for (int i = 0; i < 4; ++i) {
			float totalTorque = torque * (Mathf.Sin(Time.time * period) * amp + i/4);	
			legs[i].AddRelativeTorque( Vector3.up * totalTorque, ForceMode.VelocityChange );
		}

	}


}
