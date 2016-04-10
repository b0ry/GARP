using UnityEngine;
using System.Collections;

public class ItemCollect : MonoBehaviour {
	public enum ItemType {trngl,sqr,crcl,nice,nasty};
	public ItemType type;
	void Awake () {
		Destroy (gameObject,10f);
		}
	void OnCollisionEnter(Collision player) {
		if (player.gameObject.tag == "Player") {
			GameObject tree = GameObject.Find ("Tree");
			switch(type){
			case ItemType.trngl : {
				tree.GetComponent<TreeChromosones>().triangles++;
				break;
			}
			case ItemType.sqr : {
				tree.GetComponent<TreeChromosones>().squares++;
				break;
			}
			case ItemType.crcl : {
				tree.GetComponent<TreeChromosones>().circles++;
				break;
			}
			case ItemType.nice : {
				tree.GetComponent<TreeChromosones>().nice++;
				break;
			}
			case ItemType.nasty : {
				tree.GetComponent<TreeChromosones>().nasty++;
				break;
			}
			}
			KerChing();
			StartCoroutine(KerChing());
		}
	}
	IEnumerator KerChing() {
		gameObject.GetComponentInParent<AudioSource>().Play();
		yield return new WaitForSeconds(0.3f);
		Destroy (gameObject);
	}
}
