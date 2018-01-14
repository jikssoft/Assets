using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuUI : MonoBehaviour, ReturnKeyProcess
{

    public UISprite shop_button;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (CheckGachaDrillOrNewDrill() == true)
        {
            if (Time.fixedTime - (int)(Time.fixedTime) < 0.5f)
            {
                shop_button.spriteName = "btn-shop";
            }
            else
            {
                shop_button.spriteName = "btn-shop-new";
            }
        }
        else
        {
            shop_button.spriteName = "btn-shop";
        }
    }

    public LimitedDrillManager manager;
    bool CheckGachaDrillOrNewDrill()
    {
        GameDataSystem dataSystem = GameObject.FindGameObjectWithTag("System").GetComponent<GameDataSystem>();
        
        if (dataSystem.GetCoin() >= 100 || manager.CheckGetLimitedDrill())
        {
            return true;
        }

        return false;
    }

    public void Register()
    {
        ReturnKeyManager.RegisterReturnKeyProcess(this);
    }

    public void Return()
    {
        system.ResumeGame();
        //ReturnKeyManager.RegisterReturnKeyProcess(GetParentReturnKeyProcess());
        ReturnKeyManager.RetrunProcess();
    }
    

    public GameMainLogicSystem system;

    public void ShowBanner()
    {
        BannerController.ShowBanner();
    }

    public void HideBanner()
    {
        //BannerController.HideBanner();
    }
}
