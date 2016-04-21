using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using GARP.GA;
using GARP.Useful;

public class PlayerStrafeGA : MonoBehaviour {
	public List<float> strafeIN = new List<float>();
	public int i = 0;
	public float strafeOUT;
	public bool flag;
	private float timer = 1f;
	public Levels levels;

	public void Start () {
		levels = Singleton<Levels>.Unique;
	}

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

		if (i == levels.strafeLevel * 10) {
			i = 0;
			float fullStrafe = 0f;
			for (int j = 0; j < levels.strafeLevel*10; j ++) {
				fullStrafe += strafeIN [j];
			}
			int hits = gameObject.GetComponentInParent<ThirdPersonController> ().hits;
			strafeOUT = fullStrafe / (levels.strafeLevel*10);
			strafeIN.Clear ();
			SendMessage ("NextLevel", "Strafe", SendMessageOptions.DontRequireReceiver);

		}
	}
}
