using UnityEngine;
using System.Collections;

public class Walk : MonoBehaviour {
	public GameObject smoke;
	public float initial = 0f;
	public float scale;
	public float startpos;
	private Color normal;
	public bool flag = false;
	public bool enumFlag = false;
	public int damage;
	private float timer = 0f;

	void Start () {
		startpos = transform.localPosition.y;
		normal = gameObject.GetComponent<Renderer>().material.color;
	}

	void FixedUpdate () 
		{
		timer += Time.deltaTime;
	if( initial < 3f ||
			(Input.GetKey(KeyCode.W) || 
		    Input.GetKey(KeyCode.A) || 
		    Input.GetKey(KeyCode.D) || 
		    Input.GetKey(KeyCode.UpArrow) || 
		    Input.GetKey(KeyCode.LeftArrow) || 
		    Input.GetKey(KeyCode.RightArrow)) && !flag)
		{
		scale = Mathf.Abs(Mathf.Sin(initial));
		initial = initial + 0.05f;
		transform.localPosition = new Vector3(transform.localPosition.x,startpos+scale,transform.localPosition.z);
		
		if (initial >= 3.2f) {
				GameObject player = GameObject.Find("MyPlayer");
				bool cooldownFlag = player.GetComponent<ThirdPersonController>().flag;
				initial = 0f;
				if(Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift) && !flag && !enumFlag && cooldownFlag){
					GameObject m = (GameObject)Instantiate(smoke, transform.parent.position, transform.parent.rotation);
			}
		}
		}
		// Block
		if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) && !enumFlag){
				flag = true;
			}
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) && !enumFlag){
			flag = true;
			damage= Random.Range (1,11);
			float rnd = (float)damage/40f;
			gameObject.GetComponent<Renderer>().material.color = new Color (rnd,rnd,rnd+0.1f,1f);
			if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S) && timer > 0.5f && !enumFlag){
				gameObject.GetComponent<Renderer>().material.color = normal;
				flag = false;
				strafe("Left");
				StartCoroutine(strafe("Left"));
				//enumFlag = true;
			}
			if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S) && timer > 0.5f && !enumFlag){
				gameObject.GetComponent<Renderer>().material.color = normal;
				flag = false;
				strafe("Right");
				StartCoroutine(strafe("Right"));
				//enumFlag = true;
			}
		}
		// Not Block
		if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.UpArrow) || !Input.anyKey)
		{
			StopAllCoroutines();
			gameObject.GetComponent<Renderer>().material.color = normal;
			flag = false;
			enumFlag = false;
		}
	}
	IEnumerator strafe (string lr){
		enumFlag = true;
		int level;
		float speed = 0f;
		float sine = Time.deltaTime*Mathf.Sin ((transform.parent.rotation.eulerAngles.y)*Mathf.PI/180f);
		float cosine = Time.deltaTime*Mathf.Cos ((transform.parent.rotation.eulerAngles.y)*Mathf.PI/180f);
		if(gameObject.name == ("Sphere")){
			level = gameObject.GetComponent<PlayerStrafeGA>().level;
			if (level == 1){
				speed = Random.Range (5.0f,10.0f);
				sine *= speed;
				cosine *= speed;
			}
			else {
				speed = gameObject.GetComponent<PlayerStrafeGA>().strafeOUT;
				speed += Random.Range (5.0f,10.0f);
				sine *= speed;
				cosine *= speed;
			}
		}
		else{
			level = GameObject.Find("Sphere").GetComponent<PlayerStrafeGA>().level;
			if (level == 1){
				speed = Random.Range (5.0f,10.0f);
				sine *= speed;
				cosine *= speed;
			}
			else {
				speed = GameObject.Find("Sphere").GetComponent<PlayerStrafeGA>().strafeOUT;
				speed += Random.Range (5.0f,10.0f);
				sine *= speed;
				cosine *= speed;
			}
		}
		for (int i = 0; i < 10; i++){
			if (lr == "Left"){
				transform.parent.localPosition -= new Vector3(cosine,0f,-sine);
				transform.Rotate(Camera.main.transform.forward, 500f * Time.deltaTime);
			}
			if (lr == "Right"){
				transform.parent.localPosition += new Vector3(cosine,0f,-sine);
				transform.Rotate(Camera.main.transform.forward, -500f * Time.deltaTime);
			}
			gameObject.GetComponent<Renderer>().material.color = normal;
			yield return new WaitForSeconds(0.01f);
		}
		timer = 0f;
		if(gameObject.tag == ("sphere")){
			enumFlag = false;
			gameObject.GetComponent<PlayerStrafeGA>().AddToList(speed);
		}
		enumFlag = false;
	}
}