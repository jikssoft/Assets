using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPointListItem : MonoBehaviour {

    int point;
    int level;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetPoint(int levelValue, int pointValue)
    {
        point = pointValue;
        level = levelValue;
        UpdateUI();
    }

    public UISprite[] stars;
    public UISprite three_star_bg;
    public UIButton button;
    public UILabel level_text;

    public void UpdateUI()
    {
        
        GameDataSystem data_system = GameObject.FindGameObjectWithTag("System").GetComponent<GameDataSystem>();
        int last_level = data_system.GetLevel();

        Debug.Log("-------------" + level + "===================" + last_level);

        if (point == 0 && level == last_level)
        {
            gameObject.SetActive(true);
            button.normalSprite = "bg-level-lock";
            level_text.text = string.Format("LV {0}", level);
            level_text.color = new Color(0.509f, 0.552f, 0.592f);
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].gameObject.SetActive(false);
            }
        }
        else if (point == 0 && level > last_level)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            button.normalSprite = "bg-level";
            three_star_bg.enabled = false;

            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].gameObject.SetActive(true);
                stars[i].spriteName = "star_blank";
            }

            for (int i = 0; i < point; i++)
            {
                if (i >= stars.Length)
                    continue;
                stars[i].spriteName = "star";
            }

            if (point >= 3)
            {
                three_star_bg.enabled = true;
            }

            level_text.text = string.Format("LV {0}", level);
            level_text.color = new Color(0.21f, 0.21f, 0.21f);
        }

        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        EventDelegate eventDelegate = new EventDelegate(systemObj.GetComponent<GameMainLogicSystem>(), "SetLevel");
        eventDelegate.parameters[0] = new EventDelegate.Parameter(level);

        button.onClick.Add(eventDelegate);
    }
}
