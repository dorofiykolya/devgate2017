﻿using System.Collections;
using DevGate;
using UnityEngine;
using Utils;


public class GameContext
{
    private static GameContext _instance;
    private Game _game;
    private Lifetime _lifetime;
    private GameSceneManager _sceneManager;
    private GameController _controller;

    public GameContext(Lifetime lifetime, Game game)
    {
        _instance = this;
        _lifetime = lifetime;
        _game = game;

        _sceneManager = new GameSceneManager();
        _controller = new GameController();
    }

    public static GameController Controller { get { return _instance._controller; } }
    public static Lifetime Lifetime { get { return _instance._lifetime; } }
    public static GameSceneManager SceneManager { get { return _instance._sceneManager; } }
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

    public static Lifetime.Definition StartCoroutine(IEnumerator enumerator)
    {
        var def = Lifetime.Define(_instance._lifetime);
        StartCoroutine(def.Lifetime, enumerator);
        return def;
    }
}
