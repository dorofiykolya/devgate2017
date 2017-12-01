using Utils;

namespace DevGate
{
    public class GameLevelController
    {
        public LevelComponent Current;
        public Lifetime LeveLifetime;

        public Lifetime.Definition LoadLevel(Scenes levelId)
        {
            var def = GameContext.SceneManager.Load(Scenes.Level_01, component =>
            {
                Current = component;
            });
            LeveLifetime = def.Lifetime;
            def.Lifetime.AddAction(() =>
            {
                Current = null;
            });
            return def;
        }
    }
}
