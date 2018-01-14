using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioClip success_level;
    public AudioClip fail_level;
    public AudioClip fail_nail_and_timeover;
    public AudioClip fail_over_amazing;
    public AudioClip get_coin;
    public AudioClip get_new_wating;
    public AudioClip get_new_drill;
    public AudioClip buy_drill;

    public AudioClip good_perfect;
    public AudioClip amazing;
    
    public AudioClip default_drill;
    public AudioClip knock_drill;
    public AudioClip rolling_drill;
    public AudioClip after_ad;
    

    public AudioClip bgm;

    AudioSource general;
    AudioSource judgment;
    
    AudioSource bgm_source;
    
    AudioSource drill_operator;
    AudioSource drill_env;
    AudioSource drill_idle;

    static SoundManager instance;

    void Awake()
    {
        general = GetComponent<AudioSource>();
        judgment = gameObject.AddComponent<AudioSource>();
        bgm_source = gameObject.AddComponent<AudioSource>();

        drill_operator = gameObject.AddComponent<AudioSource>();
        drill_env = gameObject.AddComponent<AudioSource>();
        drill_idle = gameObject.AddComponent<AudioSource>();
        instance = this;
    }

    // Use this for initialization
    void Start () {
        
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    bool onoffBGM;
    bool onoffSound;

    static public void OnOffBGM(bool onoff)
    {
        if(onoff == true)
        {
            instance.bgm_source.volume = 0.25f;
        }
        else
        {
            instance.bgm_source.volume = 0f;
        }
    }

    static public void OnOffSound(bool onoff)
    {
        if (onoff == true)
        {
            instance.general.volume = 1f;
            instance.judgment.volume = 1f;
            instance.drill_operator.volume = 1f;
            instance.drill_env.volume = 1f;
            instance.drill_idle.volume = 1f;
        }
        else
        {
            instance.general.volume = 0f;
            instance.judgment.volume = 0f;
            instance.drill_operator.volume = 0f;
            instance.drill_env.volume = 0f;
            instance.drill_idle.volume = 0f;
        }
    }

    static public void PlayBGM()
    {
        instance.bgm_source.clip = instance.bgm;
        instance.bgm_source.loop = true;
        instance.bgm_source.Play();
    }
    
    static public void PlaySuccess()
    {
        instance.general.PlayOneShot(instance.success_level);
    }

    static public void PlayFail()
    {
        //instance.general.PlayOneShot(instance.fail_level);
    }

    static public void PlayAmazing()
    {
        instance.judgment.PlayOneShot(instance.amazing);
    }

    static public void PlayGoodPerfect()
    {
        instance.judgment.PlayOneShot(instance.good_perfect);
    }

    static public void PlayGetCoin()
    {
        instance.general.PlayOneShot(instance.get_coin);
    }

    static public void PlayFailNailAndTimeOver()
    {
        instance.general.PlayOneShot(instance.fail_nail_and_timeover);
    }

    static public void PlayFailOverAmazing()
    {
        Debug.Log("Fail Amazing===================");
        instance.general.PlayOneShot(instance.fail_over_amazing);
    }

    static public void PlayBuyDrill()
    {
        instance.general.PlayOneShot(instance.buy_drill);
    }

    static public void PlayGetNewDrillWaiting()
    {
        instance.general.PlayOneShot(instance.get_new_wating);
    }

    static public void StopGetNewDrillWaiting()
    {
        instance.general.Stop();
    }

    static public void PlayGetNewDrill()
    {
        instance.general.PlayOneShot(instance.get_new_drill);
    }

    static public void PlayAfterAD()
    {
        instance.general.PlayOneShot(instance.after_ad);
    }

    static public void PlayDefaultDrill()
    {
        instance.drill_operator.clip = instance.default_drill;
        instance.drill_operator.loop = true;
        instance.drill_operator.Play();
    }

    static public void PlayknockDrill()
    {
        instance.drill_operator.clip = instance.knock_drill;
        instance.drill_operator.loop = true;
        instance.drill_operator.Play();
    }

    static public void PlayRollingDrill()
    {
        instance.drill_operator.clip = instance.rolling_drill;
        instance.drill_operator.loop = true;
        instance.drill_operator.Play();
    }

    static public void StopDrillSound()
    {
        instance.drill_operator.Stop();
        instance.drill_operator.loop = false;
    }

    static public void PlayDrillAdditionOperation(AudioClip addition_operate)
    {
        instance.drill_operator.clip = addition_operate;
        instance.drill_operator.loop = true;
        instance.drill_operator.Play();
    }

    static public void PlayDillIdleSound(AudioClip addition_idle)
    {
        instance.drill_idle.clip = addition_idle;
        instance.drill_idle.loop = true;
        instance.drill_idle.Play();
    }
    
    static public void PlayDillEnvironmentSound(AudioClip addition_environment)
    {
        instance.stop_environment = false;
        instance.StartCoroutine(instance.PlayDrillEnvironment(addition_environment));
    }

    IEnumerator PlayDrillEnvironment(AudioClip addition_environment)
    {
        yield return new WaitForSeconds(Random.Range(1f, 5f));

        if (stop_environment == false)
        {
            instance.drill_env.PlayOneShot(addition_environment);
        }

        yield return new WaitForSeconds(Random.Range(4f, 6f));

        if (stop_environment == false)
        {
            StartCoroutine(PlayDrillEnvironment(addition_environment));
        }
    }

    bool stop_environment = true;
    static public void  StopDrillIdleAndEnvironmentSound()
    {
        instance.stop_environment = true;
        instance.drill_idle.Stop();
        instance.drill_env.Stop();
    }
}
