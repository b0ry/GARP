using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PlayerRunGA : MonoBehaviour {
	public List<float> runIN = new List<float>();
	public List<float> cdIN = new List<float>();
	public int level = 1;
	public float runOUT = 0.0f;
	public float cdOUT = 0.0f;
	public int i = 0;
	
	public void AddToList(float run, float cooldown) {
		runIN.Add(run);
		cdIN.Add (cooldown);
		i++;
		if(i == level*10){
			i = 0;
			CrossoverMutation();
		}	
	}

	void CrossoverMutation(){
		int hits = gameObject.GetComponent<PlayerBlock> ().hits;
		runOUT = runIN.Average () / 10f - 0.01f * (float)hits;
		cdOUT  = 5f/cdIN.Average () - 0.01f * (float)hits;
		level++;
		runIN.Clear ();
		cdIN.Clear ();
		gameObject.GetComponent<Morph>().MorphNGrow ("circle");
	}
}
