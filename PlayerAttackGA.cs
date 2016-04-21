using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using GARP.GA;
using GARP.Useful;

public class PlayerAttackGA : MonoBehaviour {
	public List<string> xDirIN = new List<string>();
	public List<string> zDirIN = new List<string>();
	public List<float> rangeIN = new List<float>();
	public List<int> damageIN = new List<int>();
	public int i;
	public Levels levels;
	public List<string> xDirOUT_A = new List<string>();
	public List<string> xDirOUT_B = new List<string>();
	public List<string> zDirOUT_A = new List<string>();
	public List<string> zDirOUT_B = new List<string>();
	public float rangeOUT;
	public int damageOUT;
	private string[] mutations = {"+ sin","* sin","+ cos","* cos"};

	// Use this for initialization
	public void Start () {
		levels = Singleton<Levels>.Unique;
	}
	public void AddToAttackList(Attack attack) {
		xDirIN.Add (attack.x);
		zDirIN.Add (attack.z);
		damageIN.Add (attack.damage);
        rangeIN.Add (attack.range);
		i++;
		if(i == levels.attackLevel*10){
			i = 0;

			CrossoverMutation();
		}
	}
	
	// Update is called once per frame
	void CrossoverMutation () {
		//Crossover range and damage
		float fullRange = 0;
		int fullDamage = 0;
		for (int j = 0; j < levels.attackLevel*10; j++){
			fullRange += rangeIN[j];
			fullDamage += damageIN[j];
			Mutate (xDirIN, xDirOUT_A, xDirOUT_B, j);
			Mutate (zDirIN, zDirOUT_A, zDirOUT_B, j);
		}

		xDirOUT_A = xDirOUT_A.Distinct().ToList();
		xDirOUT_B = xDirOUT_B.Distinct().ToList();
		zDirOUT_A = zDirOUT_A.Distinct().ToList();
		zDirOUT_B = zDirOUT_B.Distinct().ToList();
		rangeOUT = (fullRange/(levels.attackLevel*10))/4;
		damageOUT = fullDamage/(levels.attackLevel*10);
		SendMessage ("NextLevel", "Attack", SendMessageOptions.DontRequireReceiver);
	}

	void Mutate (List<string> dirIN, List<string> dirOUT_A, List<string> dirOUT_B, int j) {
		int k;
		for ( k = 0; k < 4; k++){
			int mutator = Random.Range (1,100);
			if (mutator > 90){
				int mutated = Random.Range (0,4);
				dirIN[j] = dirIN[j].Replace (mutations[k], mutations[mutated]);
			}
		}
		
		int m=1;
		int n=1;
		int noOpenBrackets = 0;
		int noClseBrackets = 0;
		for (k = 1; k < dirIN[j].Length; k++)
		{
			int foundOpenBracket = dirIN[j].IndexOf("(", m);
			if (foundOpenBracket > 0)
			{
				m = foundOpenBracket+1;
				noOpenBrackets++;
			}
			int foundClseBracket = dirIN[j].IndexOf(")", n);
			if (m > n && k != 1){
				dirOUT_A.Add (dirIN[j].Substring (0,n));
				dirOUT_B.Add (dirIN[j].Substring (n));
			}
			else {
				if (foundClseBracket > 0)
				{
					n = foundClseBracket+1;
					noClseBrackets++;
				}
			}
		}
	}

}