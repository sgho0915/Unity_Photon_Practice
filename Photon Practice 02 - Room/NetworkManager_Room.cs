using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager_Room : MonoBehaviourPunCallbacks
{
    [Header("DisconnectPanel")] // �ν����Ϳ� ������Ʈ �׸� ����� ��������
    public InputField nickNameInput;

    [Header("LobbyPanel")]
    public GameObject lobbyPanel;
    public InputField roomInput;
    public Text welcomeText;
    public Text lobbyInfoText;
    public Button[] cellBtn;
    public Button prevBtn;
    public Button nextBtn;

    [Header("RoomPanel")]
    public GameObject roomPanel;
    public Text listText;
    public Text roomInfoText;
    public Text[] chatText;
    public InputField chatInput;

    [Header("ETC")]
    public Text statusText;
    public PhotonView pv;

    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;

    #region �� ����Ʈ ����
    // ���� ��� ��ư : -2,   ���� ��� ��ư : -1,    �� ����
    public void MyListClick(int num)
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else PhotonNetwork.JoinRoom(myList[multiple + num].Name);
        MyListRenewal();
    }
    #endregion

    #region ��������
    void Awake() => Screen.SetResolution(960, 540, false);

    void Update()
    {
        statusText.text = PhotonNetwork.NetworkClientState.ToString();
        lobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "�κ� / " + PhotonNetwork.CountOfPlayers + "����"; // �κ� �ִ� �ο� ���� ������ ������ �ο� �� ���
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings(); // ������ ����

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby(); // ������ ����Ǹ� �ݹ�Ǵ� �Լ�, ���� �� �ٷ� �κ�� �̵���

    public override void OnJoinedLobby() // �κ� ���� �� �ڵ����� �ݹ�Ǵ� �Լ�
    {
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
        PhotonNetwork.LocalPlayer.NickName = nickNameInput.text; // �г��� ��ǲ�ʵ忡 �Է��� �ؽ�Ʈ�� ������ ������ ���� �÷��̾��� �г������� ����
        welcomeText.text = PhotonNetwork.LocalPlayer.NickName + "�� ȯ���մϴ�";
        myList.Clear();
    }

    public void Disconnect() => PhotonNetwork.Disconnect(); // ���� ������ ������ ����

    public override void OnDisconnected(DisconnectCause cause) // ������ ������ ����� �ڵ����� �ݹ�Ǵ� �Լ�
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(false);
    }
    #endregion

    #region ��
    // �� ������ �����̶�� ������ ���ڸ� ���� �� �������� �����ϰ� �� ������ �Էµ� ���¶�� �Է��� �� �������� �� ����, �� �ɼ����� �ִ� ���� �ο� ����
    public void CreateRoom() => PhotonNetwork.CreateRoom(roomInput.text == "" ? "Room" + Random.Range(0, 100) : roomInput.text, new RoomOptions { MaxPlayers = 2 });

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom(); // ������ ������ ������

    public void LeaveRoom() => PhotonNetwork.LeaveRoom(); // �� ������

    public override void OnJoinedRoom() // �濡 �������� �� �ڵ����� �ݹ�Ǵ� �Լ�
    {
        roomPanel.SetActive(true);
        RoomRenewal(); // �� ����
        chatInput.text = ""; // ä�� ��ǲ�ʵ� �ʱ�ȭ
        for (int i = 0; i < chatText.Length; i++)
            chatText[i].text = ""; // ��ũ�Ѻ� ������ �ȿ� �ִ� ä�� �ؽ�Ʈ ����鵵 �ݺ����� ���� ���� �ʱ�ȭ
    }

    public override void OnCreateRoomFailed(short returnCode, string message) { roomInput.text = ""; CreateRoom(); } // ���� �̸��� ���� ������ �� ����Ⱑ ������ ��� ���̸� ��ǲ�ʵ� ������ �ʱ�ȭ�Ǹ� CreateRoom �޼��� ����� ���� ������ �̸��� ���� ������

    public override void OnJoinRandomFailed(short returnCode, string message) { roomInput.text = ""; CreateRoom(); } // ������ �� ���忡 ������ ��� �� �̸� ��ǲ�ʵ� ���� �ʱ�ȭ, CreateRoom �޼��� ����� ���� ������ ���� ����

    public override void OnPlayerEnteredRoom(Player newPlayer) // ���ο� �÷��̾ �濡 �������� ��� �濡 �ִ� ��ü �������� ȣ��Ǵ� �ݹ� �Լ�
    {
        RoomRenewal(); // �÷��̾ ���� ������ �� ������ �� ����
        pv.RPC("ChatRPC", RpcTarget.All, "<color=yellow>" + newPlayer.NickName + "���� �����ϼ̽��ϴ�</color>");
        // pv.RPC(�Լ� �̸�, Ÿ��, �Ű�����);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) // �濡 �ִ� �ٸ� �÷��̾ ���� ������ ���
    {
        RoomRenewal();
        ChatRPC("<color=yellow>" + otherPlayer.NickName + "���� �����ϼ̽��ϴ�</color>"); // ���ڿ� �ȿ� <color>�±׸� ����� �ؽ�Ʈ�� ���� ���� �� ����
    }

    void RoomRenewal() // �� ����
    {
        listText.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            listText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", "); // �÷��̾� ����Ʈ�� �ش� �迭 ���̰� ��ġ�ϸ� �÷��̾� �̸� �ڿ� ��ǥ �Ⱥ��� (ex : �輺ȣ1, �輺ȣ)
        roomInfoText.text = PhotonNetwork.CurrentRoom.Name + " / " + PhotonNetwork.CurrentRoom.PlayerCount + "�� / " + PhotonNetwork.CurrentRoom.MaxPlayers + "�ִ�"; // ���̸� / �� ������ / �ִ� ���� ������ ��
    }
    #endregion

    #region ä��
    public void Send() // �濡 ������ �ο� ��ü���� �г��� : �Է� ������ ä�� ���� �� ä�� ��ǲ�ʵ� �ʱ�ȭ
    {
        pv.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + chatInput.text);
        chatInput.text = "";
    }

    [PunRPC] // RPC�� �÷��̾ �����ִ� �� ��� �ο����� �����Ѵ�, RPC �Լ��� �κ� �ƴ� �濡 �� ���� ��쿡�� �� �� �ִ�.
    void ChatRPC(string msg) // string ������ �Ű������� ���� pv.RPC �Լ��� Ÿ�� ���� �Ű����� ������ ��
    {
        bool isInput = false;
        for (int i = 0; i < chatText.Length; i++) // ��ũ�Ѻ� ������ ���� ä�� ������Ʈ�� �� ����ִ� ������Ʈ�� ã�� �Ű����� msg�� �־���
            if (chatText[i].text == "")
            {
                isInput = true; // �Ű������� ���������� �ؽ�Ʈ ������Ʈ�� ä�� ��� true�� ��
                chatText[i].text = msg;
                break;
            }
        if(!isInput) // ä�� ȭ���� ������ �� ĭ�� ���� �ø�, ���� ���� �ִ� �ε����� ������
        {
            for (int i = 1; i < chatText.Length; i++)
                chatText[i - 1].text = chatText[i].text;
            chatText[chatText.Length - 1].text = msg;
        }
    }
    #endregion
}