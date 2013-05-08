using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimelineManager : MonoBehaviour {
	
	public ArrayList timelines;
	public GameObject boxHolder;
	public DraggableBox db;
	public int numPages;
	bool[]  emptyCols;
	string[] emptyColValues;
	public int numSnapPoints;
	
	bool firstRun;
	
	StoryManager inputManager;
	List<int> lastStepIDs;
	
	// Use this for initialization
	void Start () {
		
		timelines = new ArrayList();
		numPages = 1;
		lastStepIDs = new List<int>();
		lastStepIDs.Add(-1);
		firstRun = true;
		
		inputManager = new StoryManager();
		
		initializeSpawnBox();
		
		initializeColArrays();
		
		//dont forget to set actor number and heigh offset!!
		//timelines are 50 pixels in height
		TimelineGUI t1 = gameObject.AddComponent("TimelineGUI") as TimelineGUI;
		t1.setActorNumber(0);
		t1.setHeightOffset(0);
		
		timelines.Add(t1);
		
		TimelineGUI t2 = gameObject.AddComponent("TimelineGUI") as TimelineGUI;
		t2.setActorNumber(1);
		t2.setHeightOffset(50);
		
		timelines.Add(t2);
	}
	
	// Update is called once per frame
	void Update () {
		checkIdles ();
	}
	
	// OnGUI is called once per GUI Event
	void OnGUI() {
		if (GUI.Button (new Rect(Screen.width-30, Screen.height - (timelines.Count+1)*50 + 20, 20, 20), "+")) {
			addTimeline();
		}
		
		for (int i = 0; i < numSnapPoints; i++) {
			if (emptyCols[i]) {
				TimelineGUI t = timelines[0] as TimelineGUI;
				emptyColValues[i] = GUI.TextArea (new Rect(t.snapPoints[i].x - 20, Screen.height - (timelines.Count)*50 - 30, 40, 20), emptyColValues[i]);
			}
		}
	}
	
	void initializeSpawnBox() {	
		string name = "";
		List<string> paramNames = new List<string>();
		List<string> paramValues = new List<string>();
		
		boxHolder = new GameObject("Draggable Box");
		boxHolder.AddComponent("DraggableBox");
		db = boxHolder.GetComponent("DraggableBox") as DraggableBox;
		db.makeSpawner();
		db.setID (0);
		name = "SHOTGUNFIRECOCK";
		paramNames.Add("ACTOR");
		paramNames.Add("TARGET");
		paramValues.Add("GenericMale");
		paramValues.Add("Item");
		db.setActionName (name);
		db.setActionParamNames(paramNames);
		db.setActionParamValues(paramValues);
		
		paramNames.Clear();
		paramValues.Clear();
		
		boxHolder = new GameObject("Draggable Box");
		boxHolder.AddComponent("DraggableBox");
		db = boxHolder.GetComponent("DraggableBox") as DraggableBox;
		db.makeSpawner();
		db.setID (1);
		name = "PICKUP";
		paramNames.Add("ACTOR");
		paramNames.Add("TARGET");
		paramValues.Add("GenericMale");
		paramValues.Add("Item");
		db.setActionName (name);
		db.setActionParamNames(paramNames);
		db.setActionParamValues(paramValues);
		
		paramNames.Clear();
		paramValues.Clear();
		
		boxHolder = new GameObject("Draggable Box");
		boxHolder.AddComponent("DraggableBox");
		db = boxHolder.GetComponent("DraggableBox") as DraggableBox;
		db.makeSpawner();
		db.setID (2);
		name = "PUTDOWN";
		paramNames.Add("ACTOR");
		paramNames.Add("TARGET");
		paramValues.Add("GenericMale");
		paramValues.Add("Item");
		db.setActionName (name);
		db.setActionParamNames(paramNames);
		db.setActionParamValues(paramValues);
		
		paramNames.Clear();
		paramValues.Clear();
		
		boxHolder = new GameObject("Draggable Box");
		boxHolder.AddComponent("DraggableBox");
		db = boxHolder.GetComponent("DraggableBox") as DraggableBox;
		db.makeSpawner();
		db.setID (3);
		name = "WAIT";
		paramNames.Add("ACTOR");
		paramNames.Add("Time");
		paramValues.Add("GenericMale2");
		paramValues.Add(".5");
		db.setActionName (name);
		db.setActionParamNames(paramNames);
		db.setActionParamValues(paramValues);
		
		paramNames.Clear();
		paramValues.Clear();
			
		boxHolder = new GameObject("Draggable Box");
		boxHolder.AddComponent("DraggableBox");
		db = boxHolder.GetComponent("DraggableBox") as DraggableBox;
		db.makeSpawner();
		db.setID (4);
		name = "TAKINGHITWITHCOWER";
		paramNames.Add("ACTOR");
		paramValues.Add("GenericMale2");
		db.setActionName (name);
		db.setActionParamNames(paramNames);
		db.setActionParamValues(paramValues);
		
		paramNames.Clear();
		paramValues.Clear();
		
		boxHolder = new GameObject("Draggable Box");
		boxHolder.AddComponent("DraggableBox");
		db = boxHolder.GetComponent("DraggableBox") as DraggableBox;
		db.makeSpawner();
		db.setID (5);
		name = "MOVETO";
		paramNames.Add("ACTOR");
		paramNames.Add("FROM");
		paramNames.Add("TO");
		paramNames.Add("SPEED");
		paramNames.Add("TURNFIRST");
		paramNames.Add("ANIMATE");
		paramNames.Add("AVOIDOBJECTS");
		paramValues.Add("GenericMale2");
		paramValues.Add("GenericMale");
		paramValues.Add("Spawn");
		paramValues.Add("1");
		paramValues.Add("false");
		paramValues.Add("true");
		paramValues.Add("false");
		db.setActionName (name);
		db.setActionParamNames(paramNames);
		db.setActionParamValues(paramValues);
		
		paramNames.Clear();
		paramValues.Clear();
		
	}
	
	// Adds a new timeline
	// assumes at least 1 timeline already exists
	
	void addTimeline() {
		TimelineGUI t = gameObject.AddComponent("TimelineGUI") as TimelineGUI;
		t.setActorNumber (timelines.Count);
		t.setHeightOffset (timelines.Count* 50);
		
		timelines.Add(t);
	}
	
	void checkIdles() {
		if (firstRun) {
			initializeColArrays();
			firstRun = false;
		}
		
		TimelineGUI t;
		bool isEmpty = true;
		for (int i = 0; i < numSnapPoints; i++) {
			isEmpty = true;
			for (int j = 0; j < timelines.Count; j++) {
				t = timelines[j] as TimelineGUI;
				if (t.snappedBoxIDs != null) {
					if (t.snappedBoxIDs[i] != -1) {
						isEmpty = false;
					}
				}
			}
			
			emptyCols[i] = isEmpty;
				
		}
	}
	
	void initializeColArrays() {
		emptyCols = new bool[numSnapPoints];
		emptyColValues = new string[numSnapPoints];
		
		for (int i = 0; i < numSnapPoints; i++) {
			emptyColValues[i] = "0";
		}
	}
	
	public void recreateStory() {
		TimelineGUI t;
		List<int> stepIDs = new List<int>();
        inputManager = new StoryManager();
		for (int i = 0; i < numPages*numSnapPoints; i++) {
			lastStepIDs.Clear ();
			lastStepIDs.AddRange(stepIDs);
			stepIDs.Clear();
			for (int j = timelines.Count-1; j >= 0; j--) {
				t = timelines[j] as TimelineGUI;
				if (t.snappedBoxIDs[i] != -1) {
					DraggableBox box = t.getBoxWithID(t.snappedBoxIDs[i]).GetComponent("DraggableBox") as DraggableBox;
					stepIDs.Add(inputManager.addStep(box.actionName, box.actionParamNames, 
						box.actionParamValues, lastStepIDs));
				}
			}
		}
		inputManager.generateXML();
	}
	
	
	
}
