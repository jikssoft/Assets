using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPointController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public UISprite[] start_table;
    public UIPlayTween[] text_table;
    public UIPlayTween fail_text_image;

    public void StartStarEffect(int point)
    {
        StartCoroutine(StarEffect(point));
    }

    IEnumerator StarEffect(int point)
    {
        GetComponent<Animation>().Play();

        yield return new WaitForSeconds(0.2f);

        if(point > 3)
        {
            Debug.Log("============ wrong point " + point);
            point = 3;
        }

        for (int i = 0; i < point; i++)
        {
            UISprite sprite = start_table[i];

            sprite.transform.localScale = new Vector3(3f, 3f, 3f);
            iTween.ScaleTo(sprite.gameObject, iTween.Hash("scale", new Vector3(1f, 1f),
                "time", 0.2f,
                "easetype", iTween.EaseType.easeOutElastic));

            yield return new WaitForSeconds(0.1f);
        }

        if (point > 0)
        {
            text_table[point - 1].Play(true);
            last_index = point - 1;
            SoundManager.PlaySuccess();
        }
        else if(point == 0)
        {
            fail_text_image.Play(true);
            SoundManager.PlayFail();
        }

        BannerController.ShowBanner();
    }

    int last_index;
    public void ArrageStar()
    {
        for (int i = 0; i < 3; i++)
        {
            start_table[i].transform.localScale = new Vector3(0f,0f,0f);
        }

        foreach(UITweener tweener in text_table[last_index].GetComponentsInChildren<UITweener>())
        {
            tweener.GetComponent<UITweener>().enabled = false;
            tweener.GetComponent<UITweener>().ResetToBeginning();
        }

        foreach (UITweener tweener in fail_text_image.GetComponentsInChildren<UITweener>())
        {
            tweener.GetComponent<UITweener>().enabled = false;
            tweener.GetComponent<UITweener>().ResetToBeginning();
        }
        
        GetComponent<Animation>().Play("HideStartPoint");

        gameObject.SetActive(false);
    }
}
