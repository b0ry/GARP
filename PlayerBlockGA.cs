using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PlayerBlockGA : MonoBehaviour {
	public List<int> blockEffectIN = new List<int>();
	public float average = 0f;
	public int level = 1;
	public int i = 0;
	public List<int> blockEffectOUT = new List<int>();

	public void AddToList(int blockEffect) {
		blockEffectIN.Add(blockEffect);
		i++;
		if(i == level*10){
			gameObject.GetComponent<PlayerBlock>().hits = 0;
			i = 0;
			CrossoverMutation();
		}
	}
	void CrossoverMutation() {
		int min = 0;
		int max = 0;

		
		for(int j = 0; j < 10*level; j ++) {
			blockEffectOUT.Add (blockEffectIN[j]);
			if(blockEffectIN[j] < blockEffectIN[min] ) {
				min  = j;
			}
			if(blockEffectIN[j] > blockEffectIN[max] ) {
				max  = j;
			}
		}
		blockEffectOUT = blockEffectOUT.Distinct().ToList();
		Debug.Log (max+" "+min);
		blockEffectOUT[min] = blockEffectIN[max];
		int rnd = Random.Range(0,50*level);
		if (rnd < 10*level) {blockEffectOUT[rnd] = Random.Range (1,11)*10;}
		level++;
		gameObject.GetComponent<LevelUp>().NextLevel("Block");
		average = (float)blockEffectOUT.Average ();

	}
}
