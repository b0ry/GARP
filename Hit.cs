using UnityEngine;
using System.Collections;

public class Hit : MonoBehaviour {
	public GameObject hit;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = transform.position;
	}
	void OnCollisionEnter(Collision slash){
		if (slash.gameObject.tag == "Player"){
			Rigidbody h = Instantiate(hit,slash.gameObject.transform.localPosition,slash.gameObject.transform.localRotation)as Rigidbody;
		}
	}
}
