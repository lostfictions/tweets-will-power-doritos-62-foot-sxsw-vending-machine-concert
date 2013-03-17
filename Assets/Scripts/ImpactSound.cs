using UnityEngine;
using System.Collections;

public class ImpactSound : MonoBehaviour
{
	public AudioClip[] SoundList;
	public float forceThreshold = 1f;
	public float pitchRandomization = 0.08f;
	public float impactForceFactor = 0f;

	void Start()
	{
		if(!audio)
			gameObject.AddComponent<AudioSource>();
	}

	void OnCollisionEnter(Collision col)
	{	
		//TODO: take another look at this stuff, sanity check on force-based volume

		float forcePastThreshold = col.relativeVelocity.magnitude - forceThreshold;
		
		if(forcePastThreshold > 0)
		{
			audio.pitch = 1f + Random.Range(-pitchRandomization/2f, pitchRandomization/2f);
			audio.PlayOneShot(SoundList.RandomInRange(), 1f + forcePastThreshold * impactForceFactor);							   
		}
	}
}
