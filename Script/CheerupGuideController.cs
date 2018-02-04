using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheerupGuideController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public UILabel guide_text;
    public UITexture texture;
    public DrillSelector drill_selector;

    public UILabel target_level_text;

    int limited_drill_index; 

    public void Show(int current_level)
    {
        
        int next_target_level = GetNextLimitedDrill(current_level);
        if(next_target_level < 0)
        {
            return;
        }

        /*
        texture.mainTexture = drill_selector.texture_table[limited_drill_index];
        guide_text.text = Localization.Get("Cheer up");

        guide_text.text = guide_text.text.Replace("LEVEL %%%", "[373449]LEVEL " + next_target_level.ToString() + "[-]");
        */

        target_level_text.text = next_target_level.ToString();


        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    int GetNextLimitedDrill(int current_level)
    {
        GameDataSystem data_system = GameObject.FindGameObjectWithTag("System").GetComponent<GameDataSystem>();

        for (int i = 0; i < drill_selector.limited_drill_level.Length; i++)
        {
            if (current_level <= drill_selector.limited_drill_level[i] &&
                drill_selector.limited_drill_level[i] - current_level <= 2)
            {
                if (data_system.IsCollectDrill(drill_selector.limited_drill_index[i]) == false)
                {
                    limited_drill_index = drill_selector.limited_drill_index[i];
                    return drill_selector.limited_drill_level[i];
                }
            }
        }

        return -1;
    }
    
}
