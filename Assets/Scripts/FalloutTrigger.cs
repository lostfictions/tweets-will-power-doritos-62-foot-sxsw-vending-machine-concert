using UnityEngine;
using System.Collections;

public class FalloutTrigger : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		Destroy(other.gameObject);
	}
}
