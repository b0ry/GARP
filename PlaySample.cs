using UnityEngine;
using System.Collections;

public class PlaySample : MonoBehaviour {
	public AudioClip sample;
	public float volume;
	public GameObject trigger;
	private AudioSource audio;
	
	void Start() {
		audio = GetComponent<AudioSource>();
	}
	
	void OnCollisionEnter(Collision trig) {
		if (trig.gameObject.tag == trigger.tag){
		audio.PlayOneShot(sample, volume);
		}
	}
}
