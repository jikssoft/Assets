using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingController : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public GameObject loading_go;

	public void ShowLoading()
	{
		loading_go.SetActive (true);
	}

	public void HideLoading()
	{
		loading_go.SetActive (false);
	}

    public bool IsLoading()
    {
        return loading_go.activeSelf == true;
    }
}
