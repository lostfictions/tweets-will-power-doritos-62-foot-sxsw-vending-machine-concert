using UnityEngine;
using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class TextBark : MonoBehaviour
{
	static Dictionary<char, GameObject> charMeshMap;
	static TweetQueue tq;

	public float tracking = 0.35f;
	public float charDelay = 0.35f;

	Queue<char> tweet = new Queue<char>();

	float curCharDelay = 0;

	Transform letterParent;

	void Start()
	{
		letterParent = new GameObject("Letters").transform;

		if(tq == null)
		{
			tq = (TweetQueue)FindObjectOfType(typeof(TweetQueue));
		}

		if(charMeshMap == null)
		{
			charMeshMap = new Dictionary<char, GameObject>();
			for(int i=0; i<tq.chars.Length; i++)
			{
				charMeshMap.Add(tq.chars[i][0], tq.charMeshes[i]);
			}
		}

		string dequeuedTweet = null;
		Waiters.DoUntil(_ => dequeuedTweet != null, _ => dequeuedTweet = tq.Dequeue(), gameObject)
			   .Then(() => { Debug.Log(dequeuedTweet); foreach(char c in dequeuedTweet) tweet.Enqueue(c); });
	}

	void Update()
	{
		if(tweet.Count < 1)
			return;

		curCharDelay -= Time.deltaTime;
		if(curCharDelay > 0)
			return;

		char c = tweet.Dequeue();

		if(Char.IsWhiteSpace(c))
		{
			curCharDelay = charDelay * 1.6f;
			return;
		}

		GameObject go;
		if(!charMeshMap.TryGetValue(c, out go) && !charMeshMap.TryGetValue(Char.ToUpper(c), out go))
			return;

		Quaternion rot = Quaternion.Euler(Mathy.Sin(25f, 1.3f, 0), Mathy.Sin(25f, 0.7f, 0.7f), Mathy.Sin(25f, 1.7f, 0.4f));
		GameObject clone = (GameObject)Instantiate(go, transform.position, transform.rotation * rot);

		clone.transform.parent = letterParent;

		//use mesh bounds width to determine tracking
		curCharDelay = charDelay + clone.GetComponent<MeshFilter>().mesh.bounds.size.x * tracking;

		clone.AddComponent<LetterFloat>();
	}
}