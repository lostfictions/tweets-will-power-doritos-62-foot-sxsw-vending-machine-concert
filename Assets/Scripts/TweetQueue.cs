using UnityEngine;
using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

public class TweetQueue : MonoBehaviour
{
	public string[] chars;
	public GameObject[] charMeshes;

	public string queryUri = "http://search.twitter.com/search.json?q=doritos";
	public float queryRefreshDelay = 30f;

	//Actual text of the last tweets pulled
	Queue<string> tweets = new Queue<string>(15);

	//Id of the most recent tweet pulled, for comparison
	ulong lastTweetId = 0;

	float currentQueryRefreshDelay;
	bool queryPending = false;

	public string Dequeue()
	{
		if(tweets.Count > 0)
			return tweets.Dequeue();
		return null;
	}

	void Start()
	{
		currentQueryRefreshDelay = queryRefreshDelay;
		RefreshTweets(queryUri);
	}
	
	void RefreshTweets(string queryUri)
	{
		if(queryPending)
			return;

		queryPending = true;

		WWW www = new WWW(queryUri);

		ulong newId = lastTweetId;

		IEnumerable<string> newTweets = null;

		//TODO: timeout, check for www errors
		Waiters.DoUntil(_ => www.isDone, _ => {}, gameObject)
			   .Then(()=> {
			   				if(www.error != null)
			   				{
			   					Debug.LogWarning(name + ": " + www.error);
			   					return;
			   				}
			   				newTweets = JObject.Parse(www.text)["results"]
														  	   .Where(obj => {
															  					ulong id = obj["id"].ToObject<ulong>();
															  					if(id > lastTweetId)
															  					{
															  						if(id > newId)
															  							newId = id;
															  						return true;
															  				  	}
															  				  	else
															  				  		return false;
														  				     })
														 	   .Select(obj => obj["text"].ToString());
						  })
			   .OnDestroy(() => {
			   						queryPending = false;
			   						if(www.error != null || !www.isDone)
			   							return;
			   						foreach(string t in newTweets.Reverse())
			   							tweets.Enqueue(t);
			   						lastTweetId = newId;
		   						});
	}

	void Update()
	{
		currentQueryRefreshDelay -= Time.deltaTime;
		if(currentQueryRefreshDelay < 0)
		{
			currentQueryRefreshDelay = queryRefreshDelay;
			RefreshTweets(queryUri);
		}
	}
}
