using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeCamera : MonoBehaviour {

    private float DesignOrthographicSize;
    private float DesignAspect;
    private float DesignWidth;

    public float DesignAspectHeight = 1280f;
    public float DesignAspectWidth = 720f;

    public void Awake()
    {
        Camera target_camera =  GetComponent<Camera>();
        this.DesignOrthographicSize = target_camera.orthographicSize;
        this.DesignAspect = this.DesignAspectHeight / this.DesignAspectWidth;
        this.DesignWidth = this.DesignOrthographicSize * this.DesignAspect;

        this.Resize();
    }

    public void Resize()
    {
        Camera target_camera = GetComponent<Camera>();
        float wantedSize = this.DesignWidth / target_camera.aspect;
		Debug.Log (wantedSize.ToString () + " " + this.DesignOrthographicSize.ToString ());
        target_camera.orthographicSize = Mathf.Max(wantedSize,
        this.DesignOrthographicSize);
    }
}
