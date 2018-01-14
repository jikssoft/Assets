using UnityEngine;

public class LocalizeTextMesh : MonoBehaviour {

    TextMesh text_mesh;

    public string key;

	// Use this for initialization
	void Start () {
        text_mesh = GetComponent<TextMesh>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetKey(string localize_key)
    {
        text_mesh = GetComponent<TextMesh>();
        key = localize_key;
        OnLocalize();
    }

    public void OnLocalize()
    {
        text_mesh.text = Localization.Get(key);
    }
}
