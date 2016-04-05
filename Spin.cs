using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {
	float rotationAmount = 180.0f;
	float i = 0.1f;
	void Update()
	{
		transform.localScale = new Vector3 (i,i,i);
		Vector3 rot = transform.rotation.eulerAngles;
		rot.y = rot.y + rotationAmount * Time.deltaTime;
		if(rot.y > 360){
			rot.y -= 360;
		}
		else if(rot.y < 360){
			rot.y += 360;
		}
		transform.eulerAngles = rot;
		i = i+0.05f;
	}
}
