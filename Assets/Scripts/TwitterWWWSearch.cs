using UnityEngine;
using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

public class TwitterWWWSearch : MonoBehaviour
{
	string searchQuery = "http://search.twitter.com/search.json?q=doritos";

	string[] tweets;

	int lastTweetId = 0;


	MeshText mt;

	void Start()
	{
		mt = GetComponent<MeshText>();

		WWW www = new WWW(searchQuery);
		// JObject queryResult;

		Waiters.DoUntil(_ => www.isDone, _ => {}, gameObject)
			   .Then(()=> tweets = JObject.Parse(www.text)["results"].Select(obj => obj["text"].ToString()).ToArray());
		//TODO: timeout
		//TODO: only add tweets with id newer than first tweet
	}


	int i=0;
	float timer=0;
	void Update()
	{
		timer += Time.deltaTime;
		if(timer > 2.4f)
		{
			timer = 0;
			if(i<tweets.Length)
			{
				mt.CreateMeshText(tweets[i]);
				i++;
			}
		}
	}
}