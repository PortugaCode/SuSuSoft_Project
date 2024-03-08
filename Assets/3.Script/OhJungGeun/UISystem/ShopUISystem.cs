using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUISystem : MonoBehaviour
{
    [Header("Test Shop DBManager")]
    private int[,] dailyItemData = new int[3,2] { {0,1}, {1,1}, {2, 1} };
    private int[,] tokenItemData = new int[6,2] { {0,3}, {1,1}, {2, 3}, { 3, 0 }, { 4, 3 }, { 5, 3 } };


    [Header("Token Data")]
    /// Token Image 모음 및 가격
    /// 0 : 농구공         토큰  / 400골드
    /// 1 : 울타리         토큰  / 200골드
    /// 2 : 수풀           토큰  / 200골드
    /// 3 : 나무           토큰  / 200골드
    /// 4 : 쌍둥이 나무    토큰  / 200골드
    /// 5 : 테니스공       토큰  / 400골드
    /// 6 : 골프           토큰  / 400골드
    /// 7 : 축구공         토큰  / 400골드
    /// 8 : 사과 나무      토큰  / 200골드
    /// 9 : 트로피         토큰  / 400골드
    /// 초기화 500원
    ///
    [SerializeField] private Sprite[] tokenImage;
    [SerializeField] private int[] tokenPrice = new int[3] { 400, 200, 200 };
    [SerializeField] private string[] tokenNames = new string[10]
        {
            "농구공 토큰", "울타리 토큰", "수풀 토큰", "나무 토큰", "쌍둥이 나무 토큰", "테니스공 토큰", "골프 토큰", "축구공 토큰", "사과 나무 토큰", "트로피 토큰"
        };
    
    //즉시 갱신 가격
    [SerializeField] private int resetListPrice = 500;

    [Header("UI GameObject")]
    [SerializeField] private GameObject shopPopUp;
    [SerializeField] private GameObject updatePopUp;
    [SerializeField] private GameObject errorPopUp;

    [Header("Daily Shop Data")]
    [SerializeField] private TextMeshProUGUI[] dailyItemNames;
    [SerializeField] private TextMeshProUGUI[] dailyItemPrices;
    [SerializeField] private int[] dailyItemIntPrices = new int[3] { 0, 500, 100 };
    [SerializeField] private Image[] dailyItemImages;
    [SerializeField] private List<int> dailyItemIndex;

    [Header("Token Shop Data")]
    [SerializeField] private TextMeshProUGUI[] tokenItemNames;
    [SerializeField] private TextMeshProUGUI[] tokenItemPrices;
    [SerializeField] private int[] tokenItemIntPrices = new int[6];
    [SerializeField] private Image[] tokenItemImages;
    [SerializeField] private TextMeshProUGUI[] tokenRemain;
    [SerializeField] private List<int> tokenItemIndex;

    [Header("Gold Shop Data")]
    [SerializeField] private int[] getGold = new int[3] { 100, 2000, 40000};
    [SerializeField] private int[] goldPrices = new int[3] {0, 100, 500};


    private void Start()
    {
        SetDailyShopItem();
        ShuffleTokenData();
    }

    #region[Set & Update ShopItem]
    private void SetDailyShopItem()
    {
        dailyItemIndex.Clear();

        for (int i = 0; i < dailyItemNames.Length; i++)
        {
            //해당 품복 인덱스 저장
            dailyItemIndex.Add(dailyItemData[i, 0]);

            //상점 품목 이미지 및 텍스트 변경
            dailyItemNames[i].text = tokenNames[dailyItemIndex[i]];
            dailyItemPrices[i].text = (dailyItemIntPrices[i].Equals(0)) ? "무료" : $"{dailyItemIntPrices[i].ToString()} 골드";
            dailyItemImages[i].sprite = tokenImage[dailyItemIndex[i]];

            if(dailyItemData[i,1] <= 0)
            {
                dailyItemImages[i].color = new Color32(255, 255, 255, 120);
            }
            else
            {
                dailyItemImages[i].color = new Color32(255, 255, 255, 255);
            }
        }
    }

    private void SetTokenShopItem()
    {
        tokenItemIndex.Clear();

        for (int i = 0; i < tokenItemNames.Length; i++)
        {
            //해당 품복 인덱스 저장
            tokenItemIndex.Add(tokenItemData[i, 0]);

            //상점 품목 이미지 및 텍스트 변경
            tokenItemNames[i].text = tokenNames[tokenItemIndex[i]];
            tokenItemPrices[i].text = $"{GetTokenPrice(tokenItemIndex[i]).ToString()} 골드";
            tokenItemIntPrices[i] = GetTokenPrice(tokenItemIndex[i]);
            tokenRemain[i].text = $"{tokenItemData[i, 1]} / 3";
            tokenItemImages[i].sprite = tokenImage[tokenItemIndex[i]];

            if(tokenItemData[i,1] <= 0)
            {
                tokenItemImages[i].color = new Color32(255, 255, 255, 120);
            }
            else
            {
                tokenItemImages[i].color = new Color32(255, 255, 255, 255);
            }
        }
    }

    private void UpdateDailyShopItem()
    {
        for (int i = 0; i < dailyItemNames.Length; i++)
        {
            //상점 품목 이미지 및 텍스트 변경
            dailyItemNames[i].text = tokenNames[dailyItemIndex[i]];
            dailyItemPrices[i].text = (dailyItemIntPrices[i].Equals(0)) ? "무료" : $"{dailyItemIntPrices[i].ToString()} 골드";
            dailyItemImages[i].sprite = tokenImage[dailyItemIndex[i]];

            if (dailyItemData[i, 1] <= 0)
            {
                dailyItemImages[i].color = new Color32(255, 255, 255, 120);
            }
            else
            {
                dailyItemImages[i].color = new Color32(255, 255, 255, 255);
            }
        }
    }

    private void UpdateTokenShopItem()
    {
        for (int i = 0; i < tokenItemNames.Length; i++)
        {
            //상점 품목 이미지 및 텍스트 변경
            tokenItemNames[i].text = tokenNames[tokenItemIndex[i]];
            tokenItemPrices[i].text = $"{GetTokenPrice(tokenItemIndex[i]).ToString()} 골드";
            tokenItemIntPrices[i] = GetTokenPrice(tokenItemIndex[i]);
            tokenRemain[i].text = $"{tokenItemData[i, 1]} / 3";
            tokenItemImages[i].sprite = tokenImage[tokenItemIndex[i]];

            if (tokenItemData[i, 1] <= 0)
            {
                tokenItemImages[i].color = new Color32(255, 255, 255, 120);
            }
            else
            {
                tokenItemImages[i].color = new Color32(255, 255, 255, 255);
            }
        }
    }
    #endregion


    #region[Shuffle TokenData]
    public void ShuffleTokenData()
    {
        for(int i = 0; i < 6; i++)
        {
            int rand = Random.Range(0, tokenImage.Length);
            tokenItemData[i, 0] = rand;
            tokenItemData[i, 1] = 3;
        }
        SetTokenShopItem();
    }
    #endregion


    public void BuyToken_DailyShop(int index)
    {
        //살 수 있는 지 확인 메서드 (돈 + 갯수 확인)


        //토큰 데이터에서 갯수 빼기 (DB)
        dailyItemData[index, 1] -= 1;

        //토큰 갯수 올리기 (DB)
        //DBManager.instance.user.tokens[tokenItemData[index, 0]] += 1;


        //토큰 업데이트하기
        UpdateDailyShopItem();
    }

    public void BuyToken_TokenShop(int index)
    {
        //살 수 있는 지 확인 메서드 (돈 + 갯수 확인)


        //토큰 데이터에서 갯수 빼기 (DB)
        tokenItemData[index, 1] -= 1;
        tokenRemain[index].text = $"{tokenItemData[index, 1]} / 3";

        //토큰 갯수 올리기 (DB)
        //DBManager.instance.user.tokens[tokenItemData[index, 0]] += 1;


        //토큰 업데이트하기
        UpdateTokenShopItem();
    }



    private int GetTokenPrice(int index)
    {
        bool value = index == 0 || index == 9 || (index >= 5 && index <= 7);
        if (value) return 400;
        else return 200;
    }

}
