using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutterController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShutDown()
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(0f, 0f),
            "time", 0.5f,
            "easetype", iTween.EaseType.easeOutQuart,
            "islocal", true));
    }

    public void ShutUp()
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(0f, 1600f),
            "time", 0.5f,
            "easetype", iTween.EaseType.easeInQuart,
            "islocal", true));
    }
}
