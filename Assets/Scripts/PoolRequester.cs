using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectNameTemplate.PoolingSystem
{
    public class PoolRequester : MonoBehaviour
    {
        [SerializeField] private GameObject keyNameByObject;
        [SerializeField] private string keyName;
        private float delay = 5;
        private float delayCount = 0;
        private void Update() 
        {
            if (delayCount >= delay)
            {
                
                GameObject newObj = null;
                
                if (keyNameByObject != null)
                {
                    keyName = (keyNameByObject.name);
                }

                newObj = PoolingManager.Instance.GetObjectFromPool(keyName);
                
                if (newObj != null)
                {
                    newObj!.gameObject.SetActive(true);
                    newObj!.transform.SetParent(this.transform);
                }
                else
                {
                    Debug.LogWarning($"{keyName} Null");
                }
               
                delayCount = 0;
            }
            else
            {
                delayCount += Time.fixedDeltaTime;
            }
        }
        
    }
}
