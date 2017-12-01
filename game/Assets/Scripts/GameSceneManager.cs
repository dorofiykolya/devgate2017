using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace DevGate
{
    public class GameSceneManager
    {
        public Lifetime.Definition Load(Scenes sceneName)
        {
            var def = Lifetime.Define(GameContext.Lifetime);
            var async = SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Additive);
            async.completed += operation =>
            {
                var scene = SceneManager.GetSceneByName(sceneName.ToString());
                SceneManager.SetActiveScene(scene);
                def.Lifetime.AddAction(() =>
                {
                    foreach (var gameObject in scene.GetRootGameObjects())
                    {
                        GameObject.Destroy(gameObject);
                    }
                    SceneManager.UnloadSceneAsync(scene);
                });
            };
            return def;
        }
    }
}