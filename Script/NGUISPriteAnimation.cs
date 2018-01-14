using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NGUISPriteAnimation : MonoBehaviour {

	public string[] sprite_table;

	public float time;

	float running_time;

	public float scale = 1f;

	public bool loop = false;

	public bool start_active = false;

	// Use this for initialization
	void Start () {
		if (start_active == false)
			running_time = -1f;
		else
			running_time = 0f;
	}

	// Update is called once per frame
	void Update () {
		if(running_time < 0)
		{
			return;
		}

		running_time += Time.fixedDeltaTime * scale;

		if(running_time > time)
		{
			if(loop == false)
				return;

			running_time = 0f;
		}

		float t = running_time / time;

		int index = (int)(t * (sprite_table.Length-1));

		UISprite renderer = gameObject.GetComponent<UISprite>();
		renderer.spriteName = sprite_table[index];
	}

	public void ResetAni()
	{
		running_time = 0f;
	}

	public void StartAnimation()
	{
		running_time = 0f;
		scale = 1f;
	}

	public void SetScale(float scale)
	{
		this.scale = scale;
	}
}