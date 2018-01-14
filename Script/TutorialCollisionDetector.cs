using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCollisionDetector : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public TutorualController tutorial_controller;

    string box_name = "Tutorial_Box";
    void OnCollisionEnter(Collision coll)
    {
        Debug.Log("impact1 ==== " + coll.gameObject.name);
        if (coll.gameObject.name.Equals(box_name) == true)
        {
            tutorial_controller.Success();
        }
    }

    void OnTriggerEnter(Collision coll)
    {
        Debug.Log("impact2 ==== " + coll.gameObject.name);
        if (coll.gameObject.name.Equals(box_name) == true)
        {
            tutorial_controller.Success();
        }
    }
}
