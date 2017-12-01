using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DevGate
{
    public class GameStarterComponent : MonoBehaviour
    {
        private void Awake()
        {
#if UNITY_EDITOR
            if (!GameContext.Initialized)
            {
                foreach (var component in GetComponents<MonoBehaviour>())
                {
                    if (component != this && !(component is Transform))
                        DestroyImmediate(component);
                }
                SceneManager.LoadScene(Scenes.Main.ToString());
            }
#endif
        }
    }
}
