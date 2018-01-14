using UnityEngine;
using System.Collections;

public class CameraTweener : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        float i = 0f;
        if (size_duration > 0f)
        {
            i = (Time.fixedTime - size_startTime) / size_duration;
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(size_start_value, size_end_value, i);
        }

        if(i > 1.0f)
        {
            GetComponent<Camera>().orthographicSize = size_end_value;
            size_duration = 0f;
        }

        i = 0f;
        if (rotation_duration > 0f)
        {
            i = (Time.fixedTime - rotation_startTime) / rotation_duration;
            transform.rotation = Quaternion.Lerp(rotation_start_value, rotation_end_value, i);
        }

        if (i > 1.0f)
        {
            transform.rotation = rotation_end_value;
            rotation_duration = 0f;
        }
    }

    float size_startTime;
    float size_start_value;
    float size_end_value;
    float size_duration;

    public void SetSize(float size, float time)
    {
        size_start_value = GetComponent<Camera>().orthographicSize;
        size_end_value = size;
        size_startTime = Time.fixedTime;
        size_duration = time;
    }

    float rotation_startTime;
    Quaternion rotation_start_value;
    Quaternion rotation_end_value;
    float rotation_duration;

    public void SetRotation(float zAxis, float time)
    {
        rotation_startTime = Time.fixedTime;
        rotation_start_value = transform.rotation;
        rotation_end_value = Quaternion.Euler(0f, 0f, zAxis);

        Debug.Log(rotation_start_value + " " + rotation_end_value);
        rotation_duration = time;
    }
}
