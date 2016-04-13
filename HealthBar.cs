using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HealthBar : MonoBehaviour {
	public int[] weight = new int[10];
	public int no_cells; 
	private int recharge;
	private int heal;
	private float timer;
	private float subTimer;
	public float top;
	public float left;
	public int hit = 0;
	private GameObject player;
	private int min = 10;
	private int max = 0;
	private int minIdx;
	private int maxIdx;
	
	// Use this for initialization
	void Start () {
		recharge = -1;
		player = GameObject.Find ("MyPlayer");
		for (int i = 0 ; i < 10; i++){
			weight[i] = i+1;
		}
		ShuffleArray (weight);
		weight[0] = 10;
		no_cells = transform.childCount;
		for (int i = 0; i < no_cells; i++){
			Transform cell = transform.GetChild(i);
			cell.localScale = new Vector3(0.1f+(weight[i]/100f),0.1f+(weight[i]/100f),0f);
		}
		for (int i = 0; i < no_cells; i++){
			Transform cell = transform.GetChild(i);
			cell.GetComponent<HealthCell>().damage = weight[i];
		}
	}

	public static void ShuffleArray(int[] arr) {
		for (int i = arr.Length - 1; i > 0; i--) {
			int r = Random.Range(0, i + 1);
			int tmp = arr[i];
			arr[i] = arr[r];
			arr[r] = tmp;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (hit != 0){
			timer = 0f;
			for (int i = no_cells - 1; i >= 0; i--) {
				if (weight[i] > 0){
					weight[i] -= hit;
					break;
				}
			}
			for (int i = no_cells-1; i >= 0; i--) {
				if (weight[i] <= 0){
					weight[i] = 0;
					recharge = i;
					heal = i;
				}
				 
			}
			hit = 0;
		}
		for (int i = 0; i < no_cells; i++){
			Transform cell = transform.GetChild(i);
			if (weight[i] > 0){
				cell.localScale = new Vector3(0.1f+(weight[i]/100f),0.1f+(weight[i]/100f),0f);
			}
			else {
				cell.localScale = new Vector3(0.05f,0.05f,0f);
			}
		}
		subTimer += Time.deltaTime;

		if(timer > 3f && recharge >= 0 && subTimer > 0.5f ){
			subTimer = 0f;
			Transform cell = transform.GetChild(recharge);
			int cellDamage = cell.GetComponent<HealthCell>().damage;

			if (weight[recharge] < cellDamage){
				weight[recharge]++;
			}
			if (weight[recharge] == cellDamage && recharge != no_cells){
				recharge++;
			}
			//if (recharge == no_cells ) {recharge = 1;}
			if (recharge == no_cells){
				minIdx = no_cells-1;
				maxIdx = heal;
				int sum = no_cells + heal-1;
				int dif = no_cells - heal-1;
				int[] cTemp = new int[dif+1];
				//weight.
				for (int i = heal; i < no_cells; i++){
					cTemp[i-heal] = weight[Mathf.Abs (i - sum)];
				}
				for (int i = heal; i < no_cells; i++){
					weight[i] = cTemp[i-heal];
				}
				for (int i = heal; i < no_cells; i++){
					if (i == heal) { 
						min = weight[i];
						max = weight[i];
					}
					if (weight[i] < min) { 
						min = weight[i];
						minIdx = i;
					}
					if (weight[i] > max) { 
						max = weight[i];
						maxIdx = i;
					}
				}
				recharge = -1;
				timer = 0f;
				weight[minIdx] = weight[maxIdx];
				transform.GetChild(minIdx).GetComponent<HealthCell>().damage = weight[maxIdx];
				GameObject.Find("MyPlayer").GetComponent<Morph>().MorphNGrow ("square");
			}
		}

		timer += Time.deltaTime;
	}
}
