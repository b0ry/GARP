using UnityEngine;
using System.Collections;

public class ShapeMove : MonoBehaviour {
	public float initial;
	private float scale;
	public Color normal;
	
	void Start () {
		//startpos = transform.position.y;
		normal = gameObject.GetComponent<Renderer>().material.color;
	}
	public void Bounce() {
		scale = Mathf.Sin(initial)/2f;
		initial = initial + 0.05f;
		this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y*scale+1f, this.transform.position.z);
	}
	
	void FixedUpdate () 
	{
		if (initial >= Mathf.PI)
			initial = 0f;
	}
}

