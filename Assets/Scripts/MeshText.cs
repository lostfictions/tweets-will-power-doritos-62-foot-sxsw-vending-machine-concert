using UnityEngine;
using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class MeshText : MonoBehaviour
{
	public Transform[] letters;
	public Transform period;

	public float relativeTracking = 1.1f;
	public float absoluteTracking = 0.05f;

	void Start()
	{
	}
	
	public void CreateMeshText(string text)
	{
		Transform wrapper = new GameObject(text).transform;
		wrapper.parent = transform;

		float offset = 0;

		for (int i = 0; i < text.Length; i++)
		{
			if(Char.IsWhiteSpace(text[i]))
			{
				offset += 1f;
				continue;
			}
			// TODO: special-casing like this isn't going to be sustainable, break this out
			else if(text[i] == '.')
			{
				Transform p = (Transform)Instantiate(period, transform.position + transform.right * -offset, transform.rotation);
				p.parent = wrapper;
				p.gameObject.name = i.ToString() + p.gameObject.name;
				offset += p.GetComponentInChildren<MeshFilter>().mesh.bounds.size.x * relativeTracking + absoluteTracking;

				continue;
			}
			
			//TODO: check other chartypes 
			//incl. newline 


			int letterIndex = (int)Char.ToUpper(text[i]) - 65;

			if(letterIndex > 25 || letterIndex < 0)
				continue;

			Transform t = (Transform)Instantiate(letters[letterIndex], transform.position + transform.right * -offset, transform.rotation);

			t.parent = wrapper;

			t.gameObject.name = i.ToString() + t.gameObject.name;

			offset += t.GetComponent<MeshFilter>().mesh.bounds.size.x * relativeTracking + absoluteTracking;
		}

			
	}
}
