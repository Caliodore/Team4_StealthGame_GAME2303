using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class NetworkHUD : MonoBehaviour
{
    [SerializeField] GameObject HostButton;
    [SerializeField] GameObject JoinButton;
    [SerializeField] GameObject StartButton;
    [SerializeField] GameObject uiCanvas;
    private void Start()
    {
        Time.timeScale = 0f;
    }

    public void StartHost()
    {
       NetworkManager.Singleton.StartHost();
       JoinButton.SetActive(false);
       HostButton.SetActive(false);
       StartButton.SetActive(true);
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        HostButton.SetActive(false);
        JoinButton.SetActive(false);

    }

    public void StartBtnPress()
    {
        StartButton.SetActive(false);
        uiCanvas.SetActive(true);
        Time.timeScale = 1f;
    }
}
