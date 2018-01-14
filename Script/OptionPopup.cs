using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPopup : MonoBehaviour, ReturnKeyProcess {

    public UIButton bgm_on;
    public UIButton bgm_off;

    public UIButton sound_on;
    public UIButton sound_off;

    public UIButton vibration_on;
    public UIButton vibration_off;

    GameDataSystem data_system;
    // Use this for initialization
    void Start () {
        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        data_system = systemObj.GetComponent<GameDataSystem>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public UILabel language;
    public UISprite flag;
    public LanguagePopup language_popup;

    public void ShowOptionMenu()
    {
        gameObject.SetActive(true);
        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        data_system = systemObj.GetComponent<GameDataSystem>();

        if (data_system.GetBGM() == 1)
        {
            bgm_on.gameObject.SetActive(true);
            bgm_off.gameObject.SetActive(false);
        }
        else
        {
            bgm_on.gameObject.SetActive(false);
            bgm_off.gameObject.SetActive(true);
        }

        if(data_system.GetSound() == 1)
        {
            sound_on.gameObject.SetActive(true);
            sound_off.gameObject.SetActive(false);
        }
        else
        {
            sound_on.gameObject.SetActive(false);
            sound_off.gameObject.SetActive(true);
        }

        if(data_system.GetVibration() == 1)
        {
            vibration_on.gameObject.SetActive(true);
            vibration_off.gameObject.SetActive(false);
        }
        else
        {
            vibration_on.gameObject.SetActive(false);
            vibration_off.gameObject.SetActive(true);
        }

        int language_index = data_system.GetLanguage();

        language.text = Localization.knownLanguages[language_index];
        for(int i = 0; i < language_popup.index_table.Length; i++)
        {
            if(language_index == language_popup.index_table[i])
            {
                flag.spriteName = language_popup.languages[i].transform.Find("Sprite").GetComponent<UISprite>().spriteName;
                break;
            }
        }

        //BannerController.HideBanner();

        Register();
    }

    public void BGMOn()
    {
        bgm_on.gameObject.SetActive(true);
        bgm_off.gameObject.SetActive(false);
        data_system.SetBGM(1);
    }

    public void BGMOff()
    {
        bgm_on.gameObject.SetActive(false);
        bgm_off.gameObject.SetActive(true);
        data_system.SetBGM(0);
    }

    public void SoundOn()
    {
        sound_on.gameObject.SetActive(true);
        sound_off.gameObject.SetActive(false);
        data_system.SetSound(1);
    }

    public void SoundOff()
    {
        sound_on.gameObject.SetActive(false);
        sound_off.gameObject.SetActive(true);
        data_system.SetSound(0);
    }

    public void VibrationOn()
    {
        vibration_on.gameObject.SetActive(true);
        vibration_off.gameObject.SetActive(false);
        data_system.SetVibration(1);
    }

    public void VibrationOff()
    {
        vibration_on.gameObject.SetActive(false);
        vibration_off.gameObject.SetActive(true);
        data_system.SetVibration(0);
    }

    public void ReturnOptionMenu()
    {

        StartCoroutine(CloseOptionMenu());
        
    }

    IEnumerator CloseOptionMenu()
    {
        ActiveAnimation.Play(gameObject.GetComponent<Animation>(), "HideOptionMenu", AnimationOrTween.Direction.Forward);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        BannerController.ShowBanner();
    }

    public void Register()
    {
        ReturnKeyManager.RegisterReturnKeyProcess(this);
    }

    public void Return()
    {
        ReturnOptionMenu();
        //ReturnKeyManager.RegisterReturnKeyProcess(GetParentReturnKeyProcess());
        ReturnKeyManager.RetrunProcess();
    }


    public GameMenuUI menu_ui;


    public void ResetDrill()
    {
        data_system.ResetDrill();
    }

    public void AllDrill()
    {
        data_system.AllDrill();
    }

	public PurchaseDrillManager purchase_drill_manager;
	public void RestoreDrill()
	{
		purchase_drill_manager.RestoreDrill (true);
	}
}

