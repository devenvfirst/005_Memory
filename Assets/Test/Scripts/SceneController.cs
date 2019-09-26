using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {

    public Transform cardParents;

    public const int gridRows = 3;
    public const int gridCols = 4;

    public float originX = -3;
    public float originY = 0;

    public const float offsetX = 2f;
    public const float offsetY = 2.5f;

    public GameObject originalCard;

    public Sprite[] images;

    private int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5 };

    //分数
    private int score;
    //步数
    private int step;
    //已找到图片组合数量
    private int winner;

    //UI界面
    public Text scoreLabel;
    public Text stepLabel;
    public GameObject victory;

    /// <summary>
    /// 俩次点击卡片的引用
    /// </summary>
    private MemoryCard _firClickCard;
    private MemoryCard _secClickCard;

    // Use this for initialization
    void Start () {

        victory.SetActive(false);
        InitMap();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 初始化地图（3x4）
    /// </summary>
    private void InitMap()
    {
        int tmpIndex = 0;
        int[] newArray = ShutNumberArray(numbers);
        for (int i = 0; i < gridCols; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                GameObject cardGo = Instantiate(originalCard,
                    new Vector2(originX, originY),
                    Quaternion.identity);

                //随机图片样式
                cardGo.GetComponent<SpriteRenderer>().sprite = images[newArray[tmpIndex]];

                //设置卡片的属性
                MemoryCard mc = cardGo.GetComponent<MemoryCard>();
                mc.SetCard(newArray[tmpIndex],images[newArray[tmpIndex]]);

                //数值检查
                if (tmpIndex < 12)
                {
                    tmpIndex++;
                }

                cardGo.transform.SetParent(cardParents);

                originY += offsetY;
            }
            originX += offsetX;
            originY = 0;
        }
    }
    
    /// <summary>
    /// 打乱数组顺序
    /// </summary>
    /// <param name="numbers">整形数组</param>
    /// <returns></returns>
    private int[] ShutNumberArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = Random.Range(i,newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }

        return newArray;
    }

    /// <summary>
    /// 判断第二张卡牌是否已经点击
    /// </summary>
    /// <returns>对象是否为null</returns>
    public bool canReveal()
    {
        return _secClickCard == null;
    }

    /// <summary>
    /// 点击卡牌要触发的方法
    /// </summary>
    /// <param name="mc"></param>
    public void RevealedCard(MemoryCard mc)
    {
        if(_firClickCard == null)
        {
            _firClickCard = mc;
        }
        else
        {
            _secClickCard = mc;
            //如果执行else循环，则说明俩张卡牌都已经点击,就进行匹配检查
            //执行一步校验
            if(_firClickCard != null && _secClickCard != null)
            {
                //步数增加
                step++;
                stepLabel.text = "当前步数：" + step;
                StartCoroutine("CheckMatched");
            }
        }
    }

    /// <summary>
    /// 检查卡牌类型
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckMatched()
    {

        if(_firClickCard.ID == _secClickCard.ID)
        {
            //卡片类型相同
            //加分数或者显示特效
            winner++;
            score += 10;
            scoreLabel.text = "当前分数：" + score;

            if(winner == (gridRows * gridCols / 2))
            {
                victory.gameObject.SetActive(true);
            }
        }
        else
        {
            //卡片类型不同

            //等待1.5s
            yield return new WaitForSeconds(1.5f);
            
            //显示卡背
            _firClickCard.UnReveal();
            _secClickCard.UnReveal();
        }

        //对象引用置空
        _firClickCard = null;
        _secClickCard = null;
    }
}
