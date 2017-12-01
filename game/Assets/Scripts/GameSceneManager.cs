using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace DevGate
{
    public class GameSceneManager
    {
        public Lifetime.Definition Load(Scenes sceneName)
        {
            var def = Lifetime.Define(GameContext.Lifetime);
            GameContext.StartCoroutine(def.Lifetime, LoadScene(sceneName));
            return def;
        }

        private IEnumerator LoadScene(Scenes sceneName)
        {
            var async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName.ToString());
            yield return async;
        }
    }
}