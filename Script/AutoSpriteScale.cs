using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSpriteScale : MonoBehaviour {

    float unit_camera_size = 6.4f;

	// Use this for initialization
	void Start () {

        float current_camera_size = Camera.main.orthographicSize;

        float scale = current_camera_size / unit_camera_size;

        gameObject.transform.localScale = new Vector3(scale, scale, 1f);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
