using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AuthoringGUI : MonoBehaviour {
	
	List<string> actorList = new List<string>();
	List<string> objectList = new List<string>();
	
	private int selectedActor = 0;
	private int selectedObject = 0;
	
	public GameObject genericMale;
	public GameObject shotgun;
	public GameObject knife;
	
	private string name = "";
	private string type = "";
	
	bool actorCreation = false;
	bool objectCreation = false;
	
	bool displayOn = false;
	Rect timelineRect = new Rect(Screen.width/6f, Screen.height - Screen.height/4f, Screen.width*5f/6f, Screen.height/4f);
	Rect sideDisplayRect = new Rect(Screen.width*3f/4f, Screen.height/8f, Screen.width/4f, Screen.height*3f/4f - Screen.height/8f);
	
	private enum sideBarState{
		actors,	
		objects,
	}
	
	private sideBarState state;
	// Use this for initialization
	void Start () {
		state = sideBarState.actors;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.J)){
			toggleDisplay();
			
			
			
			GameObject.Find("Chat").SendMessage("toggleChatWindow");
			GameObject c = GameObject.Find ("FPSCamera");
			
			if (c != null) {
				c.transform.parent.gameObject.SendMessage("toggleCameraLock");	
				
			}
			
			GameObject.Find("FPSMenu").SendMessage("toggleCommandsOpen");
		}
	
	}
	
	void toggleDisplay(){
		
		displayOn = !displayOn;	
		gameObject.GetComponent<TimelineManager>().enabled = !gameObject.GetComponent<TimelineManager>().enabled;
		gameObject.SendMessage("toggleGUI");
		if(GameObject.FindGameObjectsWithTag("DraggableBox") != null){
			GameObject[] dbList = GameObject.FindGameObjectsWithTag("DraggableBox");
			foreach (GameObject go in dbList){
				go.SendMessage("toggleDisplay");	
			}
				
		}
		
	}
	
	void OnGUI(){
		if(!displayOn) return;
		 GUI.Box(timelineRect, "");
		 GUI.Label(new Rect(timelineRect.x, timelineRect.y, 70f, 35f),"Timeline: ");
		 GUI.Box(sideDisplayRect, "");
		 if(GUI.Button(new Rect(sideDisplayRect.x,sideDisplayRect.y,sideDisplayRect.width/2, 35f), "Actors")){
			state = sideBarState.actors;
		}
		if(GUI.Button(new Rect(sideDisplayRect.x + sideDisplayRect.width/2,sideDisplayRect.y,sideDisplayRect.width/2, 35f), "Objects")){
			state = sideBarState.objects;
			
		}
		//GUI.Box(timelineRect, "Timeline");
		switch(state){
		case sideBarState.actors:
			float actorGridHeight = actorList.Count*35f;
			if(actorGridHeight > sideDisplayRect.height - 70f) actorGridHeight = sideDisplayRect.height - 70f;
			selectedActor = GUI.SelectionGrid(new Rect(sideDisplayRect.x, sideDisplayRect.y+35f, sideDisplayRect.width, actorGridHeight), selectedActor, actorList.ToArray(), 1);
			if(GUI.Button(new Rect(sideDisplayRect.x, sideDisplayRect.y+sideDisplayRect.height - 35f, sideDisplayRect.width/4f, 35f), "Add")){
				actorCreation = true;
			}
			if(GUI.Button(new Rect(sideDisplayRect.x+ sideDisplayRect.width*1/4, sideDisplayRect.y+sideDisplayRect.height - 35f, sideDisplayRect.width/4f, 35f), "Edit")){
				
			}
			if(GUI.Button(new Rect(sideDisplayRect.x + sideDisplayRect.width*2/4, sideDisplayRect.y+sideDisplayRect.height - 35f, sideDisplayRect.width/4f, 35f), "Move")){
				GameObject.Find("MouseTool").SendMessage("moveObject", actorList[selectedActor]);
			}
			if(GUI.Button(new Rect(sideDisplayRect.x+ sideDisplayRect.width*3/4, sideDisplayRect.y+sideDisplayRect.height - 35f, sideDisplayRect.width/4f, 35f), "Delete")){
				GameObject.Destroy(GameObject.Find(actorList[selectedActor]));
				actorList.RemoveAt(selectedActor);
			}
			break;
		case sideBarState.objects:
			float objectGridHeight = objectList.Count*35f;
			if(objectGridHeight > sideDisplayRect.height - 70f) objectGridHeight = sideDisplayRect.height - 70f;
			selectedObject = GUI.SelectionGrid(new Rect(sideDisplayRect.x, sideDisplayRect.y+35f, sideDisplayRect.width, objectGridHeight), selectedObject, objectList.ToArray(), 1);
			if(GUI.Button(new Rect(sideDisplayRect.x, sideDisplayRect.y+sideDisplayRect.height - 35f, sideDisplayRect.width/4f, 35f), "Add")){
				objectCreation = true;
			}
			if(GUI.Button(new Rect(sideDisplayRect.x+ sideDisplayRect.width*1/4, sideDisplayRect.y+sideDisplayRect.height - 35f, sideDisplayRect.width/4f, 35f), "Edit")){
				
			}
			if(GUI.Button(new Rect(sideDisplayRect.x+ sideDisplayRect.width*2/4, sideDisplayRect.y+sideDisplayRect.height - 35f, sideDisplayRect.width/4f, 35f), "Move")){
				GameObject.Find("MouseTool").SendMessage("moveObject", objectList[selectedObject]);
			}
			if(GUI.Button(new Rect(sideDisplayRect.x+ sideDisplayRect.width*3/4, sideDisplayRect.y+sideDisplayRect.height - 35f, sideDisplayRect.width/4f, 35f), "Delete")){
				GameObject.Destroy(GameObject.Find(objectList[selectedObject]));
				objectList.RemoveAt(selectedObject);
			}
			break;
			
			
		
			
		}
		
		if(actorCreation)
		GUI.Window (21,new Rect((Screen.width * .5f) - 150f, (Screen.height * .5f) - 200f,400f,300f),createNewActor,"New Actor");
		
		if(objectCreation)
		GUI.Window (21,new Rect((Screen.width * .5f) - 150f, (Screen.height * .5f) - 200f,400f,300f),createNewObject,"New Object");
		
		
		
	}
	
	
	void createNewActor(int windowID){
		GUI.Label (new Rect(5f,30f,75f,20f), "Name: ");
		GUI.Label (new Rect(5f,55f,75f,20f), "Type: ");
		
		name = GUI.TextField(new Rect (85f,30f,100f,25f), name, 25);
		type = GUI.TextField(new Rect (85f,55f,100f,25f), type, 10);
		
		if(GUI.Button (new Rect(85f, 260f, 110f, 30f), "Create Actor")){
			
			actorList.Add(name);
			GameObject actor;
			Quaternion rot = Quaternion.identity;
			if(actorList.Count %2 == 0){
				rot.SetLookRotation(Vector3.left);
				actor = (GameObject)GameObject.Instantiate(genericMale, Vector3.zero, rot);
				actor.tag = "StoryObject";
			}else{
				rot.SetLookRotation(Vector3.right);
				actor = (GameObject)GameObject.Instantiate(genericMale, Vector3.zero, rot);
				actor.tag = "StoryObject";
			}
			actor.name = name;
			actor.transform.parent = GameObject.Find("Lerpz Island Example").transform;
			
			actorCreation = !actorCreation;
			name = string.Empty;
			type = string.Empty;
		}
		
		if(GUI.Button (new Rect(205f, 260f, 110f, 30f), "Cancel")){
			actorCreation = !actorCreation;
			name = string.Empty;
			type = string.Empty;
		}
	}
	
	void createNewObject(int windowID){
		GUI.Label (new Rect(5f,30f,75f,20f), "Name: ");
		GUI.Label (new Rect(5f,55f,75f,20f), "Type: ");
		
		name = GUI.TextField(new Rect (85f,30f,100f,25f), name, 25);
		type = GUI.TextField(new Rect (85f,55f,100f,25f), type, 10);
		
		if(GUI.Button (new Rect(85f, 260f, 110f, 30f), "Create Object")){
			
			objectList.Add(name);
			GameObject obj;
			Quaternion rot = Quaternion.identity;
			if(objectList.Count %2 == 0){
				rot.SetLookRotation(Vector3.left);
				obj = (GameObject)GameObject.Instantiate(shotgun, Vector3.zero, rot);
				obj.tag = "StoryObject";
			}else{
				rot.SetLookRotation(Vector3.right);
				obj = (GameObject)GameObject.Instantiate(shotgun, Vector3.zero, rot);
				obj.tag = "StoryObject";
			}
			obj.name = name;
			obj.transform.parent = GameObject.Find("Lerpz Island Example").transform;
			
			objectCreation = !objectCreation;
			name = string.Empty;
			type = string.Empty;
		}
		
		if(GUI.Button (new Rect(205f, 260f, 110f, 30f), "Cancel")){
			objectCreation = !objectCreation;
			name = string.Empty;
			type = string.Empty;
		}
	}

	
	
}
