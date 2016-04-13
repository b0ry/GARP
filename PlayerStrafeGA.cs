using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PlayerStrafeGA : MonoBehaviour {
	public List<float> strafeIN = new List<float>();
	public int level = 1;
	public int i = 0;
	public float strafeOUT;
	public bool flag;

	public void Strafe (string lr){
		gameObject.GetComponent<Renderer> ().material.color = gameObject.GetComponent<NewWalk> ().normal;
			if (lr == "Left") transform.Rotate (transform.forward*1f);	
			if (lr == "Right") transform.Rotate (transform.forward*-1f);	
		if (flag) {
			//AddToList ();
			flag = false;
		}
	}	

	void AddToList() {
		float speed = Random.Range(0.2f,0.5f)+strafeOUT*1f;
		strafeIN.Add (speed);
		i++;

		if (i == level * 10) {
			i = 0;
			float fullStrafe = 0f;
			for (int j = 0; j < 10*level; j ++) {
				fullStrafe += strafeIN [j];
			}
			int hits = gameObject.GetComponentInParent<PlayerBlock> ().hits;
			strafeOUT = (fullStrafe / (10 * level)) / 10 - 0.01f * (float)hits;
			strafeIN.Clear ();
			level++;
			gameObject.transform.parent.GetComponent<Morph> ().MorphNGrow ("circle");

		}
	}
}
