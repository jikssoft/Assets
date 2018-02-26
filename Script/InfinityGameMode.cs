using SA.Analytics.Google;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinityGameMode : MonoBehaviour, GameMainLogicSystem.GameMode
{
    GameDataSystem dataSystem;
    GameMainLogicSystem system;

    public GameObject fast_nail_guide_popup;
    public CheerupGuideController cheerup_guide_controller;

    public GameObject game_ui_obj;

    // Use this for initialization
    void Start()
    {
        GameObject system_obj = GameObject.FindGameObjectWithTag("System");
        system = system_obj.GetComponent<GameMainLogicSystem>();
        dataSystem = system_obj.GetComponent<GameDataSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public TextMesh clear_count_text;
    public StarPointController star_point_controller;

    public int count_clear_nail = 0;

    public bool CheckContinuePopup(int continue_count)
    {
        bool retval = false;

        if (count_clear_nail > 30 && continue_count == 0)// && retry_count >= 5)
        {
            if (((float)(2f * count_clear_nail) / 3f) > (float)count_clear_nail)
            {
                if (UnityEngine.Random.Range(0, 1f) > 0.2f)
                {
                    retval = true;
                }
            }
        }

        return retval;
    }

    public void UpdateGUI()
    {
        game_ui_obj.SetActive(true);

        iTween.FadeTo(clear_count_text.gameObject, 1f, 0.1f);

        count_clear_nail = 0;
        clear_count_text.text = count_clear_nail.ToString();
    }

    public IEnumerator StartGame(float first_time, float second_time, float third_time)
    {
        GameObject selector = GameObject.FindGameObjectWithTag("DrillSelector");
        selector.GetComponent<DrillSelector>().ChangeDrillInfiniteMode();
        system.box.AdjustInfinityModeBoxPositionForMultiResolution();

        yield return new WaitForSeconds(first_time);

        system.nail_table = new ArrayList();

        count_clear_nail = 0;
        system.builder.BuildNailInfinityMode(system.nail_table, count_clear_nail, system.box);
        system.nail_index = 0;
        unit_clear_count_nail = 0;

        clear_count_text.text = count_clear_nail.ToString();

        system.build_nail_table = true;


        yield return new WaitForSeconds(second_time);



        yield return new WaitForSeconds(third_time);

        

    }

 
    public IEnumerator ReStartGame(float first_time, float second_time, float third_time)
    {


        yield return new WaitForSeconds(first_time);

        UpdateGUI();

        Destroy(system.drill.gameObject);
        system.box.DeleteAllBox();
        GameObject selector = GameObject.FindGameObjectWithTag("DrillSelector");
        selector.GetComponent<DrillSelector>().ChangeDrillInfiniteMode();
        system.box.AdjustInfinityModeBoxPositionForMultiResolution();

        star_point_controller.ArrageStar();

        system.nail_table.Clear();
        count_clear_nail = 0;
        system.builder.BuildNailInfinityMode(system.nail_table, count_clear_nail, system.box);
        system.nail_index = 0;
        unit_clear_count_nail = 0;

        clear_count_text.text = count_clear_nail.ToString();

        system.build_nail_table = true;

        yield return new WaitForSeconds(second_time);
        

        yield return new WaitForSeconds(third_time);

        
    }

    public IEnumerator ChangeBox(bool withShutter)
    {
        system.first_nail = true;
        system.drill_time_gause.StopGauseTime();

        system.box.ScrollBox();
        system.builder.ScrollNails(system.nail_table, -system.box.GetDistanceBox(), 0.2f);
        yield return new WaitForSeconds(0.2f);

        int re_position_start_nail_index = system.nail_index - unit_max_count_nail;
        if(re_position_start_nail_index < 0)
        {
            re_position_start_nail_index = system.nail_table.Count - unit_max_count_nail;
        }

        system.box.SetFrontBoxToLast();
        system.builder.SetRePositionNail(system.nail_table,
                re_position_start_nail_index,
                system.box.GetLastBox());
        
        star_point_controller.ArrageStar();

        if(system.nail_index >= system.nail_table.Count)
        {
            system.nail_index = 0;
        }

        system.SetNextNail();

        clear_count_text.text = count_clear_nail.ToString();

        system.build_nail_table = true;

        yield return null;
    }
    
    public void FastGuidePopup()
    {
        
    }


    public IEnumerator CreateGame(float first_time, float second_time, float third_time)
    {
        throw new NotImplementedException();
    }

    public void TapDown()
    {
        if (system.first_nail == true)
        {
            cheerup_guide_controller.Hide();
        }
    }

    public void TapUp()
    {
        count_clear_nail++;
    }

    public void ProcessClearGamePoint()
    {
        //To do save best point
    }

    public void UpdateFailGUI()
    {
        star_point_controller.gameObject.SetActive(true);
        star_point_controller.StartStarEffect(0);
    }

    public void UpdateClearGUI()
    {
        //To show best point
    }

    public void ProcessClearReward()
    {

    }

    public void UpdatePreFailClearGUI(float time)
    {
        iTween.FadeTo(clear_count_text.gameObject, 0f, time);
    }

    public void Continue()
    {
        count_clear_nail--;
        Manager.Client.SendEventHit("GameSequence", "Continue-InfinityMode", count_clear_nail.ToString() + "_" + count_clear_nail.ToString());

        star_point_controller.ArrageStar();
    }

    public int GetAdvancedDiturbTriggerValue()
    {
        return count_clear_nail;
    }

    public void SetLevel(int level)
    {
    }

    public bool CheckInterstitialAD()
    {
        if (count_clear_nail >= 20)
        {
            return true;
        }
        return false;
    }

    int unit_clear_count_nail;
    int unit_max_count_nail = 3;
    public bool CheckNextNail()
    {
        unit_clear_count_nail++;
        clear_count_text.text = count_clear_nail.ToString();

        if (unit_clear_count_nail >= unit_max_count_nail)
        {
            unit_clear_count_nail = 0;
            return true;
        }
        return false;
    }

    public bool CheckClearGame()
    {
        return count_clear_nail == 0;
    }

    public void TimeGaugeOver()
    {
        count_clear_nail++;
    }

    public void CrashBox()
    {
        count_clear_nail++;
    }

    public void NextStage()
    {
        
    }
}
