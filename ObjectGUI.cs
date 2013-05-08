using UnityEngine;
using System;
using System.Collections;

public class ObjectGUI : MonoBehaviour {
	public Rect objectRect;
	int objectNumber;
	string objectName;
	string objectType;
	
	// Use this for initialization
	void Start () {
		objectRect = new Rect(Screen.width - 150f, 50f + 55f*objectNumber, 140f, 50f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	// OnGUI is called once per GUI Event
	void OnGUI () {
		GUI.depth = 0;
		GUI.Box (objectRect, objectName);
	}
	
	public void setObjectNumber(int i) {
		objectNumber = i;
	}
	
	public void setObjectName(string name){
		objectName = name;
	}
	
	public void setObjectType(string type){
		objectType = type;
	}
}
