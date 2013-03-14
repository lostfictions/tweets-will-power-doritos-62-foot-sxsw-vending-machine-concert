using UnityEngine;
using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text.RegularExpressions;

public class TweetQueue : MonoBehaviour
{
	public string[] chars;
	public GameObject[] charMeshes;

	//Actual text of the last tweets pulled
	Queue<string> tweets = new Queue<string>(15);

	//Id of the most recent tweet pulled, for comparison
	ulong lastTweetId = 0;

	public float queryRefreshDelay = 30f;
	float currentQueryRefreshDelay;
	bool queryPending = false;

	bool showGUI = false;

	readonly string queryUri = "http://search.twitter.com/search.json?q=";
	string queryKeyword = "doritos";

	void OnGUI()
	{
		if(!showGUI)
			return;

		GUILayout.Label("Search query");
		queryKeyword = GUILayout.TextField(queryKeyword);
		if(GUI.changed)
		{
			queryKeyword = Regex.Replace(queryKeyword, @"[ ]", "%20");
			queryKeyword = Regex.Replace(queryKeyword, @"[@]", "%40");
			queryKeyword = Regex.Replace(queryKeyword, @"[#]", "%23");
			queryKeyword = Regex.Replace(queryKeyword, @"[^a-zA-Z0-9%]", "");
		}
		if(GUILayout.Button("Refresh now"))
		{
			tweets.Clear();

			var tweeters = Object.FindObjectsOfType(typeof(TextBark));
			foreach(var tw in tweeters)
			{
				((TextBark)tw).ClearTweet();
			}

			currentQueryRefreshDelay = 0;

			lastTweetId = 0;
		}
		if(GUILayout.Button("Clean up letters"))
		{
			Transform t = GameObject.Find("Letters").transform;
			foreach(Transform c in t)
			{
				if(!c.rigidbody.isKinematic)
					Destroy(c.gameObject);
			}
		}

		GUILayout.Label(tweets.Count + " results in queue");
		
	}

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

		WWW www = new WWW(queryUri + queryKeyword);

		ulong newId = lastTweetId;

		IEnumerable<string> newTweets = null;

		//TODO: timeout on www request
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
		if(Input.GetKeyDown("`"))
		{
			showGUI = !showGUI;
		}

		currentQueryRefreshDelay -= Time.deltaTime;
		if(currentQueryRefreshDelay < 0)
		{
			currentQueryRefreshDelay = queryRefreshDelay;
			RefreshTweets(queryUri);
		}
	}
}
