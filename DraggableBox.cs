using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DraggableBox : MonoBehaviour
{
	
	bool dragging;
	bool hidden;
	bool spawner;
	bool paramEdit;
	public Rect rect;
	int id;
	TimelineGUI[] timelines;
	

	
	//Gamestate hide, serves authoring interface turning on and off
	bool display = true;
	public const int width = 100;
	public const int height = 40;
	public string actionName;
	public List<string> actionParamNames;
	public List<string> actionParamValues;

	// Use this for initialization
	void Start ()
	{
	
		
		hidden = false;
		paramEdit = false;
		if (actionName == null) {
			actionName = "";
		}
		if (actionParamNames == null) {
			actionParamNames = new List<string> ();
		}
		if (actionParamValues == null) {
			actionParamValues = new List<string> ();
		}
		if (spawner == null) {
			spawner = false;
		}
		
		
		tag = "DraggableBox";
		if (dragging == null) {
			dragging = false;
		}
		
		if (id == null) {
			id = 0;
		}
		rect = new Rect (Screen.width / 4 + 30 + (width + 10) * id, Screen.height * 3 / 4, width, height);

		
		timelines = GameObject.Find ("AuthoringGUI").GetComponents<TimelineGUI> ();
	}
	
	void toggleDisplay ()
	{
		display = !display;	
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		if (hidden) {
			return;
		}
		
		if (spawner) {
			return;
		}
		
		if (dragging) {
			moveTo (Input.mousePosition.x, Screen.height - Input.mousePosition.y);
		}
	}
	
	// OnGUI is called once per GUI event
	void OnGUI ()
	{
		if (!display)
			return;
		
		if (hidden) {
			return;
		}
		
		if (rect.Contains (new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y))) {
			if (Event.current.type == EventType.MouseDown) {
				if (spawner) {
					GameObject obj = Instantiate (gameObject) as GameObject;
					DraggableBox db = obj.GetComponent ("DraggableBox") as DraggableBox;
					db.turnOnDrag ();
					db.setID (id);
					
					//really - when you add actions to these boxes, youll want to
					//add a method within this class like setAction() - similar to
					//setID(), so when you clone the object, you can clone all of 
					//the parameters as well, and only initialize null stuff in start()
					//side note: in a similar manner you could associate 1 timeline with 1 actor
					
					return;
				}
				if (Event.current.button == 1) {
					//right click - open param menu
					openParamWindow ();
					return;
				}
				foreach (TimelineGUI timeline in timelines) {
					if (timeline.timelineRect.Contains (new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y))) {
						timeline.unSnap (gameObject);
					}
				}
				dragging = true;
			}
		}
		
		if (Event.current.type == EventType.MouseUp) {
			if (dragging) {
				
				moveTo (Input.mousePosition.x, Screen.height - Input.mousePosition.y);
				dragging = false;
				
				bool toDestroy = true;
				
				foreach (TimelineGUI timeline in timelines) {
					if (timeline.timelineRect.Contains (new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y))) {
						Vector2 newCenter = timeline.snapToClosest (gameObject);
						newCenter.y = Screen.height - newCenter.y;
						moveTo (newCenter);
						toDestroy = false;
					} 
				}
				
				if (toDestroy) {
					Destroy (gameObject);
				}
			}
		}
		
		switch (id) {
		case 0: 	
			GUI.Box (rect, ("Shotgun Cock"));
			break;
		case 1:
			GUI.Box (rect, ("Pick UP"));
			break;
		case 2:
			GUI.Box (rect, ("Put Down"));
			
			break;
		case 3:
			GUI.Box (rect, ("Wait"));
			break;
		case 4:
			GUI.Box (rect, ("Cower"));
			break;
		case 5:
			GUI.Box (rect, ("Move To"));
			break;
			
			
		}
		//GUI.Box (rect, ("Box " + id));
		
		
		
		
		if (paramEdit) {
			//set up specifics for ease of use
			float left = Screen.width * .1f;
			float top = Screen.height * .2f;
			float gap = Screen.width * .02f;
			float nameW = Screen.width * .58f;
			float closeW = Screen.width * .06f;
			float rowHeight = 20;
			float rowGap = 10;
			float nameEW = Screen.width * .15f;
			float paramEW = Screen.width * .20f;
			
			//bounding box
			GUI.Box (new Rect (left, top, Screen.width * .4f, (actionParamNames.Count + 1) * 30), "");
			
			//first row - name and close - always exists as such
			GUI.Label (new Rect (left + gap, top + rowGap, 2 * closeW, rowHeight), "Name:");
			GUI.Label (new Rect (left + 2 * gap + 2 * closeW, top + rowGap, nameW, rowHeight), actionName);
			if (GUI.Button (new Rect (left + Screen.width * .40f - gap - closeW, top + rowGap, closeW, rowHeight), "x")) {
				paramEdit = false;
				//on clicking close button, the XML get generated again
				GameObject.Find("AuthoringGUI").SendMessage("recreateStory");
			}
			
			//parameter rows
			for (int i = 0; i < actionParamNames.Count; i++) {
				
				actionParamNames [i] = GUI.TextArea (new Rect (left + gap, top + (i + 1) * (rowHeight + rowGap), nameEW, rowHeight), actionParamNames [i]);
				Rect valRec = new Rect (left + 2 * gap + nameEW, top + (i + 1) * (rowHeight + rowGap), paramEW, rowHeight);
				actionParamValues [i] = GUI.TextArea (valRec, actionParamValues [i]);
				Rect valButton = new Rect (left + 2 * gap + nameEW + paramEW, top + (i + 1) * (rowHeight + rowGap), 22.0f, rowHeight);
				
				bool show = false;
				
				if (GUI.Button (valButton, "O")) {
					show = !show;
				}
				
				if(!(actionParamNames[i] == "ACTOR" || actionParamNames[i] == "TARGET" || actionParamNames[i] == "FROM" || actionParamNames[i] == "TO")) continue;
				
				//if (show) {
					
					if (GameObject.FindGameObjectsWithTag ("StoryObject") != null) {
						GameObject[] storyObjects = GameObject.FindGameObjectsWithTag ("StoryObject");
						int index = 0;
						foreach (GameObject go in storyObjects) {
							//Debug.Log (go.name);
							if (GUI.Button (new Rect (left + 2 * gap + nameEW + paramEW+ 22.0f + 70.0f*index, top + (i + 1) * (rowHeight + rowGap) , 70.0f, rowHeight), go.name)) {
								actionParamValues [i] = go.name;	
								show = false;
							}
							
							index ++;
							
						}
						
					}
					
				//}
				
				
				
			}
		}
	}
	
	// moves the center of the box to the specified point
	public void moveTo (float x, float y)
	{
		rect.center = new Vector2 (x, y);
	}
	
	// moves the center of the box to the specified vector coordinate
	public void moveTo (Vector2 v)
	{
		rect.center = v;
	}
	
	public void hide ()
	{
		hidden = true;
	}
	
	public void unhide ()
	{
		hidden = false;
	}
	
	public void makeSpawner ()
	{
		spawner = true;
	}
		
	public void turnOnDrag ()
	{
		dragging = true;
	}
	
	public void setID (int i)
	{
		id = i;
	}
	
	public void setActionName (string n)
	{
		actionName = n;
	}
	
	public void setActionParamNames (List<string> s)
	{
		actionParamNames = new List<string> ();
		foreach (string str in s) {
			actionParamNames.Add (str);
		}
	}
	
	public void setActionParamValues (List<string> s)
	{
		actionParamValues = new List<string> ();
		foreach (string str in s) {
			actionParamValues.Add (str);
		}
	}
	
	void openParamWindow ()
	{
		paramEdit = true;
	}
}
