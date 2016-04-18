using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelUp : MonoBehaviour {
	public GameObject smoke;
	public int attackLevel = 1;
	public int blockLevel = 1;
	public int runLevel = 1;
	public int strafeLevel = 1;

	public void NextLevel(string type) {
		GameObject kerching = (GameObject)Instantiate(smoke, transform.position, transform.rotation);
		string shape = "";
		if (type == "Attack") {
			attackLevel++;
			shape = "triangle";
		}
		if (type == "Block") {
			blockLevel++;
			shape = "square";
		}
		if (type == "Strafe") {
			strafeLevel++;
			shape = "circle";
		}
		if (type == "Run") {
			runLevel++;
			shape = "circle";
		}
		List<int> levels = new List<int>();
		levels.Add (attackLevel);
		levels.Add (blockLevel);
		levels.Add (runLevel);
		levels.Add (strafeLevel);
		levels.Sort();
		levels.Reverse();
		int max = levels [0];
		gameObject.GetComponent<SphereCollider> ().radius = max;

		SendMessage ("MorphNGrow", shape, SendMessageOptions.DontRequireReceiver);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
