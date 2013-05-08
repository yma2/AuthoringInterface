using UnityEngine;
using System.Collections;

public class DropTool : MonoBehaviour
{
	public GameObject character;
	public GameObject shotgun;
	public GameObject pistol;
	public GameObject knife;
	GameObject obj;
	bool toolState = false;
	int toolID = 0;
	RaycastHit info;
	bool rayCasting = false;
	private Camera c;
	// Use this for initialization
	void Start ()
	{
		character = GameObject.Find("GenericMale");
		shotgun = GameObject.Find("Item");
		pistol = GameObject.Find("GenericMale2");
		
		//character = (GameObject)Instantiate (character);
		//shotgun = (GameObject)Instantiate (shotgun);
		//pistol = (GameObject)Instantiate (pistol);
		knife = (GameObject)Instantiate (knife);
	}
	
	
	void moveObject(string name){
		
		rayCasting = true;
		obj = GameObject.Find(name);
		
				
				
				
		
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey (KeyCode.J)) {
			//Debug.Log("Drag and drop tool on");
			toolState = true;
			
		}
		
		/*
		if (Input.GetKey (KeyCode.Alpha7))
			toolID = 1;
		if (Input.GetKey (KeyCode.Alpha8))
			toolID = 2;
		if (Input.GetKey (KeyCode.Alpha9))
			toolID = 3;
		if (Input.GetKey (KeyCode.Alpha0))
			toolID = 4;
		*/
		
		
		if (toolState) {
			
			if (Input.GetMouseButton (0)) {
							rayCasting	= false;	
		
			}
			if(GameObject.Find ("FPSCamera") != null){
			 c = GameObject.Find ("FPSCamera").camera;
			}
			if (c != null) {
			
				Ray ray = c.ScreenPointToRay (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));
				Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
				if (rayCasting) {
					Physics.Raycast (ray, out info, 300f);
					
					obj.transform.position = info.point;
				
				}
			}
			
		}
	}
}
