using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    public GameObject panelToOpen;

    private bool isActive;
    ScrollableList scrollableList;

    Animator animator;
    Animator scrollableListAnimator;
    void Awake()
    {
        animator = GetComponent<Animator>();
        scrollableListAnimator = FindObjectOfType<ScrollableList>().GetComponent<Animator>();
        isActive = false;
    }

    public void OpenPanel()
    {
        if (panelToOpen == null) { return; }
        else
        {
            PlayAnimation();
        }
    }

    public void PlayAnimation()
    {
        if(isActive)
        {
            scrollableListAnimator.SetTrigger("TrClose");
            animator.SetTrigger("TrClose");   
        }
        else
        {
            scrollableListAnimator.SetTrigger("TrOpen");
            animator.SetTrigger("TrOpen");
        }
        isActive = !isActive;
    }
}
