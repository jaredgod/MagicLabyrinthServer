using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;

public enum ServerToClientId : ushort
{
    // game state syncs
    gamestateWaiting = 1,
    gamestateExplore,
    gamestateEscape,
    gamestateEnded,
    syncClock,
    playerConnected,
    playerDisconnected,
    playerActions,

    // game actions
    roomPlaced,
    characterSpawned,
    characterMoved,
    gateRemoved,
    tileChanged,
    timerFlipped,
    pinged,
}

public enum ClientToServerId : ushort
{
    // game state syncs
    name = 1,
    startGame,

    // game actions
    moveCharacter,
    explore,
    teleport,
    vent,
    flipTimer,
    ping
}


public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;
    public static NetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    public Server Server { get; private set; }
    public ushort CurrentTick { get; private set; } = 0;

    [SerializeField] private ushort port;
    [SerializeField] private ushort maxClientCount;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        Application.runInBackground = true;

#if UNITY_EDITOR
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
#else
        System.Console.Title = "Server";
        System.Console.Clear();
        Application.SetStackTraceLogType(UnityEngine.LogType.Log, StackTraceLogType.None);
        RiptideLogger.Initialize(Debug.Log, true);
#endif

        Server = new Server();
        Server.ClientConnected += NewPlayerConnected;
        Server.ClientDisconnected += PlayerLeft;

        Server.Start(port, maxClientCount);

        GameController.RenderWaitingScene();
    }

    private void FixedUpdate() { 
        Server.Tick();
        if (GameController.currentScene != GameScene.explore && GameController.currentScene != GameScene.escape) return;

        GameController.fixedUpdate();
        if (CurrentTick % (60 * 10) == 0) GameController.SendSync();

        CurrentTick++;
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
    {
        PlayerController.PlayerDisconnected(e.Id);
    }


    
    private void NewPlayerConnected(object sender, ServerClientConnectedEventArgs e)
    {
        PlayerController.PlayerConnected(e.Client.Id);
    }

}
