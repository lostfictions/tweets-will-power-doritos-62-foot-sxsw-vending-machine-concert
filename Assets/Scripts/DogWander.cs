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

	void Start()
	{
		nma = GetComponent<NavMeshAgent>();

		nma.SetDestination(new Vector3(Random.Range(-12f, 12f), 0, Random.Range(-12f, 12f)));

		// NavMeshHit hit;
		// if(NavMesh.SamplePosition(new Vector3(, out hit : NavMeshHit, maxDistance : float, allowedMask : int)))
		// {
		// 	nma.SetDestination()
		// }
	}
	
	void Update()
	{
		if(!nma.hasPath && w == null)
		{
			w = Waiters.Wait(Random.Range(0,3f), gameObject)
					   .Then(()=> nma.SetDestination(new Vector3(Random.Range(-12f, 12f), 0, Random.Range(-12f, 12f))));
		}
	}
}
