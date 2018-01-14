using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRouteBuilder : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private Vector3[] path;

    public float time;

    public void StartRoute(Transform target)
    {
        StartCoroutine(RouteCoroutine(target));

        /*path = new Vector3[6];

        GameObject coin_target = GameObject.FindWithTag("CoinIcon");
        float delta_y = coin_target.transform.position.y - gameObject.transform.position.y;
        float random_x = Random.Range(0.7f, 1f);
        float random_y = Random.Range(0.7f, 1f);
        float z_pos = -11f;

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, z_pos);

        random_x *= Random.Range(0f, 1f) > 0.5f ? -1 : 1;
        random_y *= Random.Range(0f, 1f) > 0.5f ? -1 : 1;
        random_x += gameObject.transform.position.x;
        random_y += gameObject.transform.position.y;

        path[0] = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, z_pos);
        path[1] = new Vector3(random_x, random_y, z_pos);
        path[2] = new Vector3(random_x, random_y, z_pos);
        path[3] = new Vector3(random_x, random_y, z_pos);
        path[4] = new Vector3(gameObject.transform.position.x + Random.Range(0f, 1f), gameObject.transform.position.y + (delta_y / 3f), z_pos);
        //path[1] = new Vector3(Random.Range(coin_target.transform.position.x+1.5f, 0f), gameObject.transform.position.y + (delta_y * 2f/3f));
        path[5] = new Vector3(coin_target.transform.position.x, coin_target.transform.position.y, z_pos);
        //path[6] = new Vector3(coin_target.transform.position.x, coin_target.transform.position.y, z_pos);

        iTween.MoveTo(gameObject, iTween.Hash("movetopath", false, "path", path, "time", time, "easytype", iTween.EaseType.easeInExpo, "oncomplete", "DestroyCoin"));
        */
    }

    public Vector2 Vector2FromAngle(float a)
    {
        a *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
    }

    IEnumerator RouteCoroutine(Transform target)
    {
        Vector2 vector = Vector2FromAngle(Random.Range(0f, 360f));
        vector *= Random.Range(0.7f, 1f);

        float random_x = vector.x;
        float random_y = vector.y;
        float z_pos = -11f;

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, z_pos);

        random_x *= Random.Range(0f, 1f) > 0.5f ? -1 : 1;
        random_y *= Random.Range(0f, 1f) > 0.5f ? -1 : 1;
        random_x += gameObject.transform.position.x;
        random_y += gameObject.transform.position.y;

        iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(random_x, random_y, z_pos), 
            "time", time / 2f, 
            "easytype", iTween.EaseType.easeInOutQuad));

        yield return new WaitForSeconds(time/2f);
        yield return new WaitForSeconds(0.01f);
        
        iTween.MoveTo(gameObject, iTween.Hash("position", target.position,
            "time", time / 2f,
            "easytype", iTween.EaseType.easeOutQuart,
            "oncomplete", "DestroyCoin"));
    }

    public void DestroyCoin()
    {
        Destroy(gameObject);
    }
}
