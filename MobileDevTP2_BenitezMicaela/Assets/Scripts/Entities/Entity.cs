using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toolbox;

namespace Entities
{
    public class Entity : MonoBehaviour
    {
        public float timeToTurnOff = 0;

        protected Timer timer = new Timer();
        protected Rigidbody rb = null;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        protected virtual void OnEnable()
        {
            timer.SetTimer(timeToTurnOff, Timer.TIMER_MODE.DECREASE, true);
        }

        protected virtual void Update()
        {
            if (timer.Active) timer.UpdateTimer();
            if (timer.ReachedTimer()) gameObject.SetActive(false);
        }

        protected virtual void OnDisable()
        {
            if (rb) rb.Sleep();
        }
    }
}