using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillShopList : MonoBehaviour, ReturnKeyProcess, Rewardable {

    public DrillSelector drill_selector;
    public GameObject grid_view;

	// Use this for initialization
	void Start () {
        drill_table = new ArrayList();
        lock_icon_table = new ArrayList();
        drill_index_table = new ArrayList();

        scroll_panel.onDragStarted = onDragStarted;
        scroll_panel.onStoppedMoving = onDragFinished;
    }

    public UIScrollView scroll_panel;

    float unit_drill_width = 240f;
	// Update is called once per frame
	void Update () {

       if(dragging == true)
        {
            int x = (int)scroll_panel.GetComponent<UIPanel>().clipOffset.x - (int)(unit_drill_width/2);

            int shop_index = -1;

            if (x > 0)
            {
                shop_index = (int)((x - 1) / unit_drill_width);
            }
                        
            if(shop_index >= 0 && shop_index < drill_table.Count && shop_index != last_selected_shop_index)
            {
                GameObject selected_drill = ((GameObject)(drill_table[shop_index]));
                SelectDrill(shop_index, selected_drill);
            }
            if(shop_index < 0)
            {
                SelectRandom();
            }
        }
       
        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        GameDataSystem dataSystem = systemObj.GetComponent<GameDataSystem>();
        if (dataSystem.GetCoin() >= cost_gacha)
        {
            if (Time.fixedTime - (int)(Time.fixedTime) < 0.5f)
            {
                collect_drill_button.spriteName = "btn_tool_shop_new_y";
            }
            else
            {
                collect_drill_button.spriteName = "btn_tool_shop_new";
            }

            collect_drill_button.GetComponent<UIPlayTween>().enabled = true;
        }


        if (free_coin_button.activeSelf == true)
        {
            if (RewardADController.CheckRewardAD() == true)
            {
                free_coin_button.GetComponentInChildren<UIButton>().enabled = true;
                free_coin_button.GetComponentInChildren<UISprite>().color = new Color(1f, 1f, 1f);
                waiting_time.gameObject.SetActive(false);
                waiting_time.text = null;
            }
            else
            {
                free_coin_button.GetComponentInChildren<UIButton>().enabled = false;
                free_coin_button.GetComponentInChildren<UISprite>().color = new Color(0.3f, 0.3f, 0.3f);
                waiting_time.gameObject.SetActive(true);

                TimeSpan delta_time = RewardADController.GetWatingTime();
                waiting_time.text = string.Format("{0}:{1}", delta_time.Minutes.ToString("D2"), delta_time.Seconds.ToString("D2"));
            }
        }
    }

    bool dragging = false;
    void onDragStarted()
    {
        dragging = true;
    }

    void onDragFinished()
    {
        dragging = false;
    }

    public void ShowShopMenu()
    {
        gameObject.SetActive(true);
        StartCoroutine(ShowShopMenuSequence());
        //BannerController.HideBanner();
    }

    public ShutterController shop_shutter;
    public GameObject shop_bottom_ui;
    public Animation game_ui;

    public UILabel coin;
    public LimitedDrillManager manager;
    IEnumerator ShowShopMenuSequence()
    {
        ReturnKeyManager.PauseReturnProcess();

        shop_shutter.ShutDown();
        yield return new WaitForSeconds(0.5f);

        //ActiveAnimation.Play(game_ui, "HideInGameMenu", AnimationOrTween.Direction.Forward);
        
        gameObject.GetComponent<UIPanel>().alpha = 1f;
        shop_bottom_ui.SetActive(true);
        DrillUpdate();

        manager.ConfirmLimitedDrill();

        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        GameDataSystem dataSystem = systemObj.GetComponent<GameDataSystem>();
        coin.text = dataSystem.GetCoin().ToString();

        drill_count_label.text = drill_count.ToString();
        total_drill_count_label.text = string.Format("/{0}", total_drill_count);

        yield return new WaitForSeconds(0.2f);
        shop_shutter.ShutUp();
        
        yield return new WaitForSeconds(0.2f);
        ScrollDrill(last_selected_shop_index, false);

        yield return new WaitForSeconds(0.3f);
        Register();
    }


    public GameObject level_label;
    public GameObject lock_icon;
    public UISprite collect_drill_button;
    public GameObject uncle;

    public ArrayList drill_table;
    public ArrayList lock_icon_table;
    public ArrayList drill_index_table;

    int cost_gacha = 100;

    void UpdatePurchaseDrillUI()
    {
        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        GameDataSystem dataSystem = systemObj.GetComponent<GameDataSystem>();

        if (dataSystem.GetCoin() >= cost_gacha)
        {
            collect_drill_button.spriteName = "btn_tool_shop_new";
            collect_drill_button.GetComponent<UIPlayTween>().enabled = true;
            //uncle.SetActive(true);
        }
        else
        {
            collect_drill_button.spriteName = "btn_tool_shop_new_none";
            collect_drill_button.GetComponent<UIPlayTween>().enabled = false;
            //uncle.SetActive(false);
        }
    }

    public GameObject drill_shop_list_item;

    public UILabel drill_count_label;
    public UILabel total_drill_count_label;

    public int drill_count;
    public int total_drill_count;

    public GameObject new_label_icon;

    void DrillUpdate()
    {
        drill_count = 0;
        total_drill_count = 0;

        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        GameDataSystem dataSystem = systemObj.GetComponent<GameDataSystem>();
        bool limited_drill;

        UpdatePurchaseDrillUI();

        GameObject selectedDrill = null;
        
        for (int i = 0; i < drill_selector.drill_table.Length; i++)
        {
            limited_drill = false;
            foreach (int limited_index in drill_selector.limited_drill_index)
            {
                if (limited_index == i)
                {
                    limited_drill = true;
                    break;
                }
            }

            if (limited_drill == true)
            {
                continue;
            }

            drill_index_table.Add(i);
            GameObject drill = Instantiate(drill_shop_list_item, grid_view.transform);
            ChangeLayersRecursively(drill.transform, "Popup");
            drill.transform.position = new Vector3(0f, 0f, 0f);
            drill.transform.localScale = new Vector3(1f, 1f, 1f);

            

            //drill.transform.GetChild(0).localScale = new Vector3(50f, 50f, 50f);
            //drill.transform.GetChild(0).position = new Vector3(0f, -0.1f, 0f);
            //drill.transform.GetChild(1).gameObject.SetActive(false);

            //drill.AddComponent<BoxCollider>().size = new Vector3(240f, 276f, 1f);

            drill.GetComponent<UITexture>().mainTexture = drill_selector.texture_table[i];

            drill.AddComponent<UIDragScrollView>();
            drill.AddComponent<UICenterOnClick>();

            UIEventTrigger eventTrigger = drill.AddComponent<UIEventTrigger>();
            EventDelegate eventDelegate = new EventDelegate(this, "SelectDrill");
            eventDelegate.parameters[0] = new EventDelegate.Parameter(drill_table.Count);
            eventDelegate.parameters[1] = new EventDelegate.Parameter(drill);
            eventTrigger.onClick.Add(eventDelegate);

            //GameObject lockObj = Instantiate(lock_icon, drill.transform);
            //lockObj.transform.localScale = new Vector3(1f, 1f, 1f);

            //lock_icon_table.Add(lockObj);

            if (dataSystem.IsCollectDrill(i) == false)
            {
                drill.GetComponent<ShopDrillItem>().SetGrayScale();
            }
            else
            {
                drill_count++;
                if(dataSystem.GetDrillUsed(i) == false)
                {
                    GameObject new_icon = Instantiate(new_label_icon, drill.transform, false);
                    new_icon.transform.parent = drill.transform;
                }
            }
            
            if(i == dataSystem.GetLastDrill())
            {
                selectedDrill = drill;
                last_selected_sellector_index = dataSystem.GetLastDrill();
                last_selected_shop_index = drill_table.Count;
            }
            drill_table.Add(drill);
        }

        int level_index = 0;
        foreach(int sellector_index in drill_selector.limited_drill_index)
        {
            drill_index_table.Add(sellector_index);
            GameObject drill = Instantiate(drill_shop_list_item, grid_view.transform);
            ChangeLayersRecursively(drill.transform, "Popup");
            drill.transform.position = new Vector3(0f, 0f, 0f);
            drill.transform.localScale = new Vector3(1f, 1f, 1f);

            //drill.transform.GetChild(0).localScale = new Vector3(50f, 50f, 50f);
            //drill.transform.GetChild(0).position = new Vector3(0f, -0.1f, 0f);
            //drill.transform.GetChild(1).gameObject.SetActive(false);

            //drill.AddComponent<BoxCollider>().size = new Vector3(240f, 276f, 1f);
            drill.GetComponent<UITexture>().mainTexture = drill_selector.texture_table[sellector_index];

            drill.AddComponent<UIDragScrollView>();
            drill.AddComponent<UICenterOnClick>();

            UIEventTrigger eventTrigger = drill.AddComponent<UIEventTrigger>();
            EventDelegate eventDelegate = new EventDelegate(this, "SelectDrill");
            eventDelegate.parameters[0] = new EventDelegate.Parameter(drill_table.Count);
            eventDelegate.parameters[1] = new EventDelegate.Parameter(drill);
            eventTrigger.onClick.Add(eventDelegate);

            //GameObject lockObj = Instantiate(lock_icon, drill.transform);
            //lockObj.transform.localScale = new Vector3(1f, 1f, 1f);

            GameObject levelObj = Instantiate(level_label, drill.transform);
            levelObj.transform.localScale = new Vector3(1f, 1f, 1f);
            levelObj.transform.localPosition = new Vector3(0f, 0f, 0f);
            levelObj.GetComponentInChildren<UILabel>().text = string.Format("LV.{0}", drill_selector.limited_drill_level[level_index]);
            levelObj.SetActive(false);
            level_index++;

            if (dataSystem.IsCollectDrill(sellector_index) == false)
            {
                drill.GetComponent<ShopDrillItem>().SetBlack();   
            }
            else
            {
                drill_count++;
                if (dataSystem.GetDrillUsed(sellector_index) == false)
                {
                    GameObject new_icon = Instantiate(new_label_icon, drill.transform, false);
                    new_icon.transform.parent = drill.transform;
                }
            }

            if (sellector_index == dataSystem.GetLastDrill())
            {
                selectedDrill = drill;
                last_selected_sellector_index = dataSystem.GetLastDrill();
                last_selected_shop_index = drill_table.Count;
            }
            drill_table.Add(drill);
        }

        grid_view.GetComponent<UIGrid>().enabled = true;

        total_drill_count = drill_table.Count;

        SelectDrill(last_selected_shop_index, selectedDrill);
        //ScrollDrill(dataSystem.GetLastDrill());
    }

    bool isLimitedDrill(int selector_index)
    {
        foreach (int i in drill_selector.limited_drill_index)
        {
            if (i == selector_index)
            {
                return true;
            }
        }
        return false;
    }


    GameObject selected_drill = null;

    public GameObject random_image;
    public void SelectRandom()
    {
        free_coin_button.SetActive(false);
        random_image.SetActive(true);
        if (selected_drill != null)
        {
            Destroy(selected_drill);
        }

        last_selected_sellector_index = -1;
        last_selected_shop_index = -1;
        check_button.enabled = true;
        check_button.gameObject.GetComponent<UISprite>().spriteName = "button_red";
		check_button.gameObject.GetComponent<UIButton>().normalSprite = "button_red";
    }

    public void SelectDrill(int shop_index, GameObject selectedDrill)
    {
        StartCoroutine(ShowSelectedDrill(shop_index, selectedDrill));
    }

    public UIButton check_button;
    int last_selected_shop_index = 0;
    int last_selected_sellector_index = 0;
    public UISprite icon_limited_drill;

	bool purchased_drill;

    IEnumerator ShowSelectedDrill(int shop_index, GameObject selectedDrill)
    {
        yield return new WaitForEndOfFrame();

        free_coin_button.SetActive(false);
        random_image.SetActive(false);

        if (selected_drill != null)
        {
            Destroy(selected_drill);
        }

        if(isLimitedDrill(last_selected_sellector_index) == true)
        {
            ((GameObject)(drill_table[last_selected_shop_index])).transform.GetChild(0).gameObject.SetActive(false);
        }

        yield return new WaitForEndOfFrame();

        if (shop_index >= 0)
        {
            selected_drill = Instantiate(selectedDrill, gameObject.transform);
            Destroy(selected_drill.GetComponent<BoxCollider>());
            selected_drill.GetComponent<UITexture>().width = 350;
            selected_drill.GetComponent<UITexture>().height = 400;
            selected_drill.transform.localPosition = new Vector3(0f, 400f, 0f);

            for(int i = 0; i < selected_drill.transform.childCount; i++)
            {
                selected_drill.transform.GetChild(i).gameObject.SetActive(false);
            }

            
            int collector_index = (int)(drill_index_table[shop_index]);
            /*
            if (isLimitedDrill(collector_index) == true)
            {
                selected_drill.transform.GetChild(0).gameObject.SetActive(false);
            }
            */

            GameObject systemObj = GameObject.FindGameObjectWithTag("System");
            GameDataSystem dataSystem = systemObj.GetComponent<GameDataSystem>();

            icon_limited_drill.gameObject.SetActive(false);

            if (dataSystem.IsCollectDrill(collector_index) == true)
            {
				purchased_drill = false;
                check_button.enabled = true;
                check_button.gameObject.GetComponent<UISprite>().spriteName = "button_red";
				check_button.gameObject.GetComponent<UIButton>().normalSprite = "button_red";


				check_button.transform.GetChild(0).gameObject.SetActive (true);
				check_button.transform.GetChild(1).gameObject.SetActive (false);
            }
            else
            {
				

				if (isLimitedDrill (collector_index) == true) {
					icon_limited_drill.gameObject.SetActive (true);
					selectedDrill.transform.GetChild (0).gameObject.SetActive (true);
					check_button.enabled = false;
					check_button.gameObject.GetComponent<UISprite> ().spriteName = "bg_black";
					check_button.gameObject.GetComponent<UIButton>().normalSprite = "bg_black";
				} else {

					string local_price = purchase_drill_manager.GetLocalizedPrice ();
					if (local_price != null) 
					{
						purchased_drill = true;
						check_button.gameObject.GetComponent<UISprite>().spriteName = "button_red";
						check_button.gameObject.GetComponent<UIButton>().normalSprite = "button_red";

						check_button.transform.GetChild (0).gameObject.SetActive (false);
						check_button.transform.GetChild (1).gameObject.GetComponent<UILabel> ().text = local_price;
						check_button.transform.GetChild (1).gameObject.SetActive (true);

						check_button.enabled = true;
					} else 
					{
						check_button.transform.GetChild (0).gameObject.SetActive (true);
						check_button.transform.GetChild (1).gameObject.SetActive (false);

						check_button.gameObject.GetComponent<UISprite> ().spriteName = "bg_black";
						check_button.gameObject.GetComponent<UIButton>().normalSprite = "bg_black";

						check_button.enabled = false;
					}

				}

            }

            last_selected_sellector_index = collector_index;
            last_selected_shop_index = shop_index;
        }
        else
        {
            SelectRandom();
        }
      
        
    }

    public GameObject free_coin_button;
    public GachaDrillPopup gacha_drill_popup;
    public UILabel waiting_time;
    public void CollectDrill()
    {
        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        GameDataSystem dataSystem = systemObj.GetComponent<GameDataSystem>();

        if (dataSystem.GetCoin() >= 100)
        {
            dataSystem.SetCoin(dataSystem.GetCoin() - 100, true);
            coin.text = dataSystem.GetCoin().ToString();
            gacha_drill_popup.StartCoroutine(gacha_drill_popup.CollectDrillSequence(this));
            gacha_drill_popup.parentReturnKeyProcess = this;
            SoundManager.PlayBuyDrill();
        }
        else
        {
            free_coin_button.SetActive(true);
            if (RewardADController.CheckRewardAD() == true)
            {
                free_coin_button.GetComponentInChildren<UIButton>().enabled = true;
                free_coin_button.GetComponentInChildren<UISprite>().color = new Color(1f, 1f, 1f);
                waiting_time.gameObject.SetActive(false);
                waiting_time.text = null;
            }
            else
            {
                free_coin_button.GetComponentInChildren<UIButton>().enabled = false;
                free_coin_button.GetComponentInChildren<UISprite>().color = new Color(0.3f, 0.3f, 0.3f);
                waiting_time.gameObject.SetActive(true);
            }
        }
    }

    public void ShowRewardAD()
    {
        RewardADController.ShowRewardAd(this, false);
    }

    public GameObject random_object_drill;
    public void ScrollDrill(int shop_index, bool collected = true)
    {
        UpdatePurchaseDrillUI();
        //((GameObject)(drill_table[index])).GetComponent<UICenterOnClick>()
        GameObject selected_drill = null;
        if (shop_index < 0)
        {
            selected_drill = random_object_drill;
        }
        else
        {
            selected_drill = ((GameObject)(drill_table[shop_index]));
        }
        UICenterOnChild center = NGUITools.FindInParents<UICenterOnChild>(selected_drill);
        UIPanel panel = NGUITools.FindInParents<UIPanel>(selected_drill);

        if (selected_drill.GetComponent<ShopDrillItem>() != null)
        {
            selected_drill.GetComponent<ShopDrillItem>().SetDefaultColor();
        }
        
        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        GameDataSystem dataSystem = systemObj.GetComponent<GameDataSystem>();

        // random select
        if (shop_index > 0 && collected == true && dataSystem.IsCollectDrill((int)drill_index_table[shop_index]) == false)
        {
            dataSystem.CollectDrill((int)(drill_index_table[shop_index]));

            GameObject new_icon = Instantiate(new_label_icon, selected_drill.transform, false);
            new_icon.transform.parent = selected_drill.transform;
        }
        
        coin.text = dataSystem.GetCoin().ToString();

        if (center != null)
        {
            if (center.enabled)
                center.CenterOn(selected_drill.transform);
        }
        else if (panel != null && panel.clipping != UIDrawCall.Clipping.None)
        {
            UIScrollView sv = panel.GetComponent<UIScrollView>();
            Vector3 offset = -panel.cachedTransform.InverseTransformPoint(selected_drill.transform.position);
            if (!sv.canMoveHorizontally) offset.x = panel.cachedTransform.localPosition.x;
            if (!sv.canMoveVertically) offset.y = panel.cachedTransform.localPosition.y;
            SpringPanel.Begin(panel.cachedGameObject, offset, 6f);
        }

        SelectDrill(shop_index, selected_drill);
    }
    

    /*
    public TweenAlpha twinkle_effect;
    public GameObject collect_drill_popup;
    public GameObject particle;
    IEnumerator CollectDrillSequence()
    {
        Gacha_controller.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        yield return Gacha_controller.StartAni();
        twinkle_effect.ResetToBeginning();
        twinkle_effect.Play(true);
        yield return new WaitForSeconds(0.05f);
        Gacha_controller.gameObject.SetActive(false);
        
        int randomIndex = Random.Range(1, drill_selector.drill_table.Length);
        while(IsLimitedDrill(randomIndex) == true)
        {
            randomIndex = Random.Range(1, drill_selector.drill_table.Length);
        }

        GameObject drill = Instantiate(((GameObject)drill_table[randomIndex]).gameObject, collect_drill_popup.transform);
        drill.transform.localScale = new Vector3(2f, 2f, 2f);
        drill.transform.localPosition = new Vector3(0f, 400f, 0f);
        drill.GetComponent<Drill>().SetDefaultColor();

        ChangeLayersRecursively(drill.transform, "Particle");

        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        GameDataSystem dataSystem = systemObj.GetComponent<GameDataSystem>();
        if (dataSystem.IsCollectDrill(randomIndex) == true)
        {
            particle.SetActive(true);
        }

        ((GameObject)(lock_icon_table[randomIndex])).gameObject.SetActive(false);
    }
    */
    public bool IsLimitedDrill(int index)
    {
        bool limited_drill = false;
        foreach (int limited_index in drill_selector.limited_drill_index)
        {
            if (limited_index == index)
            {
                limited_drill = true;
                break;
            }
        }

        return limited_drill;
    }

    public void UpdateCoin()
    {
        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        GameDataSystem data_system = systemObj.GetComponent<GameDataSystem>();
        int current_coin = data_system.GetCoin();
        
        coin.text = current_coin.ToString();
    }

    public void ChangeLayersRecursively(Transform trans, string name)
    {
        trans.gameObject.layer = LayerMask.NameToLayer(name);
        foreach (Transform child in trans)
        {
            ChangeLayersRecursively(child, name);
        }
    }

    public void Cancel()
    {
        StartCoroutine(Return(false));
    }

	public PurchaseDrillManager purchase_drill_manager;

    public void Check()
    {
		if (purchased_drill == false) {
			StartCoroutine (Return (true));
		} else {
			
			purchase_drill_manager.PurchaseDrill (last_selected_sellector_index);
		}

    }

	public void CompletePurchaseDrill()
	{
		check_button.transform.GetChild(0).gameObject.SetActive (true);
		check_button.transform.GetChild(1).gameObject.SetActive (false);

		GameObject systemObj = GameObject.FindGameObjectWithTag ("System");
		GameDataSystem data_system = systemObj.GetComponent<GameDataSystem> ();


		data_system.PurchaseDrill(last_selected_sellector_index);

		ScrollDrill (last_selected_shop_index);
		//SelectDrill(shop_index, selected_drill);
	}

    public GameObject shop_drill_menu;
    public IEnumerator Return(bool checked_drill)
    {
        //ReturnKeyManager.PauseReturnProcess();
        ReturnKeyManager.RetrunProcess();

        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        GameMainLogicSystem system = systemObj.GetComponent<GameMainLogicSystem>();
        GameDataSystem data_system = systemObj.GetComponent<GameDataSystem>();

        system.UpdateCoin();

        if (checked_drill)
        {
            data_system.SelectDrill(last_selected_sellector_index);
            system.ChangeDrill();
        }
        else
        {
            BannerController.ShowBanner();
            shop_shutter.ShutDown();
        }
        
        yield return new WaitForSeconds(0.5f);

        shop_drill_menu.SetActive(false);
        GetComponent<UIPanel>().alpha = 0f;

        foreach (GameObject drill in drill_table)
        {
            Destroy(drill);
        }

        Destroy(selected_drill);

        if(checked_drill)
        {
            //BannerController.HideBanner();
        }
        else
        {
            shop_shutter.ShutUp();
        }

        drill_table.Clear();
        lock_icon_table.Clear();
        drill_index_table.Clear();

        yield return new WaitForSeconds(0f);

        //ReturnKeyManager.RegisterReturnKeyProcess(GetParentReturnKeyProcess());
        //ReturnKeyManager.RetrunProcess();

        gameObject.SetActive(false);
    }

    public void Register()
    {
        ReturnKeyManager.RegisterReturnKeyProcess(this);
    }

    public void Return()
    {
        Cancel();
    }
    
    public GameMainLogicSystem system;


    public RewardCoinController coin_controller;
    public void Reward()
    {
        free_coin_button.SetActive(false);

        GameObject target = GameObject.FindGameObjectWithTag("CoinIcon");
        //GameObject target = GameObject.FindGameObjectWithTag("ShopCoinIcon");
        coin_controller.CreateRewardCoin(target.transform);
    }
}

