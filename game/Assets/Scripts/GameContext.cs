using System;
using System.Collections;
using DevGate;
using UnityEngine;
using Utils;


public class GameContext
{
    private static GameContext _instance;
    private Game _game;
    private Lifetime _lifetime;
    private GameSceneManager _sceneManager;
    private GameController _gameController;
    private GameLevelController _levelController;

    public GameContext(Lifetime lifetime, Game game)
    {
        _instance = this;
        _lifetime = lifetime;
        _game = game;

        _sceneManager = new GameSceneManager();
        _gameController = new GameController();
        _levelController = new GameLevelController();
    }

    public static bool Initialized { get { return _instance != null; } }

    public static GameLevelController LevelController { get { return _instance._levelController; } }
    public static GameController GameController { get { return _instance._gameController; } }
    public static Lifetime Lifetime { get { return _instance._lifetime; } }
    public static GameSceneManager SceneManager { get { return _instance._sceneManager; } }
    public static Transform RooTransform { get { return _instance != null ? _instance._game.transform : null; } }

    public static void SubscribeOnUpdate(Lifetime lifetime, Action listener)
    {
        _instance._game.OnUpdate.Subscribe(lifetime, listener);
    }

    public static Lifetime.Definition SubscribeOnUpdate(Action listener)
    {
        var def = Lifetime.Define(Lifetime);
        SubscribeOnUpdate(def.Lifetime, listener);
        return def;
    }

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

    public static void DelayCall(Lifetime lifetime, float seconds, Action listener)
    {
        StartCoroutine(lifetime, DelayCallInternal(seconds, listener));
    }

    public static Lifetime.Definition DelayCall(float seconds, Action listener)
    {
        var def = Lifetime.Define(Lifetime.Eternal);
        DelayCall(def.Lifetime, seconds, listener);
        return def;
    }

    public static Lifetime.Definition StartCoroutine(IEnumerator enumerator)
    {
        var def = Lifetime.Define(_instance._lifetime);
        StartCoroutine(def.Lifetime, enumerator);
        return def;
    }

    private static IEnumerator DelayCallInternal(float seconds, Action listener)
    {
        yield return new WaitForSeconds(seconds);
        listener();
    }
}

