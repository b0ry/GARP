using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PlayerStrafeGA : MonoBehaviour {
	public List<float> strafeIN = new List<float>();
	public int level = 1;
	public int i = 0;
	public float strafeOUT;
	
	public void AddToList(float strafe) {
		strafeIN.Add(strafe);
		i++;
		if(i == level*10){
			i = 0;
			CrossoverMutation();
		}
	}
	void CrossoverMutation() {
		float fullStrafe = 0f;
		for(int j = 0; j < 10*level; j ++) {
			fullStrafe += strafeIN[j];
		}
		int hits = gameObject.GetComponentInParent<PlayerBlock> ().hits;
		strafeOUT = (fullStrafe/(10*level))/10 - 0.01f*(float)hits;
		strafeIN.Clear();
		level++;
		gameObject.transform.parent.GetComponent<Morph>().MorphNGrow ("circle");
		
	}
}
