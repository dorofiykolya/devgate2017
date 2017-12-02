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
        public GoatSpawnComponent GoatSpawnComponent;
        public ExplosionEffectComponent ExplosionEffectPrefab;
        public ExplosionEffectComponent SpawnEffectPrefab;
        public float MaxHorizontalAngle;
        public float ScreenDragCoeff;
        public float DelaySpawn = 2f;
        public float SpawnTime = 1f;
        public float SpawnSpeed = 1f;
        public float SpawnVelocity = 0.1f;
        public float SpawnObjectMaxY = 100f;
        public float BulletMinZ = 0;
        public float RocketSpeed = 20f;
        public float ShootDelay = 2f;
        public int ScoreForFirstLevel = 100;
        public Vector3[] GoatSpownPoints;
        public float GoatSpawnDelay = 30f;
        public float GoatMaxLiveTime = 10f;
        public float GoatSpawnSpeed = 10f;
        public float GoatMinY = -6f;
    }
}
