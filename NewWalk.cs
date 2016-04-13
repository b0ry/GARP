using UnityEngine;
using System.Collections;

public class NewWalk : MonoBehaviour {
	public GameObject smoke;
	public float initial;
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
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y*scale+1f, this.transform.position.z);
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

