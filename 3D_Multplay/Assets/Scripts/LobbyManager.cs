using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1.0.0";

    public TextMeshProUGUI connectionInfo;
    public Button joinButton;

    private void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        joinButton.interactable = false;
        connectionInfo.text = "Connect to Master ... ";
    }

    public override void OnConnectedToMaster()
    {
        //base.OnConnectedToMaster();
        joinButton.interactable = true;
        connectionInfo.text = "Online: Connect Master Complete";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //base.OnDisconnected(cause);
        joinButton.interactable = false;
        connectionInfo.text = "Offline: Connect Failed\nConnect to Master again ... ";

        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        joinButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            connectionInfo.text = "Connect to Room ... ";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connectionInfo.text = "Offline: Connect Failed\nConnect to Master again ... ";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //base.OnJoinRandomFailed(returnCode, message);
        connectionInfo.text = "There are no empty Room, Create new Room ... ";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });

    }

    public override void OnJoinedRoom()
    {
        //base.OnJoinedRoom();
        connectionInfo.text = "Connect Room Complete";
        PhotonNetwork.LoadLevel("PlayScene");
    }
}
