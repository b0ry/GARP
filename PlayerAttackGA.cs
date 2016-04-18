using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using GARP.GA;

public class PlayerAttackGA : MonoBehaviour {
	public List<string> xDirIN = new List<string>();
	public List<string> zDirIN = new List<string>();
	public List<float> rangeIN = new List<float>();
	public List<int> damageIN = new List<int>();

	public int level = 1;
	public int i;

	public List<string> xDirOUT_A = new List<string>();
	public List<string> xDirOUT_B = new List<string>();
	public List<string> zDirOUT_A = new List<string>();
	public List<string> zDirOUT_B = new List<string>();
	public float rangeOUT;
	public int damageOUT;

	// Use this for initialization
	public void AddToAttackList(Attack attack) {
		xDirIN.Add (attack.x);
		zDirIN.Add (attack.z);
		damageIN.Add (attack.damage);
        rangeIN.Add (attack.range);
		i++;
		if(i == level*10){
			i = 0;

			CrossoverMutation();
		}
	}
	
	// Update is called once per frame
	void CrossoverMutation () {
		//Crossover range and damage
		float fullRange = 0;
		int fullDamage = 0;
		for (int j = 0; j < level*10; j++){
			fullRange += rangeIN[j];
			fullDamage += damageIN[j];

			string[] mutations = new string[6];
			//mutations[0] = "+ e";
			//mutations[1] = "* e";
			mutations[2] = "+ sin";
			mutations[3] = "* sin";
			mutations[4] = "+ cos";
			mutations[5] = "* cos";

			int kX;
			for ( kX = 2; kX < 6; kX++){
				int mutator = Random.Range (1,100);
				if (mutator > 90){
					int mutated = Random.Range (2,6);
					xDirIN[j] = xDirIN[j].Replace (mutations[kX], mutations[mutated]);
				}
			}

			int mX=1;
			int nX=1;
			int noOpenBracketsX = 0;
			int noClseBracketsX = 0;
			for (kX = 1; kX < xDirIN[j].Length; kX++)
					{
					int foundOpenBracket = xDirIN[j].IndexOf("(", mX);
					if (foundOpenBracket > 0)
					{
						mX = foundOpenBracket+1;
						noOpenBracketsX++;
					}
					int foundClseBracket = xDirIN[j].IndexOf(")", nX);
					if (mX > nX && kX != 1){
						xDirOUT_A.Add (xDirIN[j].Substring (0,nX));
						xDirOUT_B.Add (xDirIN[j].Substring (nX));
					}
					else {
						if (foundClseBracket > 0)
						{
							nX = foundClseBracket+1;
							noClseBracketsX++;
						}
					}
				}
			
			int kZ;
			for ( kZ = 2; kZ < 6; kZ++){
				int mutator = Random.Range (1,100);
				if (mutator > 94){
					int mutated = Random.Range (2,6);
					zDirIN[j].Replace (mutations[kZ], mutations[mutated]);
				}
			}

			int mZ=1;
			int nZ=1;
			int noOpenBracketsZ = 0;
			int noClseBracketsZ = 0;
			for (kZ = 1; kZ < zDirIN[j].Length; kZ++)
			{
				int foundOpenBracket = zDirIN[j].IndexOf("(", mZ);
				if (foundOpenBracket > 0)
				{
					mZ = foundOpenBracket+1;
					noOpenBracketsZ++;
				}
				int foundClseBracket = zDirIN[j].IndexOf(")", nZ);
				if (mZ > nZ && kZ != 1){
					zDirOUT_A.Add(zDirIN[j].Substring (0,nZ));
					zDirOUT_B.Add(zDirIN[j].Substring (nZ));
				}
				else {
					if (foundClseBracket > 0)
					{
						nZ = foundClseBracket+1;
						noClseBracketsZ++;
					}
				}
			}

			//Mutation happens when fired...

		}
		xDirOUT_A = xDirOUT_A.Distinct().ToList();
		xDirOUT_B = xDirOUT_B.Distinct().ToList();
		zDirOUT_A = zDirOUT_A.Distinct().ToList();
		zDirOUT_B = zDirOUT_B.Distinct().ToList();
		rangeOUT = (fullRange/(level*10))/4;
		damageOUT = fullDamage/(level*10);

		level++;
		SendMessage ("NextLevel", "Attack", SendMessageOptions.DontRequireReceiver);
	}

}