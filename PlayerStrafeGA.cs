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
	//static private float speed;
	private float timer = 1f;

	public void Update() {
		timer += Time.deltaTime;
		if (Input.GetKey (KeyCode.S) && Input.GetKey (KeyCode.A)) {
			Strafe ("Left");
		}
		if (Input.GetKey (KeyCode.S) && Input.GetKey (KeyCode.D)) {
			Strafe ("Right");
		}
	}

	void Strafe (string lr){
		float newSpeed = Random.Range(0.8f,2f) + strafeOUT;
		gameObject.GetComponent<Renderer> ().material.color = gameObject.GetComponent<NewWalk> ().normal;
		if (lr == "Left") transform.Rotate (transform.forward*newSpeed);	
		if (lr == "Right") transform.Rotate (transform.forward*-newSpeed);	
		if (flag) {
				if (lr == "Left") transform.parent.localPosition += Camera.main.transform.right * -Time.deltaTime*newSpeed;
				if (lr == "Right") transform.parent.localPosition += Camera.main.transform.right * Time.deltaTime*newSpeed;
				if (timer >= 1f) { 
					AddToList (newSpeed);
					timer = 0f;
				}
			}
		}

	void AddToList(float spd) {
		strafeIN.Add (spd);
		i++;

		if (i == level * 10) {
			i = 0;
			float fullStrafe = 0f;
			for (int j = 0; j < 10*level; j ++) {
				fullStrafe += strafeIN [j];
			}
			int hits = gameObject.GetComponentInParent<PlayerBlock> ().hits;
			strafeOUT = fullStrafe / (10 * level);
			strafeIN.Clear ();
			level++;
			gameObject.GetComponentInParent<LevelUp>().NextLevel("Strafe");

		}
	}
}
