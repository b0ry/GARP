using UnityEngine;
using System.Collections;

public class CameraLook : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (Camera.main.transform);
		//transform.localEulerAngles = new Vector3 (0f, 180f, 0f);
	}
}
