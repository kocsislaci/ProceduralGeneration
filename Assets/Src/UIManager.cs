using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Error;
using UnityEngine;
using UnityEngine.UI;


enum UIState {
    Disconnected,
    Connected,
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private void SetSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private Button hostButton;
    [SerializeField] private Button serverButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private Button disconnectButton;

    [SerializeField] private TMP_InputField ipInput;

    [SerializeField] private TMP_Text winnerText;

    private UIState uiState;
    internal UIState UIState
    {
        get => uiState;
        set
        {
            switch (value)
            {
                case UIState.Disconnected:
                    hostButton.gameObject.SetActive(true);
                    serverButton.gameObject.SetActive(true);
                    clientButton.gameObject.SetActive(true);
                    disconnectButton.gameObject.SetActive(false);
                    ipInput.gameObject.SetActive(true);
                    break;
                case UIState.Connected:
                    hostButton.gameObject.SetActive(false);
                    serverButton.gameObject.SetActive(false);
                    clientButton.gameObject.SetActive(false);
                    disconnectButton.gameObject.SetActive(true);
                    ipInput.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
            uiState = value;
        }
    }

    void Awake()
    {
        SetSingleton();

        UIState = UIState.Disconnected;

        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            UIState = UIState.Connected;
        });
        serverButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
            UIState = UIState.Connected;
        });
        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            UIState = UIState.Connected;
        });
        disconnectButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            UIState = UIState.Disconnected;
        });
        ipInput.onValueChanged.AddListener((value) =>
        {
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            if (transport) {
                transport.SetConnectionData(value, 7777);
             }
        });
    }

    public void SetWinnerText(string winner)
    {
        winnerText.gameObject.SetActive(true);
        winnerText.text = winner + " has found the cheese!";
    }
}
