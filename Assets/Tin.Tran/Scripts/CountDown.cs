using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public static CountDown Global;
    public Text TxtTime;
    private int time;
    public void Count(int t, Action timeout)
    {
        time = t;
        StartCoroutine(StartCountDown(timeout));
    }
    public IEnumerator StartCountDown(Action timeout)
    {
        if (time >= 0)
        {
            TxtTime.text = time.ToString();
            yield return new WaitForSeconds(1);
            time--;
            StartCoroutine(StartCountDown(timeout));
        }
        else
        {
            timeout();
            gameObject.SetActive(false);
        }
    }
}
