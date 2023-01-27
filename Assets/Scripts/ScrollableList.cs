using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollableList : MonoBehaviour
{
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
}
