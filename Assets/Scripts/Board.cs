using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Sprite[] cardSprites; // 10

    private List<int> cardIDList = new List<int>();
    private List<Card> cardList = new List<Card>();
    
    void Start()
    {
        GenerateCardID();
        SuffleCardID(); // ID 선 뒤섞기
        InitBoard();
    }

    void GenerateCardID()
    {
        for (int i = 0; i < cardSprites.Length; i++)
        {
            cardIDList.Add(i);
            cardIDList.Add(i); // 0,0,1,1,2,2 ... 9,9
        }
    }

    void SuffleCardID()
    {
        for (int i = 0; i < cardIDList.Count; i++) // 20
        {
            int randomIndex = Random.Range(i, cardIDList.Count);
            (cardIDList[randomIndex], cardIDList[i]) = (cardIDList[i], cardIDList[randomIndex]);
        }
    }

    void InitBoard()
    {
        float spaceY = 1.8f;
        float spaceX = 1.3f;
        
        int rowCount = 5; // 여기서 나누기 2하면 중앙에 옴, (row - (int)(rowCount / 2)) * spaceY
        // 0 - 2 = -2 * 1.8f(== spaceY) = 0번째 카드의 Y 좌표 위치
        // 1 - 2 = -1
        // 2 - 2 = 0 
        // 3 - 2 = 1
        // 4 - 2 = 2
        
        int colCount = 4;
        // (-1.5, -0.5, 0.5, 1.5)
        // 0 - 2 (==(colCount/2)) + 0.5(균형 맞추기용) = -1.5
        // 1 - 2 + 0.5 = -0.5
        // 2 - 2 + 0.5 = 0.5
        // 3 - 2 + 0.5 = 1.5
        // (col - (colCount / 2)) * spaceX + (spaceX / 2)

        int cardIndex = 0;

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < colCount; col++)
            {
                float posX = (col - (int)(colCount / 2)) * spaceX + (spaceX / 2);
                float posY = (row - (int)(rowCount / 2)) * spaceY - 0.2f;
                Vector3 pos = new Vector3(posX, posY, 0f);
                
                GameObject cardObject = Instantiate(cardPrefab, pos, Quaternion.identity);

                Card card = cardObject.GetComponent<Card>();
                int cardID = cardIDList[cardIndex++];
                
                card.SetCardID(cardID);
                card.SetAnimalSprite(cardSprites[cardID]); //cardIndex++/2도 가능

                cardList.Add(card);
            }
        }
    }

    public List<Card> GetCards()
    {
        return cardList;
    }
}
