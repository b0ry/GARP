using UnityEngine;
using System.Collections;

public class Ard : MonoBehaviour {
	private Vector3 endPos;
	private float startTime;

	// Use this for initialization
	void Start () {
		float range = Random.Range (5f, 10f);
		float angle = Mathf.Deg2Rad*Random.Range (0f, 360f);
		float x = range * Mathf.Sin (angle);
		float z = range * Mathf.Cos (angle);
		endPos = new Vector3 (transform.localPosition.x + x, transform.localPosition.y, transform.localPosition.z + z);
		Debug.Log (endPos);
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		float time = Random.Range (2f, 3f);
		Vector3 center = (transform.localPosition + endPos) * 0.5f;
		center -= new Vector3(0f, 3f, 0f);
		Vector3 riseRelCenter = transform.localPosition - center;
		Vector3 setRelCenter = endPos - center;
		float fracComplete = (Time.time - startTime) / time;
		transform.localPosition = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
		transform.localPosition += center;
	}
}
