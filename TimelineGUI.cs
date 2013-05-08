using UnityEngine;
using System;
using System.Collections;

public class TimelineGUI : MonoBehaviour {
	
	public GameObject boxHolder;
	public DraggableBox db;
	
	public Rect timelineRect;
	int heightOffset;
	int actorNumber;
	
	public Vector2[] snapPoints;
	public int[] snappedBoxIDs;
	public int numSnapPoints;
	int pageOffset;
	
	bool display = true;
	
	TimelineManager manager;
	
	
	
	// Use this for initialization
	void Start () {
		timelineRect = new Rect(Screen.width/4f, Screen.height - 50 - heightOffset, Screen.width, 50);
		
		manager = gameObject.GetComponent("TimelineManager") as TimelineManager;
		
		initializeSnapPoints();
		
		for (int i = 1; i < manager.numPages; i++) {
			addScrollPage();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void toggleGUI(){
		display = !display;
		
	}
	
	// OnGUI is called once per GUI Event
	void OnGUI () {
		//ability to turn off timeline GUI
		if(!display) return;
		
		
		GUI.Box (timelineRect, "Actor " + actorNumber + " - Page " + (pageOffset + 1));	//might use layers to draw this stuff - especially after integration
		
		if (GUI.Button (new Rect(Screen.width - 30, Screen.height - 30 - heightOffset, 20, 20), ">")) {
			scrollRight();
		}
		
		if (GUI.Button (new Rect(10+ Screen.width/4f, Screen.height - 30 - heightOffset, 20, 20), "<")) {
			scrollLeft();
		}

		
	}
	
	// based on screen size, initializes snap coordinates
	// also initializes snapped box ids
	// also initializes pageOffset for scrolling
	void initializeSnapPoints() {
		numSnapPoints = (Screen.width - 80 - Screen.width/4) / (DraggableBox.width + 4);
		manager.numSnapPoints = numSnapPoints;
		snapPoints = new Vector2[numSnapPoints];
		snappedBoxIDs = new int[numSnapPoints];
		
		Vector2 nextSnapPoint = new Vector2(Screen.width/4f + (DraggableBox.width / 2) + 44, (DraggableBox.height / 2) + 4 + heightOffset);
		
		for (int i = 0; i < numSnapPoints; i++) {
			//also initializes the snapped box ids to -1 here
			snappedBoxIDs[i] = -1;
			
			//initialize snap points
			snapPoints[i] = nextSnapPoint;
			nextSnapPoint = new Vector2(nextSnapPoint.x + DraggableBox.width + 4, nextSnapPoint.y);
		}
		
		pageOffset = 0;
		
		
	}
	
	
	// takes a game object as a parameter and snaps it to the closest
	// snap point on the timeline if it is actually dropped on the timeline
	// replaces the box currently in that location if one exists
	public Vector2 snapToClosest(GameObject obj) {
		int currentClosest = 0;
		float currentClosestDistance = 5000; //5000 should be way larger than any distance possilbe
		DraggableBox boxScript = obj.GetComponent("DraggableBox") as DraggableBox;
		Vector2 rectCenter = boxScript.rect.center;
		
		
		for (int j = 0; j < numSnapPoints; j++) {
			if (distance (rectCenter, snapPoints[j]) < currentClosestDistance) {
				currentClosest = j;
				currentClosestDistance = distance (rectCenter, snapPoints[j]);
			}
		}
		
		if (snappedBoxIDs[currentClosest + (numSnapPoints * pageOffset)] != -1) {
			destroyBoxWithID(snappedBoxIDs[currentClosest + (numSnapPoints * pageOffset)]);
			snappedBoxIDs[currentClosest + (numSnapPoints * pageOffset)] = obj.GetInstanceID();
			
		} else {
			snappedBoxIDs[currentClosest + (numSnapPoints * pageOffset)] = obj.GetInstanceID();
		}
			
		manager.recreateStory();
		return snapPoints[currentClosest];
	}
	
	public void unSnap(GameObject obj) {
		for (int i = 0; i < numSnapPoints; i++) {
			if (snappedBoxIDs[i] == obj.GetInstanceID()) {
				snappedBoxIDs[i] = -1;
			}
		}
		manager.recreateStory();
	}
				
	float distance(Vector2 a, Vector2 b) {
		return Mathf.Sqrt( Mathf.Pow((a.x - b.x), 2) + Mathf.Pow ((a.y - b.y), 2) );
	}
	
	void destroyBoxWithID (int id) {
		GameObject[] boxes = GameObject.FindGameObjectsWithTag("DraggableBox");
		foreach (GameObject obj in boxes) {
			if (obj.GetInstanceID() == id) {
				Destroy (obj);
			}
		}
	}
	
	// scrolls the view of the timeline to the left
	void scrollLeft() {
		if (pageOffset == 0) {
			// here you are already on the left most screen - so dont do anyting
			return;
		}
		
		GameObject[] boxes = GameObject.FindGameObjectsWithTag("DraggableBox");
		
		//iterate across the current pages boxes and deactivate them
		for (int i = (numSnapPoints * pageOffset); i < (numSnapPoints * pageOffset + numSnapPoints); i++) {
			if (snappedBoxIDs[i] != -1) {
				foreach (GameObject obj in boxes) {
					if (obj.GetInstanceID() == snappedBoxIDs[i]) {
						db = obj.GetComponent ("DraggableBox") as DraggableBox;
						db.hide();
					}
				}
			}
		
		}
		
		pageOffset--;
		
		//iterate across the next pages boxes and activate them
		for (int i = (numSnapPoints * pageOffset); i < (numSnapPoints * pageOffset + numSnapPoints); i++) {
			if (snappedBoxIDs[i] != -1) {
				foreach (GameObject obj in boxes) {
					if (obj.GetInstanceID() == snappedBoxIDs[i]) {
						db = obj.GetComponent ("DraggableBox") as DraggableBox; 
						db.unhide();
					}
				}
			}
		
		}
			
	}
	
	// check if next page exists - if not create it - if so view it
	void scrollRight() {
		
		pageOffset++;
			
		if (snappedBoxIDs.Length == pageOffset*numSnapPoints) {
			// new page needs to be created
			foreach (TimelineGUI t in manager.timelines) {
				t.addScrollPage();
			}
			manager.numPages++;
		}
		
		// next page exists - just view it
		// to view a page - activate all boxes on that page and deactivate all
		// boxes on current page
		
		GameObject[] boxes = GameObject.FindGameObjectsWithTag("DraggableBox");
		
		// iterate across the current pages boxes and deactivate them
		for (int i = (numSnapPoints * (pageOffset - 1)); i < (numSnapPoints * (pageOffset - 1) + numSnapPoints); i++) {
			if (snappedBoxIDs[i] != -1) {
				foreach (GameObject obj in boxes) {
					if (obj.GetInstanceID() == snappedBoxIDs[i]) {
						db = obj.GetComponent ("DraggableBox") as DraggableBox; 
						db.hide();
					}
				}
			}
		
		}
		
		// iterate across the next pages boxes and activate them
		for (int i = (numSnapPoints * pageOffset); i < (numSnapPoints * pageOffset + numSnapPoints); i++) {
			if (snappedBoxIDs[i] != -1) {
				foreach (GameObject obj in boxes) {
					if (obj.GetInstanceID() == snappedBoxIDs[i]) {
						db = obj.GetComponent ("DraggableBox") as DraggableBox; 
						db.unhide();
					}
				}
			}
		
		}
		
	}
	
	void addScrollPage() {
		int currentSize = snappedBoxIDs.Length;
		
		Array.Resize(ref snappedBoxIDs, currentSize + numSnapPoints);
		
		for (int i = currentSize; i < (currentSize + numSnapPoints); i++) {
			snappedBoxIDs[i] = -1;
		}
	}
	
	public void setActorNumber(int i) {
		actorNumber = i;
	}
	
	public void setHeightOffset(int o) {
		heightOffset = o;
	}
	
	public GameObject getBoxWithID (int id) {
		GameObject[] boxes = GameObject.FindGameObjectsWithTag("DraggableBox");
		foreach (GameObject obj in boxes) {
			if (obj.GetInstanceID() == id) {
				return obj;
			}
		}
		return null;
	}
		
}
