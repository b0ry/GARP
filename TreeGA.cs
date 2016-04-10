using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreeGA : MonoBehaviour {
	public int treeCounter;
	public int[] indices = new int[10];
	public int[] fitness = new int[7];
	public int[] trnglVals = new int[7] ;
	public int[] sqrVals = new int[7] ;
	public int[] crclVals = new int[7] ;
	public int[] nastyVals = new int[7] ;
	public int[] niceVals = new int[7] ;
	public Vector3[] sizes = new Vector3[7];
	public Color[] shades = new Color[7];
	public List<string>[] typess = new List<string>[7];
	private int i=0;
	private GameObject treeCone;
	private int children;
	public int level;
	public GameObject tree;

	// Use this for initialization
	void Start () {
		children = transform.childCount;
		for (int j = 0; j < children; j++){ shades[j] = new Color(0f,0f,0f,0f);
			level = 1;
		}
	}

	public void RankTree(int trngl, int sqr, int crcl, int nice, int nasty, Vector3 size, Color shade, List<string> types){
		trnglVals[i] = trngl;
		sqrVals[i] = sqr;
		crclVals[i] =  crcl;
		niceVals[i] = nasty;
		nastyVals[i] = nice;
		sizes[i] = size;
		shades[i] = shade;
		typess[i] = types;

		i++;

		if (i == children) {
			Debug.Log ("Ranking  " + transform.name);
			i = 0;
			level++;
			int triSum = SumArray(trnglVals);
			int sqrSum = SumArray(sqrVals);
			int cirSum = SumArray(crclVals);
			int nstySum = SumArray(nastyVals);
			int niceSum = SumArray(niceVals);
			Color addHue = new Color (0.15f,-0.1f,0f,0f);
			if (sqrSum > triSum) {addHue = new Color(-0.1f,0f,0.15f,0f);}
			if (cirSum > sqrSum) {addHue = new Color(0.1f,0.075f,-0.15f,0f);}
			if (nstySum > cirSum) {addHue = new Color(0.08f,0.085f,0.075f,0f);}
			if (niceSum > nstySum) {addHue = new Color(-0.085f,-0.075f,-0.8f,0f);}
			Debug.Log(addHue);
			for (int j=0; j < children; j++){
				fitness[j] = trnglVals[j] + sqrVals[j] + crclVals[j];
				float rndR = Random.Range (0.01f,0.05f);
				float rndG = Random.Range (0.01f,0.05f);
				float rndB = Random.Range (0.01f,0.05f);
				shades[j] += addHue + new Color(rndR,rndG,rndB,0f);
			}

			int[] maxima = new int[10];
			int[] sorted = new int[7];
			System.Array.Copy (fitness,sorted,7);
			System.Array.Sort(sorted);
			System.Array.Reverse(sorted);
			System.Array.Copy (sorted,maxima,7);

			int kMax = 0;
			for (int j = 0; j < children; j++){
				if (fitness[j] == maxima[kMax]){
					indices[kMax] = j;
					kMax++;
					fitness[j] = 99;
				}
				if (kMax == 9){ break;}
			}

			int kInd = 0;
				for (int j = 0; j < children; j++){
					if (fitness[j] == 0){
						trnglVals[j] = trnglVals[indices[kInd]];
						sqrVals[j] = sqrVals[indices[kInd]];
						crclVals[j] = crclVals[indices[kInd]];
						nastyVals[j] = nastyVals[indices[kInd]];
						niceVals[j] = niceVals[indices[kInd]];
						sizes[j] = sizes[indices[kInd]];
						shades[j] = shades[indices[kInd]];
						typess[j] = typess[indices[kInd]];
					kInd++;
				}
				if (kInd == 9){ break;}
			}
			for (int j = 0; j < children; j++){
				int number = j+1;
				GameObject[] treeCones = new GameObject[10];
				treeCones = GameObject.FindGameObjectsWithTag(number.ToString());
				for(int k = 0; k < treeCones.Length; k++){
					for(int kk = 0; kk < children; kk++){
					if (treeCones[k].transform.IsChildOf(transform.GetChild(kk))){
							treeCone = treeCones[k];
							break;
						}
					}
				} 
			
				GameObject newTree = Instantiate(tree, treeCone.transform.position, treeCone.transform.rotation) as GameObject;
				newTree.transform.parent = treeCone.transform;
				newTree.GetComponent<TreeSpawn>().treeParent = gameObject;
				newTree.GetComponent<TreeSpawn>().level = level;

				trnglVals[j] = 0;
				sqrVals[j] = 0;
				crclVals[j] = 0;
				niceVals[j] = 0;
				nastyVals[j] = 0;
			}
		} 
	}
	public int SumArray(int[] toBeSummed)
	{
		int sum = 0;
		foreach (int item in toBeSummed)
		{
			sum += item;
		}
		return sum;
	}
	
}