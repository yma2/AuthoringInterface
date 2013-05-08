using UnityEngine;
using System.Collections;

public class BolaMecanimTest : MonoBehaviour {
	Animator animator;
	// Use this for initialization
	void Start () {
	animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1)){
			
		animator.SetInteger("AnimationCode", 1);	
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)){
			
		animator.SetInteger("AnimationCode", 2);	
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)){
			
		animator.SetInteger("AnimationCode", 3);	
		}
		if(Input.GetKeyDown(KeyCode.Alpha4)){
			
		animator.SetInteger("AnimationCode", 4);	
		}
		if(Input.GetKeyDown(KeyCode.BackQuote)){
			
		animator.SetInteger("AnimationCode", 0);	
		}
		
	}
}
