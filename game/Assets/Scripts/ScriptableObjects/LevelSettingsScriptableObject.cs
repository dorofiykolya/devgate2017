using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DevGate
{
    public class LevelSettingsScriptableObject : ScriptableObject
    {
        public SpawnComponent[] SpawnComponents;
        public BoatRocketComponent[] Rockets;
    }
}
