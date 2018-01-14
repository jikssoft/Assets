using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSetting : MonoBehaviour {

	// Use this for initialization
	void Start () {

        UIButtonScale[] buttons = (UIButtonScale[])(Resources.FindObjectsOfTypeAll(typeof(UIButtonScale)));

        foreach(UIButtonScale button_scale in buttons)
        {
            button_scale.duration = 0.1f;
            button_scale.pressed = new Vector3(0.9f, 0.9f, 0.9f);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}


