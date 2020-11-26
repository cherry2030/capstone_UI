using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class test : MonoBehaviour
{
    public Button targetButton;// likeFurnButton, noLikeFurnButton, 
    private GameObject likeFurniImage, noLikeFurnImage;
    private Image targetImage;
    private bool nowLike,isReseted=false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetLikeButton(bool _isLiked)
    {
        if (!isReseted)
        {
            nowLike = _isLiked;
            isReseted = true;
        }

    }

    public void PushLikeButton()
    {        
        if (nowLike)
        {
            targetButton.GetComponent<Image>().sprite = Resources.Load("image/searchresult_빈하투사진", typeof(Sprite)) as Sprite;
            nowLike = false;
        }
        else
        {
            targetButton.GetComponent<Image>().sprite = Resources.Load("image/searchresult_꽉찬하투사진", typeof(Sprite)) as Sprite;
            nowLike = true;
        }
    }
}
