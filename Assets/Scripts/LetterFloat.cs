using UnityEngine;
using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

using System.Collections;

public class LetterFloat : MonoBehaviour
{
	float startTime;

	void Start()
	{
		startTime = Time.time;

		StartCoroutine(PopIn());
	}

	IEnumerator PopIn()
	{
		while(Time.time - startTime < 1.2f)
		{
			transform.localScale = Vector3.one * Easing.EaseOut((Time.time - startTime) / 1.2f, EaseType.Elastic);
			yield return null;
		}
		transform.localScale = Vector3.one;
	}

	void FixedUpdate()
	{
		Vector3 v = transform.position;
		v.y += Mathf.Sin((Time.time - startTime) * (2f)) * 0.008f;
		v += transform.right * Time.deltaTime * 1.6f;
		// v.x -= Time.deltaTime * 1.6f;
		v.z += Mathf.Sin(Time.time - startTime + 0.3f) * 0.004f;

		transform.position = v;
		// rigidbody.MovePosition(v);
	}

	void OnTriggerEnter()
	{
		collider.isTrigger = false;
		rigidbody.isKinematic = false;
		Destroy(this);
	}

	void OnTriggerStay()
	{
		collider.isTrigger = false;
		rigidbody.isKinematic = false;
		Destroy(this);
	}
}
