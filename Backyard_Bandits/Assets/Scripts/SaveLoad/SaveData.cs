using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace SaveLoad
{
    [System.Serializable]
    public class SaveData
    {
        public CheckpointData checkpointData = new CheckpointData();
        
        [System.Serializable]
        public struct CheckpointData
        {
            public Vector3 lastCheckpoint;
            public List<GameObject> players;
        }
    }
}