using UnityEngine;
using System.Collections;

public class ShowText : MonoBehaviour {
	private TextMesh showText;
	public int counter = 0;

	// Use this for initialization
	void Start () {
		showText = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (Camera.main.transform);
		showText.transform.rotation = Quaternion.LookRotation(showText.transform.position - Camera.main.transform.position);
		if (counter > 0) 
			counter--;
		
		if (counter == 0) 
			showText.text = "";
	}

	public void DisplayText (string str) {
		showText.text = str;
	}
}
