using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevGate
{
    public class GameController
    {
        public void Start()
        {
            GameContext.LevelController.LoadLevel(0);
        }
    }
}
