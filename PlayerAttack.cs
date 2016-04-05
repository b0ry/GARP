using UnityEngine;
using System.Collections;
[System.Serializable]

public class PlayerAttack : MonoBehaviour {
	private static int maxDepth = 2;
	public string outputX = "";
	public string outputZ = "";
	
	void Awake(){
		int level = GameObject.Find("MyPlayer").GetComponent<PlayerAttackGA>().level;
		if (level == 1){
			NodeTree firstTreeX = new NodeTree();
			NodeTree firstTreeZ = new NodeTree();
			firstTreeX.GetTree ();
			firstTreeZ.GetTree ();
			outputX = firstTreeX.str;
			outputZ = firstTreeZ.str;
		}
		else{
			int jX = Random.Range (0,GameObject.Find("MyPlayer").GetComponent<PlayerAttackGA>().xDirOUT_A.Count);
			int kX = Random.Range (0,GameObject.Find("MyPlayer").GetComponent<PlayerAttackGA>().xDirOUT_B.Count);
			string str_AX = GameObject.Find("MyPlayer").GetComponent<PlayerAttackGA>().xDirOUT_A[jX];
			string str_BX = GameObject.Find("MyPlayer").GetComponent<PlayerAttackGA>().xDirOUT_B[kX];
			outputX = str_AX + str_BX;

			int jZ = Random.Range (0,GameObject.Find("MyPlayer").GetComponent<PlayerAttackGA>().zDirOUT_A.Count);
			int kZ = Random.Range (0,GameObject.Find("MyPlayer").GetComponent<PlayerAttackGA>().zDirOUT_B.Count);
			string str_AZ = GameObject.Find("MyPlayer").GetComponent<PlayerAttackGA>().zDirOUT_A[jZ];
			string str_BZ = GameObject.Find("MyPlayer").GetComponent<PlayerAttackGA>().zDirOUT_B[kZ];
			outputZ = str_AZ + str_BZ;

			int idx = GameObject.Find("MyPlayer").GetComponent<PlayerAttackGA>().i;
			if (idx == 0) {
				GameObject.Find("MyPlayer").GetComponent<PlayerAttackGA>().xDirIN.Clear();
				GameObject.Find("MyPlayer").GetComponent<PlayerAttackGA>().zDirIN.Clear();
				GameObject.Find("MyPlayer").GetComponent<PlayerAttackGA>().damageIN.Clear();
				GameObject.Find("MyPlayer").GetComponent<PlayerAttackGA>().rangeIN.Clear();
			}

		}


	}
	public class NodeTree{
		public string str = "";
		private int depth = 0;

	public void GetTree() {
			NodeTree node_A = new NodeTree();
			NodeTree node_B = new NodeTree();
			int idx;
			if (depth == 0){ idx = Random.Range(1,8);}
			else {idx = Random.Range(1,24);}

			if (depth == maxDepth ){ 
				str = "("+Random.Range (0,3)+"*time)"; return;
			}
			if (idx > 6 ){
				str = "(time)"; 
			}

			else{
				node_A.depth = depth+1;
				node_B.depth = depth+1;
				node_A.GetTree ();
				node_B.GetTree ();
			}

			if (idx == 1){ str = "(" + node_A.str + " + " + node_B.str + ")"; }
			if (idx == 2){ str = "(" + node_A.str + " * " + node_B.str + ")"; }
			if (idx == 5){ str = "(" + node_A.str + " * sin" + node_B.str + ")"; }
			if (idx == 4){ str = "(" + node_A.str + " * cos" + node_B.str + ")"; }
			if (idx == 3){ str = "(" + node_A.str + " + sin" + node_B.str + ")"; }
			if (idx == 6){ str = "(" + node_A.str + " + cos" + node_B.str + ")"; }

		}
  }
}
