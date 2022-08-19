using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectNameTemplate.PoolingSystem
{
    [System.Serializable]
    public class PoolData
    {
        public GameObject prefab;
        public bool useNewKey = false;
        public string newKey;
        public int startSize = 1;
        public bool allowtoIncrement = false;

        [Tooltip("If true this does not allowed add the object again to the queue and be reuses, instead needs to be Returned manually to be used again")]
        public bool disableAutoEnqueue = false;
    }
}

