using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDrillItem : MonoBehaviour {

    public Shader gray_shaer;
    UITexture main_texture;

	// Use this for initialization
	void Start () {
        main_texture = GetComponent<UITexture>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetGrayScale()
    {
        main_texture = GetComponent<UITexture>();
        main_texture.color = new Color(1f, 1f, 1f);
        main_texture.shader = gray_shaer;
    }

    public void SetBlack()
    {
        main_texture = GetComponent<UITexture>();
        main_texture.color = new Color(0f, 0f, 0f);
        main_texture.shader = null;
    }

    public void SetDefaultColor()
    {
        main_texture = GetComponent<UITexture>();
        main_texture.color = new Color(1f, 1f, 1f);
        main_texture.shader = null;
    }
}
