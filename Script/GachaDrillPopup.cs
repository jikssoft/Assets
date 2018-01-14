using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaDrillPopup : MonoBehaviour, ReturnKeyProcess {

    public DrillSelector drill_selector;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GachaBallController gacha_controller;
    

    public TweenAlpha twinkle_effect;
    public GameObject particle;
    public GameObject gacha_drill;
    public GameObject confirm_button;
    public GameObject new_icon;
    public GameObject reward_coin;
    public GameObject duplicate_text;

    int random_index;

    public IEnumerator CollectDrillSequence(DrillShopList drill_shop_list)
    {
        ReturnKeyManager.PauseReturnProcess();

        confirm_button.SetActive(false);
        new_icon.SetActive(false);
        reward_coin.SetActive(false);
        limited_drill_texture.gameObject.SetActive(false);
        limeted_drill_text.SetActive(false);
        duplicate_text.SetActive(false);

        gacha_controller.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        yield return gacha_controller.StartAni();
        twinkle_effect.ResetToBeginning();
        twinkle_effect.Play(true);
        yield return new WaitForSeconds(0.05f);
        gacha_controller.gameObject.SetActive(false);

        int randomIndex = UnityEngine.Random.Range(1, drill_shop_list.drill_table.Count);
        while (IsLimitedDrill((int)(drill_shop_list.drill_index_table[randomIndex])) == true)
        {
            randomIndex = UnityEngine.Random.Range(1, drill_shop_list.drill_table.Count);
        }
        random_index = randomIndex;

        gacha_drill = Instantiate(((GameObject)drill_shop_list.drill_table[randomIndex]).gameObject, gameObject.transform);
        gacha_drill.transform.localScale = new Vector3(1f, 1f, 1f);
        gacha_drill.transform.localPosition = new Vector3(0f, 200f, 0f);
        gacha_drill.GetComponent<ShopDrillItem>().SetDefaultColor();
        gacha_drill.GetComponent<UIWidget>().depth = 70;
        iTween.ScaleTo(gacha_drill, iTween.Hash("scale", new Vector3(2f, 2f, 2f), "time", 0.3f, "easetype", iTween.EaseType.easeOutBack));
        //iTween.PunchScale(gacha_drill, new Vector3(1f, 1f, 1f), 0.3f);
        Destroy(gacha_drill.GetComponent<BoxCollider>());

        ChangeLayersRecursively(gacha_drill.transform, "Particle");

        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        GameDataSystem dataSystem = systemObj.GetComponent<GameDataSystem>();

        if (dataSystem.IsCollectDrill((int)(drill_shop_list.drill_index_table[randomIndex])) == false)
        {
            new_icon.SetActive(true);
            reward_coin.SetActive(false);
            duplicate_text.SetActive(false);
            SoundManager.PlayGetNewDrill();

            particle.SetActive(true);
            yield return new WaitForEndOfFrame();
            particle.SetActive(false);
            yield return new WaitForEndOfFrame();
            particle.SetActive(true);
        }
        else
        {
            new_icon.SetActive(false);
            reward_coin.SetActive(true);
            duplicate_text.SetActive(true);
            dataSystem.SetCoin(dataSystem.GetCoin() + 20, true);
        }

        confirm_button.SetActive(true);

        //((GameObject)(drill_shop_list.lock_icon_table[randomIndex])).gameObject.SetActive(false);

        
        Register();
    }

    public UITexture limited_drill_texture;
    public GameObject limeted_drill_text;
    public UILabel limeted_drill_level_text;
    string format = "LEVEL {0}";

    public int limited_drill_clear_level = 0;
    public IEnumerator CollectLimitedDrillSequence(int limited_drill_index, int clear_level)
    {
        ReturnKeyManager.PauseReturnProcess();

        limited_drill_clear_level = clear_level;

        GetComponent<TweenAlpha>().Play(true);

        limited_drill_texture.gameObject.SetActive(true);
        confirm_button.SetActive(true);
        limeted_drill_text.gameObject.SetActive(true);
        new_icon.SetActive(true);
        reward_coin.SetActive(false);
        gacha_controller.gameObject.SetActive(false);
        
        limeted_drill_level_text.text = String.Format(format, limited_drill_clear_level);

        twinkle_effect.ResetToBeginning();
        twinkle_effect.Play(true);
        yield return new WaitForSeconds(0.05f);

        limited_drill_texture.mainTexture = drill_selector.texture_table[limited_drill_index];
        limited_drill_texture.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        iTween.ScaleTo(limited_drill_texture.gameObject, iTween.Hash("scale", new Vector3(1f, 1f, 1f), "time", 0.3f, "easetype", iTween.EaseType.easeOutBack));

        particle.SetActive(true);
        yield return new WaitForEndOfFrame();
        particle.SetActive(false);
        yield return new WaitForEndOfFrame();
        particle.SetActive(true);
        Register();
    }

    public void Register()
    {
        ReturnKeyManager.RegisterReturnKeyProcess(this);
    }

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

    public void ChangeLayersRecursively(Transform trans, string name)
    {
        trans.gameObject.layer = LayerMask.NameToLayer(name);
        foreach (Transform child in trans)
        {
            ChangeLayersRecursively(child, name);
        }
    }

    public DrillShopList drill_shop_list;
    public void Return()
    {
        
        GetComponent<UIPanel>().alpha = 0f;
        if (limited_drill_texture.gameObject.activeSelf == true)
        {
            limited_drill_texture.gameObject.SetActive(false);
        }
        else
        {
            drill_shop_list.ScrollDrill(random_index);
        }
        DestroyObject(gacha_drill);
        particle.SetActive(false);
        //ReturnKeyManager.RegisterReturnKeyProcess(GetParentReturnKeyProcess());
        ReturnKeyManager.RetrunProcess();
    }

    public void CheckRateUs()
    {
        if (limited_drill_clear_level == 10)
        {
            MNRateUsPopup rateUs = new MNRateUsPopup("Rate us", Localization.Get("Rate us"), "Yes", "No", "Later");
            //rateUs.SetAppleId(appleId);
            rateUs.SetAndroidAppUrl("market://details?id=com.twocm.taptapdrill");
            rateUs.AddDeclineListener(() => { Debug.Log("rate us declined"); });
            rateUs.AddRemindListener(() => { Debug.Log("remind me later"); });
            rateUs.AddRateUsListener(() => { Debug.Log("rate us!!!"); });
            rateUs.AddDismissListener(() => { Debug.Log("rate us dialog dismissed :("); });
            rateUs.Show();
            limited_drill_clear_level = 0;
        }
    }

    public ReturnKeyProcess parentReturnKeyProcess;

}
