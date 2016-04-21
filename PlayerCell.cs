using UnityEngine;
using System.Collections;

public class PlayerCell : MonoBehaviour {
	private float initial = 0.1f;
	private float scale;
	private Color normalColour;
	private Color slashColour;
	// Use this for initialization
	void Start () {
		normalColour = gameObject.GetComponent<Renderer>().material.color;
		slashColour = new Color(1f,1f,1f,1f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Breathe ();
	}

	void Breathe() {
		scale = (Mathf.Cos(initial)/20f)+transform.localScale.x;
		initial = initial + 0.1f;
		if (initial >= 360) {
			initial = 0f;
		}
		transform.localScale = new Vector3(transform.localScale.x,scale,transform.localScale.z);
	}

	public void playerFlash (){
		Flash ();
			StartCoroutine (Flash());
	}
	IEnumerator Flash() 
	{
		for (int i = 0; i < 5; i++)
		{
			gameObject.GetComponent<Renderer>().material.color = slashColour;
			yield return new WaitForSeconds(.1f);
			gameObject.GetComponent<Renderer>().material.color = normalColour; 
			yield return new WaitForSeconds(.1f);
		}
	}
}

