using UnityEngine;
using System.Collections;

public class Nail : MonoBehaviour {

    public enum NAIL_STATE { FAIL, GOOD, PERFECT, AMAZING }
    public enum DIRECTION { UP, DOWN, LEFT, RIGHT }
    public enum DISTURB_TYPE {NORMAL, ROTATE, ZOOM_OUT, ZOOM_IN, PRE_ZOOM_OUT, PRE_ZOOM_IN, PERFECT, TWICE_SPEED, SLOW_SPEED}

    public DIRECTION direction;
    public NAIL_STATE collision_state;
    public DISTURB_TYPE disturb_type;
    public GameObject image;

    public Sprite normal;
    public Sprite fast_nail;
    public Sprite slow_nail;
    public Sprite perfect_nail;

    public Sprite guide_slow_down;
    public Sprite guide_perfect;
    public Sprite guide_speed_up;
    public GameObject guide;

    

    private bool coin;


    // Use this for initialization
    void Start() {

        if (coin == true)
        {
            coin_obj.SetActive(true);
            coin = true;
        }
        else
        {
            coin_obj.SetActive(false);
            coin = false;
        }
        
    }

    public void SetDisturbType(DISTURB_TYPE type)
    {
        SpriteRenderer nail_sprite = image.GetComponent<SpriteRenderer>();
        
        if(type == DISTURB_TYPE.PERFECT)
        {
            nail_sprite.sprite = perfect_nail;
            //guide.GetComponent<LocalizeTextMesh>().SetKey("Perfect Only");
            //guide.SetActive(true);
        }
        else if(type == DISTURB_TYPE.TWICE_SPEED)
        {
            nail_sprite.sprite = fast_nail;
            guide.GetComponent<LocalizeTextMesh>().SetKey("Fast");
            guide.SetActive(true);
        }
        else if(type == DISTURB_TYPE.SLOW_SPEED)
        {
            nail_sprite.sprite = slow_nail;
            //guide.GetComponent<LocalizeTextMesh>().SetKey("Slow");
           // guide.SetActive(true);
        }
        else
        {
            nail_sprite.sprite = normal;
            guide.SetActive(false);
        }

        disturb_type = type;
    }

    public void DisableGudie()
    {
        if(guide.activeSelf ==  true)
        {
            guide.SetActive(false);
        }
        
    }

    float operate_speed = 0f;

    // Update is called once per frame
    void Update () {
        
        if (operate_speed < 0f)
        {
            gameObject.transform.Translate(0f, operate_speed, 0f);
        }

    }
    
	Vector3 start_pos;
	public void SaveStartPos()
	{
		start_pos = new Vector3 (gameObject.transform.localPosition.x,
			gameObject.transform.localPosition.y,
			gameObject.transform.localPosition.z);
	}

	public void SetStartPos()
	{
		gameObject.transform.localPosition = start_pos;
		cleared = false;
	}

    public void CollisionEnter(NAIL_STATE state)
    {
        if(cleared == true)
        {
            return;
        }

        collision_state = state;
        if(state == NAIL_STATE.FAIL)
        {
            
            GameObject system = GameObject.FindGameObjectWithTag("System");
            system.GetComponent<GameMainLogicSystem>().GameOver(this);
        }
    }

    public void CollisionExit(NAIL_STATE state)
    {

    }

    public void Screw(float speed)
    {
        operate_speed = speed;
        Debug.Log(operate_speed);
    }

    public void Stop()
    {
        //GameObject system = GameObject.FindGameObjectWithTag("System");
        operate_speed = 0f;
        Debug.Log("------------ stop");
    }

    public GameObject coin_obj;
    public GameObject coin_icon;
    public void SetCoin(bool value)
    {
        coin = value;
        coin_obj.SetActive(true);
    }

    bool cleared = false;
    public void Clear(GameMainLogicSystem system)
    {
        cleared = true;
        if (collision_state == NAIL_STATE.AMAZING)
        {
            int count = 1;
            if(disturb_type == DISTURB_TYPE.TWICE_SPEED)
            {
                count = 2;
            }
            for (int i = 0; i < count; i++)
            {
                StartCoroutine(BuildAmazingCoinRoute(system));
            }
            SoundManager.PlayGetCoin();
        }       

        if (coin == true)
        {
            if (collision_state == NAIL_STATE.PERFECT ||
                collision_state == NAIL_STATE.AMAZING)
            {
                coin_icon.transform.parent = null;
                StartCoroutine(BuildCoinRoute(system));
                coin_obj.SetActive(false);
                SoundManager.PlayGetCoin();
            }
        }
    }

    public float coin_ani_time;
    IEnumerator BuildCoinRoute(GameMainLogicSystem system)
    {
        GameObject coin_target = GameObject.FindWithTag("CoinIcon");

        CoinRouteBuilder builder = coin_icon.GetComponent<CoinRouteBuilder>();
        builder.time = coin_ani_time;
        builder.StartRoute(coin_target.transform);
        yield return new WaitForSeconds(coin_ani_time);
        system.GetCoin();
        
        GameObject coinObj = GameObject.FindGameObjectWithTag("CoinObj");
        TweenPosition tween = coinObj.GetComponent<TweenPosition>();

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

    IEnumerator BuildAmazingCoinRoute(GameMainLogicSystem system)
    {
        GameObject coin_target = GameObject.FindWithTag("CoinIcon");

        GameObject clon = Instantiate(coin_icon, coin_icon.transform.position, coin_icon.transform.rotation);
        CoinRouteBuilder builder = clon.GetComponent<CoinRouteBuilder>();
        builder.time = coin_ani_time + UnityEngine.Random.Range(0f, 0.2f);
        builder.StartRoute(coin_target.transform);
        yield return new WaitForSeconds(builder.time);
        system.GetCoin();

        GameObject coinObj = GameObject.FindGameObjectWithTag("CoinObj");
        TweenPosition tween = coinObj.GetComponent<TweenPosition>();

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

