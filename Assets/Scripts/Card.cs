using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core.Easing;

public class Card : MonoBehaviour
{
    [SerializeField] private SpriteRenderer cardRenderer;
    [SerializeField] private Sprite animalSprite;
    [SerializeField] private Sprite backSprite;

    public int cardID;
    
    private bool isFlipped;
    private bool isFlipping;
    private bool isMatched = false;
    private Vector3 originalScale, targetScale;

    private void Start()
    {
        isFlipped = false;
        isFlipping = false;
        originalScale = transform.localScale;
        targetScale = new Vector3(0f, originalScale.y, originalScale.z);
    }

    public void SetCardID(int id)
    {
        cardID = id;
    }

    public void SetMatched()
    {
        isMatched = true;
    }
    
    public void SetAnimalSprite(Sprite sprite)
    {
        animalSprite = sprite;
    }

    public void FlipCard()
    {
        isFlipping = true;
        
        transform.DOScale(targetScale, 0.2f).OnComplete(() =>
        {
            isFlipped = !isFlipped;
                    
            if (isFlipped)
            {
                cardRenderer.sprite = animalSprite;
            }
            else
            {
                cardRenderer.sprite = backSprite;
            }

            transform.DOScale(originalScale, 0.2f).OnComplete(() => isFlipping = false);
        });
    }
    
    private void OnMouseDown()
    {
        if(!isFlipping && !isMatched && !isFlipped)
            GameManager.instance.CardClicked(this);
    }
}
