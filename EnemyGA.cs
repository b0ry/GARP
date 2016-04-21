/// <summary>
/// 
/// NOT CURRENTLY USED
/// 
/// </summary>
using UnityEngine;
using System.Collections;

public class EnemyGA: MonoBehaviour {
	public int gen = 0; // public for debug purposes only
	public float NewEnemyDirection; // public for debug purposes only
	private float[] EnemyDirection = new float[60];
	private float rndDirection;
	private int j=0;
	private float[] minima = new float[10];
	void Start() 
	{
		//First Generation (then changes once every 6 seconds)
		for (int i = 0; i < 60; i++)
		{
			EnemyDirection[i] = Random.Range(0, Mathf.PI/20f);
		}
		InvokeRepeating("RandomValues",0f,0.1f);
	}

		void RandomValues ()
	{
		//Go through generations
		if (j < 60){
			rndDirection = EnemyDirection[j];
			j++;
		}
		else {
			gen += 1;
			j = 0;

			// The fitness rank is the lowest values for rndDirection
			System.Array.Sort(EnemyDirection);

			// Selection is done by taking the 10 best estimations over the past generation
			System.Array.Copy (EnemyDirection,minima,10);

			// This part is a little cheeky atm, since I don't use a crossover function, just the mutation.
			// I take the average of the Selection and use this as the next generation multiplied by some random value between 1e-4 and 10.
			// This means they evolve to the correct solution within about 30 gens.
			float sum = 0;
			for (int i = 0; i < minima.Length; i++)
			{
				sum += minima[i];
			}
			NewEnemyDirection = sum/minima.Length;
			for (int k = 0; k < 60; k++)
			{
				EnemyDirection[k] = NewEnemyDirection*Random.Range (0.0001f,10f);
			}

			//This changes the shape of the enemy. 
			//NOTE: This is an example of how you call functions from other objects in Unity. Notice how reshape.cs is on a child component
			//of this enemy.
			//GetComponentInChildren<Reshape>().ChangeShape();

		}
	}
}
