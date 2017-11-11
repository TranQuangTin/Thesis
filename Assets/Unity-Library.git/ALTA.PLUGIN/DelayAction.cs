using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
namespace Alta.Plugin
{
    public static class DelayAction
    {
        public static IEnumerator WaitToRunFunction(Action action, float time=0)
        {
            yield return new WaitForSeconds(time);
            action();
        }

        public static IEnumerator WaitToRunFunction(Func<YieldInstruction> waitFunc, Action action)
        {
            yield return waitFunc();
            action();
        }
    }
}
