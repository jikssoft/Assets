using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedDrillManager : MonoBehaviour {

    public GachaDrillPopup gacha_drill_popup;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int CheckLimitedDrillForLevel(int clear_level)
    {
        DrillSelector drill_selector = GetComponent<DrillSelector>();
        GameDataSystem data_system = GameObject.FindGameObjectWithTag("System").GetComponent<GameDataSystem>();
        
        int limited_drill_index = -1;
        for (int i = 0; i < drill_selector.limited_drill_level.Length; i++)
        {
            if (clear_level == drill_selector.limited_drill_level[i])
            {
                if (data_system.IsCollectDrill(drill_selector.limited_drill_index[i]) == false)
                {
                    limited_drill_index = drill_selector.limited_drill_index[i];
                }

                break;
            }
        }


        return limited_drill_index;
    }

    public void GetLimitedDrillForLevel(int clear_level)
    {
        int limited_drill_index = CheckLimitedDrillForLevel(clear_level);

        DrillSelector drill_selector = GetComponent<DrillSelector>();
        GameDataSystem data_system = GameObject.FindGameObjectWithTag("System").GetComponent<GameDataSystem>();
        
        if(limited_drill_index < 0)
        {
            return;
        }

        gacha_drill_popup.parentReturnKeyProcess = GameObject.FindGameObjectWithTag("System").GetComponent<GameMainLogicSystem>();
        gacha_drill_popup.StartCoroutine(gacha_drill_popup.CollectLimitedDrillSequence(limited_drill_index, clear_level));
        data_system.CollectDrill(limited_drill_index);
        getLimeted_drill = true;

        GetComponent<DrillSelector>().GetLimetedDirll(limited_drill_index);
    }

    bool getLimeted_drill = false;
    public bool CheckGetLimitedDrill()
    {
        return getLimeted_drill;
    }

    public void ConfirmLimitedDrill()
    {
        getLimeted_drill = false;
    }
}

