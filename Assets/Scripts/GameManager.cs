using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Slider timeoutSlider;
    [SerializeField] private float timeLimit = 60f;
    private float currentTime;

    [SerializeField] private TMP_Text timeoutText;
    
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text resultText;
    private bool isGameOver = false;

    private IEnumerator countDownCourtine;
    
    public static GameManager instance;
    private List<Card> allCards;
    private Card flipedCard;
    private bool isFlipping = false;

    private int totalMatches = 10;
    private int matchesFound = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    void Start()
    {
        Board board = FindObjectOfType<Board>();
        allCards = board.GetCards(); // ?? : 카드 정보가 다 저장되어있는 상황인가? 저장 안되있으면 어캄

        currentTime = timeLimit;
        SetCurrentTimeText();
        
        StartCoroutine(FlipAllCardsRoutine());
    }

    void SetCurrentTimeText()
    {
        int timeSec = Mathf.CeilToInt(currentTime);
        timeoutText.SetText(timeSec.ToString());
    }

    IEnumerator FlipAllCardsRoutine()
    {
        isFlipping = true;
        yield return new WaitForSeconds(0.5f); // 0.5초 대기
        FlipAllCards();
        yield return new WaitForSeconds(1.5f);
        FlipAllCards();
        yield return new WaitForSeconds(0.5f); // 카드 뒤집히는 시간 조금 더 대기
        isFlipping = false;

        countDownCourtine = CountdownTimerRoutine();
        yield return StartCoroutine(countDownCourtine);
    }

    IEnumerator CountdownTimerRoutine()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            timeoutSlider.value = currentTime / timeLimit; // 범위: 0~1
            SetCurrentTimeText();
            yield return null; // 대기 안하고 바로 다음 프레임에 작업 다시 시작
        }
        
        // GameOver
        GameOver(false);
    }

    void FlipAllCards()
    {
        foreach (Card card in allCards)
        {
            card.FlipCard();
        }
    }

    public void CardClicked(Card card)
    {
        if (isFlipping || isGameOver)
            return;
        
        card.FlipCard();

        if (flipedCard == null)
        {
            flipedCard = card;
        }
        else
        {
            // check match
            StartCoroutine(checkMatchRoutine(flipedCard, card));
        }
    }

    IEnumerator checkMatchRoutine(Card card1, Card card2)
    {
        isFlipping = true;
        
        if (card1.cardID == card2.cardID)
        {
            card1.SetMatched();
            card2.SetMatched();
            matchesFound++;
            
            if(matchesFound == totalMatches)
                GameOver(true);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);

            card1.FlipCard();
            card2.FlipCard();
            
            yield return new WaitForSeconds(0.4f);
        }

        flipedCard = null;
        isFlipping = false;
    }

    void GameOver(bool success)
    {
        if (!isGameOver)
        {
            isGameOver = true;
            StopCoroutine(countDownCourtine); // CountdownTimerRoutine() 이거로 하면 작동 안함
            
            if (success)
            {
                resultText.SetText("WIN");
            }
            else
            {
                resultText.SetText("LOSE");
            }
            
            Invoke("ShowGmaeOverPanel", 2f); // 2초 뒤 표시
        }
    }

    void ShowGmaeOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void Restart() // 씬 재로드로 재시작
    {
        SceneManager.LoadScene("SampleScene");
    }
}
