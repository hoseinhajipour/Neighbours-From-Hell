using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sabotage : MonoBehaviour
{
    public float sabotageTime = 3f;
    public GameObject currentObject;
    public GameObject targetObject;
    public Slider sabotageSlider;
    private Animator animator;

    private bool isSabotaging = false;
    private float currentSabotageTime = 0f;
    public bool Broke = false;

    public float actionDistance = 2.0f;

    public int angryAmount;

    private void Start()
    {
        animator = GameObject.FindWithTag("Player").GetComponent<PlayerController>().animator;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isSabotaging)
        {
            isSabotaging = true;
//            animator.SetBool("isSabotaging", true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Broke == false)
        {
            if (other.CompareTag("Player") && isSabotaging)
            {
                float distance = Vector3.Distance(transform.position, other.transform.position);
                if (distance <= actionDistance)
                {
                    if (Input.GetKey(KeyCode.E))
                    {
                        currentSabotageTime += Time.deltaTime;
                        float progress = currentSabotageTime / sabotageTime;
                        sabotageSlider.value = progress;
                        animator.SetBool("isSabotaging", true);
                        sabotageSlider.gameObject.SetActive(true);
                        if (progress >= 1f)
                        {
                            isSabotaging = false;
                            currentSabotageTime = 0f;
                            sabotageSlider.value = 0f;
                            animator.SetBool("isSabotaging", false);
                            targetObject.SetActive(true);
                            currentObject.SetActive(false);
                            Broke = true;
                            sabotageSlider.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        currentSabotageTime = 0f;
                        sabotageSlider.value = 0f;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isSabotaging)
        {
            isSabotaging = false;
            currentSabotageTime = 0f;
            sabotageSlider.value = 0f;
            //    animator.SetBool("isSabotaging", false);
        }
    }

    public void DoSabotage()
    {
    }

    public void fix()
    {
        targetObject.SetActive(false);
        currentObject.SetActive(true);
        Broke = false;
    }
}