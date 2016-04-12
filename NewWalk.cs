using UnityEngine;
using System.Collections;

public class NewWalk : MonoBehaviour {
	public GameObject smoke;
	public float initial = 0f;
	public float scale;
	public Color normal;
	private static GameObject run;
	
	void Start () {
		//startpos = transform.position.y;
		normal = gameObject.GetComponent<Renderer>().material.color;
	}
	
	void FixedUpdate () 
	{
		if( initial < 3f ||
		   (Input.GetKey(KeyCode.W) 	|| 
		 	Input.GetKey(KeyCode.A) 	|| 
		 	Input.GetKey(KeyCode.D)))
		{
			scale = Mathf.Sin(initial)/2f;
			initial = initial + 0.05f;
			transform.position = new Vector3(transform.position.x,transform.position.y*scale+2f,transform.position.z);
		}
		if (initial >= 3.2f) {
			initial = 0f;
			if(Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)){
				run = (GameObject)Instantiate(smoke, transform.position, transform.rotation);
				run.transform.parent = this.transform;
				Destroy (run,2f);
			}
		}
	}
}

