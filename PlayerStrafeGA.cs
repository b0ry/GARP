using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using GARP.GA;

public class PlayerStrafeGA : MonoBehaviour {
	public List<float> strafeIN = new List<float>();
	public int level = 1;
	public int i = 0;
	public float strafeOUT;
	public bool flag;
	//static private float speed;
	private float timer = 1f;

	public void Update() {
		timer += Time.deltaTime;
	}

	public void Strafe (string lr){
		float newSpeed = Random.Range(0.8f,2f) + strafeOUT;
		gameObject.GetComponent<Renderer> ().material.color = gameObject.GetComponent<ShapeMove> ().normal;
		if (lr == "Left") transform.Rotate (transform.forward*newSpeed);	
		if (lr == "Right") transform.Rotate (transform.forward*-newSpeed);	
		if (flag) {
				if (lr == "Left") transform.parent.localPosition += Camera.main.transform.right * -Time.deltaTime*newSpeed;
				if (lr == "Right") transform.parent.localPosition += Camera.main.transform.right * Time.deltaTime*newSpeed;
				if (timer >= 1f) { 
					Strafe strafe = new Strafe();
				strafe.strafe = newSpeed;
					AddToStrafeList (strafe);
					timer = 0f;
				}
			}
		}

	void AddToStrafeList(Strafe strafe) {
		strafeIN.Add (strafe.strafe);
		i++;

		if (i == level * 10) {
			i = 0;
			float fullStrafe = 0f;
			for (int j = 0; j < 10*level; j ++) {
				fullStrafe += strafeIN [j];
			}
			int hits = gameObject.GetComponentInParent<ThirdPersonController> ().hits;
			strafeOUT = fullStrafe / (10 * level);
			strafeIN.Clear ();
			level++;
			SendMessage ("NextLevel", "Strafe", SendMessageOptions.DontRequireReceiver);

		}
	}
}
