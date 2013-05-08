using UnityEngine;
using System;
using System.Collections;

public class ActorGUI : MonoBehaviour {
	public Rect actorRect;
	public int actorNumber;
	public string actorName;
	public string actorGender;
	
	// Use this for initialization
	void Start () {
		actorRect = new Rect(Screen.width - 150f, 50f + 55f*actorNumber, 140f, 50f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	// OnGUI is called once per GUI Event
	void OnGUI () {
		GUI.depth = 0;
		GUI.Box (actorRect, actorName);
	}
	
	public void setActorNumber(int i) {
		actorNumber = i;
	}
	
	public void setActorName(string name){
		actorName = name;
	}
	
	public void setActorGender(string gender){
		actorGender = gender;
	}
}
