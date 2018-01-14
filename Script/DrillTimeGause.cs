using UnityEngine;
using System.Collections;

public class DrillTimeGause : MonoBehaviour {

    public _2dxFX_Clipping gause;
    public _2dxFX_Clipping gause_red;

    // Use this for initialization
    void Start () {
	
	}

    int count = 0;
    // Update is called once per frame

    bool pause = false;

	void Update () {

        if (time > 0f && pause == false)
        {
            float t = (Time.fixedTime - start_time) / time;
            if (t > 1f)
            {
                t = 1f;
            }
            gause._ClipDown = t;
            gause_red._ClipDown = t;

            if(t > 0.6f)
            {
                if(count > 20)
                {
                    gause._Alpha = 0f;
                }
                else
                {
                    gause._Alpha = 1f;
                }
            }
            count++;
            if(count > 40)
            {
                count = 0;
            }
        }
        else if(time < 0f)
        {
            gause._ClipDown = 0f;
            gause._Alpha = 1f;
            gause_red._ClipDown = 0f;
        }
	}

    float pased_time;

    public void PauseGauseTime()
    {
        pause = true;
        pased_time = Time.fixedTime;
    }

    public void ResumeGauseTime()
    {
        start_time += Time.fixedTime - pased_time;
        pause = false;
    }
    
    public void SetGauseTime(float t)
    {
        gameObject.SetActive(true);
        time = t;
        start_time = Time.fixedTime;
        gause._Alpha = 1f;
        pause = false;
    }

    public void DisableGauseTime()
    {
        gameObject.SetActive(false);
    }

    public void EnableGauseTime()
    {
        gameObject.SetActive(true);
    }

    public void StopGauseTime()
    {
        time = -1f;
    }

    float time = -1f;
    float start_time;

    public float GetRemainGauseTime()
    {
        if(pause == true)
        {
            return 1f;
        }

        if (time > 0f)
        {
            return time - (Time.fixedTime - start_time);
        }
        else
        {
            return 1f;
        }
    }

    
}
