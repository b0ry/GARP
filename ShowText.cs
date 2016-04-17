using UnityEngine;
using System.Collections;

public class ShowText : MonoBehaviour {
	private TextMesh damCount;
	public int counter = 0;

	// Use this for initialization
	void Start () {
		damCount = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		damCount.transform.rotation = Quaternion.LookRotation(damCount.transform.position - Camera.main.transform.position);
		if (counter > 0) 
			counter--;
		
		if (counter == 0) 
			damCount.text = "";
	}
}
