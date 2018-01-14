using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardCoinController : MonoBehaviour {

    public GameObject coin_obj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float ani_time = 2f;
    private int reward_count = 30;

    public void CreateRewardCoin(Transform target)
    {
        coin_camera.gameObject.SetActive(true);
        for (int i = 0; i < reward_count; i++)
        {
            StartCoroutine(BuildCoinRoute(target, i));
        }
        StartCoroutine(DisableCamera(reward_count));
        SoundManager.PlayAfterAD();

        StartCoroutine(SaveCloud());
    }

    public void CreateCoin(Transform target, int count, float time)
    {
        coin_camera.gameObject.SetActive(true);
        for (int i = 0; i < count; i++)
        {
            StartCoroutine(BuildCoinRoute(target, i, time));
        }
        StartCoroutine(DisableCamera(count, time));
        SoundManager.PlayAfterAD();

        StartCoroutine(SaveCloud(time));
    }

    IEnumerator SaveCloud(float time = 0.07f)
    {
        yield return new WaitForSeconds(ani_time + (time * (reward_count + 2)));
        GameObject System = GameObject.FindGameObjectWithTag("System");
        System.GetComponent<GameDataSystem>().SaveCloud();

    }

    public IEnumerator DisableCamera(int reward_count, float time = 0.07f)
    {
        yield return new WaitForSeconds(reward_count * time + 0.2f);
        coin_camera.gameObject.SetActive(false);
    }

    public Camera coin_camera;

    public DrillShopList drill_shop_list;
    public IEnumerator BuildCoinRoute(Transform target, int i, float time = 0.07f)
    {
        GameObject clon = Instantiate(coin_obj);//, coin_obj.transform.position, coin_obj.transform.rotation);
        yield return new WaitForEndOfFrame();
        CoinRouteBuilder builder = clon.GetComponent<CoinRouteBuilder>();
        builder.time = ani_time + (time * i);
        builder.StartRoute(target);
        yield return new WaitForSeconds(builder.time);

        TweenPosition tween = target.parent.GetComponent<TweenPosition>();

        GameObject System = GameObject.FindGameObjectWithTag("System");
        System.GetComponent<GameMainLogicSystem>().GetCoin();
        drill_shop_list.UpdateCoin();

        if (tween.tweenFactor == 1)
        {
            tween.ResetToBeginning();
        }

        if (tween.tweenFactor == 0)
        {
            tween.PlayForward();
        }
        else
        {
            tween.ResetToBeginning();
        }
    }
}
