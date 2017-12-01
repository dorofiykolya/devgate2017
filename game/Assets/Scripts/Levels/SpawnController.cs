using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevGate
{
    public class SpawnController
    {
        private readonly LevelComponent _levelComponent;
        private readonly LevelSettingsScriptableObject _settings;

        public SpawnController(LevelComponent levelComponent, LevelSettingsScriptableObject settings)
        {
            _levelComponent = levelComponent;
            _settings = settings;
        }

        public void Add(SpawnComponent component)
        {
            
        }

        public void Init()
        {
            // find points
        }
    }
}
