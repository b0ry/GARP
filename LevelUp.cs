using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GARP.GA;
using GARP.Useful;

public class LevelUp : MonoBehaviour {
	public GameObject smoke;
	public Levels level;
	void Start () {
		level = Singleton<Levels>.Unique;
		level.attackLevel = 1;
		level.blockLevel = 1;
		level.runLevel = 1;
		level.strafeLevel = 1;
	}

	public void NextLevel(string type) {
		GameObject kerching = (GameObject)Instantiate(smoke, transform.position, transform.rotation);
		string shape = "";
		if (type == "Attack") {
			level.attackLevel++;
			shape = "triangle";
		}
		if (type == "Block") {
			level.blockLevel++;
			shape = "square";
		}
		if (type == "Strafe") {
			level.strafeLevel++;
			shape = "circle";
		}
		if (type == "Run") {
			level.runLevel++;
			shape = "circle";
		}
		List<int> levels = new List<int>();
		levels.Add (level.attackLevel);
		levels.Add (level.blockLevel);
		levels.Add (level.runLevel);
		levels.Add (level.strafeLevel);
		levels.Sort();
		levels.Reverse();
		int max = levels [0];
		gameObject.GetComponent<SphereCollider> ().radius = max;

		SendMessage ("MorphNGrow", shape, SendMessageOptions.DontRequireReceiver);
	}
}
