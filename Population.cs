using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Population : MonoBehaviour {
	//public float launchSpeed;
	//public float launchDamage;
	// Use this for initialization
	public List<Vector3> playerLaunchRange ;
	void AddItem (Vector3 item) {
		playerLaunchRange.Add(item);
	}
}
