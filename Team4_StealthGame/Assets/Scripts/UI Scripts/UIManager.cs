using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Banks
{
    BANKONE,
    BANKTWO
}

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject HostButton;
    [SerializeField] GameObject JoinButton;
    [SerializeField] GameObject Map1;
    [SerializeField] GameObject Map2;
    Banks bankChoice;

    private void Start()
    {
        
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
    public void Play()
    {
        HostButton.SetActive(false);
        JoinButton.SetActive(false);
        Map1.SetActive(true);
        Map2.SetActive(true);
    }

    /* PLEASE READ IMPORTANT! */

    // BankOne and BankTwo not working yet
    // Trying to figure out how to load the scenes/players through the network
    // havent found solution 
    // at least we got a basic UI now

    public void BankOne()
    {
        StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("Team4Stealth", LoadSceneMode.Single);
        
    }
    public void BankTwo()
    {

        StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("2ndBankAttempt", LoadSceneMode.Single);

    }
}
