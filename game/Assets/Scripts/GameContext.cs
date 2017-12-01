using System.Collections;
using UnityEngine;
using Utils;


public class GameContext
{
    private static GameContext _instance;
    private Game _game;
    private Lifetime _lifetime;

    public GameContext(Lifetime lifetime, Game game)
    {
        _instance = this;
        _lifetime = lifetime;
        _game = game;
    }

    public static Transform RooTransform { get { return _instance._game.transform; } }

    public static void StartCoroutine(Lifetime lifetime, IEnumerator enumerator)
    {
        var coroutine = _instance._game.StartCoroutine(enumerator);
        if (coroutine != null)
        {
            Lifetime.Intersection(lifetime, _instance._lifetime).Lifetime.AddAction(() =>
            {
                _instance._game.StopCoroutine(coroutine);
            });
        }
    }
}

