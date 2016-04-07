using UnityEngine;
using System.Collections;

public class NewWalk : MonoBehaviour {
	public GameObject smoke;
	public float initial = 0f;
	public float scale;
	public float startpos;
	private Color normal;
	public bool flag = false;
	public int damage;
	
	void Start () {
		//startpos = transform.position.y;
		normal = gameObject.GetComponent<Renderer>().material.color;
	}
	
	void FixedUpdate () 
	{
		if( initial < 3f ||
		   (Input.GetKey(KeyCode.W) || 
		 Input.GetKey(KeyCode.A) || 
		 Input.GetKey(KeyCode.D) || 
		 Input.GetKey(KeyCode.UpArrow) || 
		 Input.GetKey(KeyCode.LeftArrow) || 
		 Input.GetKey(KeyCode.RightArrow)) && !flag)
		{
			scale = Mathf.Sin(initial)/2f;
			initial = initial + 0.05f;
			transform.position = new Vector3(transform.position.x,transform.position.y*scale+2f,transform.position.z);
			
			if (initial >= 3.2f) {
				initial = 0f;
				if(Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)){
					GameObject run = (GameObject)Instantiate(smoke, transform.parent.position, transform.parent.rotation);
					Destroy (run,0.8f);
				}
			}
			// Block
			if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
				flag = true;
			}
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
				damage= Random.Range (1,11);
				float rnd = (float)damage/40f;
				gameObject.GetComponent<Renderer>().material.color = new Color (rnd,rnd,rnd+0.1f,1f);
				if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S)){
					Strafe("Left");
				}
			}
			// Not Block
			if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow) || (!Input.anyKeyDown && flag) )
			{
				gameObject.GetComponent<Renderer>().material.color = normal;
				flag = false;
			}
		}
	}
	void Strafe (string lr){
		gameObject.GetComponent<Renderer>().material.color = normal;
		flag = false;
		for (int i = 0; i < 10; i++){
			transform.parent.Rotate(Vector3.forward, 10f * Time.deltaTime);
		}
	}
}

