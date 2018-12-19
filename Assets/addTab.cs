using UnityEngine;
using System.Collections;

public class addTab : MonoBehaviour {

	public GameObject tab;

	public void createTab(float val){
		Instantiate (tab);
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
