using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 卡片类
/// </summary>
public class MemoryCard : MonoBehaviour {

    public GameObject cardBcak;

    private int id;

    /// <summary>
    /// 卡片ID
    /// </summary>
    public int ID
    {
        get
        {
            return id;
        }
    }

    /// <summary>
    /// 设置卡片属性
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="img">图片</param>
    public void SetCard(int id,Sprite img)
    {
        this.id = id;
        GetComponent<SpriteRenderer>().sprite = img;
    }

    /// <summary>
    /// 当鼠标点击卡片时
    /// </summary>
    private void OnMouseDown()
    {
        SceneController sc = FindObjectOfType<SceneController>();

        //如果第二张卡片未翻开才传递数据
        if(cardBcak.activeSelf && sc.canReveal() == true)
        {
            //显示卡正并传递卡片数据
            cardBcak.SetActive(false);
            FindObjectOfType<SceneController>().RevealedCard(this);
        }
        else
        {
            //友情提示
            Debug.Log("第二张已翻开，请等待系统检查才可点击其他卡片！");
        }
    }

    /// <summary>
    /// 显示卡背
    /// </summary>
    public void UnReveal()
    {
        cardBcak.SetActive(true);
    }
}
