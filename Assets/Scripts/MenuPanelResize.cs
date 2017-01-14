using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanelResize : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        var sr = GetComponentInChildren<Image>();

        if (sr == null) return;
        Debug.Log("Attempting resize");
        transform.localScale = new Vector3(1, 1, 1);

        var width = sr.sprite.bounds.size.x;
        var height = sr.sprite.bounds.size.y;

        var worldScreenHeight = Camera.main.orthographicSize * 2.0;
        var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        float widthScale = (float)(worldScreenWidth / width);
        float heightScale = (float)(worldScreenHeight / height);

        transform.localScale = new Vector2(widthScale, heightScale);
    }
	
}
