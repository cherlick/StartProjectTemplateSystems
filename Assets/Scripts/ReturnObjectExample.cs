using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectNameTemplate.PoolingSystem;

namespace ProjectNameTemplate.Examples
{
    public class ReturnObjectExample : MonoBehaviour
    {
        private float delayToReturn = 5;
        private void OnEnable() 
        {
            delayToReturn = 5;
        }

        private void Update() 
        {
            if (delayToReturn < 0)
            {
                PoolingManager.Instance.ReturnObject(gameObject, gameObject.name);
            }
            else
            {
                delayToReturn -= Time.deltaTime;
            }
        }
    }
}

