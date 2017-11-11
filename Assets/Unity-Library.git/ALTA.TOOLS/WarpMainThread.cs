using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alta.Tools
{
    public class WarpMainThread : MonoBehaviour
    {
        private Queue<Action> listAction;

        public void Update()
        {
            if (listAction != null)
            {
                if (listAction.Count > 0)
                {
                    listAction.Dequeue()();
                }
            }
        }

        public void Run(Action act)
        {
            if (this.listAction == null)
                this.listAction = new Queue<Action>();
            this.listAction.Enqueue(act);
        }
    }

}
