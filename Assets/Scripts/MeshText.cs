using UnityEngine;
using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class MeshText : MonoBehaviour
{
	public Transform[] letters;

	public string poop = "ABab Hello hello";
	public float relativeTracking = 1.1f;
	public float absoluteTracking = 0.05f;

	void Start()
	{
		float offset = 0;

		for (int i = 0; i < poop.Length; i++)
		{
			Debug.Log(offset);

			if(Char.IsWhiteSpace(poop[i]))
			{
				offset += 1f;
				continue;
			}

			//TODO: check other chartypes

			int letterIndex = (int)Char.ToUpper(poop[i]) - 65;

			if(letterIndex > 25)
				continue;

			Transform t = (Transform)Instantiate(letters[letterIndex], transform.right * -offset, transform.rotation);

			t.parent = transform;

			t.gameObject.name = i.ToString() + t.gameObject.name;

			offset += t.GetComponent<MeshFilter>().mesh.bounds.size.x * relativeTracking + absoluteTracking;

		}

			



	}
	
	void Update()
	{
		
	}
}
