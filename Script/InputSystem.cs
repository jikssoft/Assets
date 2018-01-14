using UnityEngine;
using System.Collections;

public class InputSystem : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {

            Vector2 pos = Input.GetTouch(0).position;    // 터치한 위치
            Vector3 theTouch = new Vector3(pos.x, pos.y, 0.0f);    // 변환 안하고 바로 Vector3로 받아도 되겠지.

            Ray ray = Camera.main.ScreenPointToRay(theTouch);    // 터치한 좌표 레이로 바꾸엉

            Debug.Log("Touched1");

            RaycastHit hit;    // 정보 저장할 구조체 만들고
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))    // 레이저를 끝까지 쏴블자. 충돌 한넘이 있으면 return true다.
            {
                Debug.Log("Touched2");

                if (Input.GetTouch(0).phase == TouchPhase.Began)    // 딱 처음 터치 할때 발생한다
                {
                    GetComponent<GameMainLogicSystem>().TapDown();
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)    // 터치 따악 떼면 발생한다.
                {
                    GetComponent<GameMainLogicSystem>().TapUp();
                }

            }

        }

        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<GameMainLogicSystem>().TapDown();
        }
        if (Input.GetMouseButtonUp(0))
        {
            GetComponent<GameMainLogicSystem>().TapUp();
        }
    }

    public float y_force = 2f;
}
