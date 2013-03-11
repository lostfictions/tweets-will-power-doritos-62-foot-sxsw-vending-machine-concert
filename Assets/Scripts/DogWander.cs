using UnityEngine;
using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class DogWander : MonoBehaviour
{
	NavMeshAgent nma;
	Waiter w;

	float timeout = 0;

	void Start()
	{
		nma = GetComponent<NavMeshAgent>();

		nma.SetDestination(new Vector3(Random.Range(-8.5f, 8.5f), 0, Random.Range(-8.5f, 8.5f)));
		timeout = 15f;

		// NavMeshHit hit;
		// if(NavMesh.SamplePosition(new Vector3(, out hit : NavMeshHit, maxDistance : float, allowedMask : int)))
		// {
		// 	nma.SetDestination()
		// }
	}
	
	void Update()
	{
		timeout -= Time.deltaTime;
		if(w == null && (timeout < 0 || !nma.hasPath))
		{
			w = Waiters.Wait(Random.Range(0,3f), gameObject)
					   .Then(()=> { timeout = 15f; nma.SetDestination(new Vector3(Random.Range(-12f, 12f), 0, Random.Range(-12f, 12f))); });
		}
	}
}
