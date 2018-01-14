using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaBallController : MonoBehaviour {

    public Vector3 shake_pos;
    public GameObject position;
    public GameObject scale;
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator StartAni()
    {
        SoundManager.PlayGetNewDrillWaiting();
        iTween.Stop(position);
        iTween.Stop(scale);
        yield return new WaitForEndOfFrame();
        scale.transform.localScale = new Vector3(1f, 1f, 1f);
        iTween.ShakePosition(position, iTween.Hash("amount", shake_pos, "time", float.MaxValue, "islocal", true));
        iTween.ScaleTo(scale, iTween.Hash("scale", new Vector3(0.7f, 0.7f), "easetype", iTween.EaseType.easeInOutElastic, "time", 2.5f));
        yield return new WaitForSeconds(2.5f);
        SoundManager.StopGetNewDrillWaiting();
    }
}
