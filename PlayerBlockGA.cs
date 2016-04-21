using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using GARP.GA;
using GARP.Useful;

public class PlayerBlockGA : MonoBehaviour {
	public List<int> blockEffectIN = new List<int>();
	public float average = 0f;
	public Levels levels;
	public int i = 0;
	public List<int> blockEffectOUT = new List<int>();

	public void Start () {
		levels = Singleton<Levels>.Unique;
	}
	public void AddToBlockList(Block block) {
		blockEffectIN.Add(block.block);
		i++;
		if(i == levels.blockLevel*10){
			gameObject.GetComponent<ThirdPersonController>().hits = 0;
			i = 0;
			CrossoverMutation();
		}
	}
	void CrossoverMutation() {
		int min = 0;
		int max = 0;
		for(int j = 0; j < levels.blockLevel*10; j ++) {
			blockEffectOUT.Add (blockEffectIN[j]);
			if(blockEffectIN[j] < blockEffectIN[min] ) {
				min  = j;
			}
			if(blockEffectIN[j] > blockEffectIN[max] ) {
				max  = j;
			}
		}
		blockEffectOUT = blockEffectOUT.Distinct().ToList();
		blockEffectOUT [min] = blockEffectIN [max];
		int rnd = Random.Range(0,50*levels.blockLevel);
		if (rnd < levels.blockLevel*10) blockEffectOUT[rnd] = Random.Range (1,11)*10;
		SendMessage ("NextLevel", "Block", SendMessageOptions.DontRequireReceiver);
		average = (float)blockEffectOUT.Average ();

	}
}
