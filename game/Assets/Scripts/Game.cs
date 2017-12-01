using UnityEngine;
using Utils;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }
    private Lifetime.Definition _lifetime;
    private GameContext _gameContext;
    private Signal _onUpdate;
    private Signal _onLateUpdate;
    private Signal _onFixedUpdate;
    private Signal _onQuit;
    private Signal<bool> _onFocus;
    private Signal<bool> _onPause;

    public bool Enabled { get; private set; }

    public ISignalSubsribe OnUpdate { get { return _onUpdate; } }
    public ISignalSubsribe OnLateUpdate { get { return _onLateUpdate; } }
    public ISignalSubsribe OnFixedUpdate { get { return _onFixedUpdate; } }
    public ISignalSubsribe OnQuit { get { return _onQuit; } }
    public ISignalSubsribe<bool> OnFocus { get { return _onFocus; } }
    public ISignalSubsribe<bool> OnPause { get { return _onPause; } }

    public void Restart()
    {
        _lifetime.Terminate();
        Initialize();
    }

    private void Initialize()
    {
        _lifetime = Lifetime.Define(Lifetime.Eternal);

        _onUpdate = new Signal(_lifetime.Lifetime);
        _onLateUpdate = new Signal(_lifetime.Lifetime);
        _onFixedUpdate = new Signal(_lifetime.Lifetime);
        _onQuit = new Signal(_lifetime.Lifetime);
        _onFocus = new Signal<bool>(_lifetime.Lifetime);
        _onPause = new Signal<bool>(_lifetime.Lifetime);

        _gameContext = new GameContext(_lifetime.Lifetime, this);
        GameContext.Controller.Start();
        DontDestroyOnLoad(gameObject);
    }

    private void Awake()
    {
        Instance = this;
        Initialize();
    }

    private void OnEnable()
    {
        Enabled = true;
    }

    private void OnDisable()
    {
        Enabled = false;
    }

    private void Update()
    {
        _onUpdate.Fire();
    }

    private void LateUpdate()
    {
        _onLateUpdate.Fire();
    }

    private void FixedUpdate()
    {
        _onFixedUpdate.Fire();
    }

    private void OnApplicationFocus(bool focus)
    {
        _onFocus.Fire(focus);
    }

    private void OnApplicationPause(bool pause)
    {
        _onPause.Fire(pause);
    }

    private void OnApplicationQuit()
    {
        _onQuit.Fire();
    }

    private void OnDestroy()
    {
        _lifetime.Terminate();
    }
}
