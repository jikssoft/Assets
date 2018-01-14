using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguagePopup : MonoBehaviour, ReturnKeyProcess {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public UISprite[] languages;
    public int[] index_table;
    UISprite last_pick;
    GameDataSystem data_system;

    public void ShowLanguagePopup()
    {
        gameObject.SetActive(true);
        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        data_system = systemObj.GetComponent<GameDataSystem>();

        int index = data_system.GetLanguage();

        for (int i = 0; i < index_table.Length; i++)
        {
            if (index == index_table[i])
            {
                languages[i].spriteName = "bg-language-select";
                languages[i].GetComponent<UIButton>().normalSprite = "bg-language-select";
                last_pick = languages[i];
                break;
            }
        }

        ActiveAnimation.Play(gameObject.GetComponent<Animation>(), "ShowLanguageMenu", AnimationOrTween.Direction.Forward);

        Register();
    }

    public UILabel option_label;
    public UISprite option_flag;

    public void SetLanguage(UISprite sprite)
    {
        last_pick.spriteName = "bg-language";
        last_pick.GetComponent<UIButton>().normalSprite = "bg-language";

        for (int i = 0; i < languages.Length; i++)
        {
            if(sprite == languages[i])
            {
                languages[i].spriteName = "bg-language-select";
                languages[i].GetComponent<UIButton>().normalSprite = "bg-language-select";
                data_system.SetLanguage(index_table[i]);
                
                LocalizeTextMesh[] test_mech_table = GameObject.FindObjectsOfType<LocalizeTextMesh>();
                foreach(LocalizeTextMesh test_mech in test_mech_table)
                {
                    test_mech.OnLocalize();
                }

                option_label.text = Localization.knownLanguages[index_table[i]];
                option_flag.spriteName = languages[i].transform.Find("Sprite").GetComponent<UISprite>().spriteName;

                break;
            }
        }

        last_pick = sprite;
    }

    public void ReturnLanguageMenu()
    {
        StartCoroutine(CloseLanguageMenu());

    }

    IEnumerator CloseLanguageMenu()
    {
        ActiveAnimation.Play(gameObject.GetComponent<Animation>(), "HideLanguageMenu", AnimationOrTween.Direction.Forward);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    public void Register()
    {
        ReturnKeyManager.RegisterReturnKeyProcess(this);
    }

    public void Return()
    {
        ReturnLanguageMenu();
        //ReturnKeyManager.RegisterReturnKeyProcess(GetParentReturnKeyProcess());
        ReturnKeyManager.RetrunProcess();
    }


    public OptionPopup option_popup;

}
