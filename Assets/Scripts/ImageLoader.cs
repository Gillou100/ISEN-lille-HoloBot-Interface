using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageLoader : MonoBehaviour
{
    public string url = "https://i.pinimg.com/originals/9e/1d/d6/9e1dd6458c89b03c506b384f537423d9.jpg";
    public Sprite mSprite;

    // automatically called when game started
    void Start()
    {
        mSprite = this.GetComponent<Sprite>();
        StartCoroutine(LoadFromLikeCoroutine()); // execute the section independently
    }

    // this section will be run independently
    private IEnumerator LoadFromLikeCoroutine()
    {
        Debug.Log("Loading ....");
        WWW wwwLoader = new WWW(url);   // create WWW object pointing to the url
        yield return wwwLoader;         // start loading whatever in that url ( delay happens here )

        Debug.Log("Loaded");
        //mSprite = wwwLoader.texture;  // set loaded image
    }
}
