using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using GARP.GA;

public class PlayerRunGA : MonoBehaviour {
	public List<float> runIN = new List<float>();
	public List<float> cdIN = new List<float>();
	public int level = 1;
	public float runOUT = 0.0f;
	public float cdOUT = 0.0f;
	public int i = 0;
	
	public void AddToRunList(Run run) {
		runIN.Add(run.speed);
		cdIN.Add (run.cooldown);
		i++;
		if(i == level*10){
			i = 0;
			CrossoverMutation();
		}	
	}

	void CrossoverMutation(){
		int hits = gameObject.GetComponent<ThirdPersonController> ().hits;
		runOUT = runIN.Average () / 10f - 0.01f * (float)hits;
		cdOUT  = 5f/cdIN.Average () - 0.01f * (float)hits;
		level++;
		runIN.Clear ();
		cdIN.Clear ();
		SendMessage ("NextLevel", "Run", SendMessageOptions.DontRequireReceiver);
	}
}
