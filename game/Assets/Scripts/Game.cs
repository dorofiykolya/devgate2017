using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Game : MonoBehaviour
{
    private Lifetime.Definition _definition;
    private Signal<float> _onUpdate;
    private Game _instance;

    private void Awake()
    {
        _definition = Lifetime.Define(Lifetime.Eternal, "Game");
        _onUpdate = new Signal<float>(_definition.Lifetime);
        _instance = this;
    }

    private void OnDestroy()
    {
        _definition.Terminate();
    }

    private void Update()
    {
        _onUpdate.Fire(Time.deltaTime);
    }

    public void SubscribeOnUpdate(Lifetime lifetime, Action<float> listener)
    {
        _onUpdate.Subscribe(lifetime, listener);
    }

    public Lifetime.Definition SubscribeUpdate(Action<float> listener)
    {
        var lifetime = Lifetime.Define(_instance._definition.Lifetime);
        _instance.SubscribeOnUpdate(lifetime.Lifetime, listener);
        return lifetime;
    }
}
