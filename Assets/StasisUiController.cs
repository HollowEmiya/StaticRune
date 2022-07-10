using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StasisUiController : MonoBehaviour
{
    public static StasisUiController instance;

    public CanvasGroup canvasGroup;
    public RectTransform animatorUiSquare;
    public Sprite noTargetSprite;
    public Sprite targetSprite;

    private void Awake()
    {
        instance = this;
        canvasGroup.alpha = 0;
        animatorUiSquare = GetComponentsInChildren<RectTransform>()[2];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterStasis()
    {
        canvasGroup.alpha = 1;
    }

    public void ExitStasis()
    {
        canvasGroup.alpha = 0;
    }

    public void FindTarget()
    {
        animatorUiSquare.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void ResetTarget()
    {
        animatorUiSquare.localScale = Vector3.one;
    }

    public void LockedTarget()
    {
        RectTransform[] rects = animatorUiSquare.GetComponentsInChildren<RectTransform>();
        //print(rects[1].name);
        for(int i = 1; i < rects.Length; i++)
        {
            rects[i].GetComponent<Image>().sprite = targetSprite;
        }
    }

    public void ResetLockedTarget()
    {
        RectTransform[] rects = animatorUiSquare.GetComponentsInChildren<RectTransform>();
        for (int i = 1; i < rects.Length; i++)
        {
            rects[i].GetComponent<Image>().sprite = noTargetSprite;
        }
    }
}
