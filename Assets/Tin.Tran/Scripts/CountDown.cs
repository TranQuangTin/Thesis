using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public static CountDown Global;
    public Text TxtTime;
    public IEnumerator StartCountDown(int time, Action timeout)
    {
        int t = time;
        while (true)
        {
            if (t >= 0)
            {
                TxtTime.text = t.ToString();
                yield return new WaitForSeconds(1);
                t--;
            }
            else
            {
                //timeout-- lose
                gameObject.SetActive(false);
                timeout();
            }
        }
    }
}
