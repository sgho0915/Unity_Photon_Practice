using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks //MonoBehaviourPunCallbacks Ŭ���� ���
{
    public Text statusText;
    public InputField nickNameInput, roomNameInput;

    private void Awake() => Screen.SetResolution(960, 540, false); // ����, ����, ��üȭ�� ����
    // �޼��� => ���� �����ڸ� ���� �ڵ带 �����ϰ� ���� ��.

    void Update() => statusText.text = PhotonNetwork.NetworkClientState.ToString(); // ��Ʈ��ũ ���� ���¸� ���ڿ��� ���, ������Ʈ

    public void Connect() => PhotonNetwork.ConnectUsingSettings(); // ������ ����

    public override void OnConnectedToMaster() // ������ ������ �Ǹ� �ݹ��Լ��� OnConnectedToMaster�� �������̵� �Ͽ� ȣ��
    {
        print("���� ���� �Ϸ�");
        PhotonNetwork.LocalPlayer.NickName = nickNameInput.text; // ���� �÷��̾��� �г����� ��ǲ�ʵ忡 ���� �г��� �ؽ�Ʈ�� ����
    }

    public void Disconnect() => PhotonNetwork.Disconnect(); // �������� ������ ����
    public override void OnDisconnected(DisconnectCause cause) => print("���� ����"); // �������� ������ ������ ���

    public void JoinLobby() => PhotonNetwork.JoinLobby(); // �κ� ����, ���� ������ ��� �κ� ������ ����ϴ� ��쵵 ����(ex : ũ��)

    public override void OnJoinedLobby() => print("�κ� ���� �Ϸ�"); // �κ� ������ �Ϸ���� ���


    /// <summary>
    /// ���� �� ����� ���� �޼���
    /// �Ʒ��� �޼������ ����Ƿ��� PhotonNetwork.ConnectUsingSettings()�� ���� ������ ������ �Ǿ��ְų�
    /// PhotonNetwork.JoinLobby()�� ���� �κ� ������ ����ž���.
    /// </summary>
    public void CreateRoom() => PhotonNetwork.CreateRoom(roomNameInput.text, new RoomOptions { MaxPlayers = 2 }); // �� ����� : ���� �̸�, �ִ� ���� ���� �ο� �� ����
    public void JoinRoom() => PhotonNetwork.JoinRoom(roomNameInput.text); // �� ���� : �Է��� ���� �̸����� �� ���� �õ�
    public void JoinOrCreateRoom() => PhotonNetwork.JoinOrCreateRoom(roomNameInput.text, new RoomOptions { MaxPlayers = 2 }, null); // �Է��� �̸����� �� ���� ���ٸ� �� �̸����� ���� ����, ���� �ִٸ� �� �濡 ����
    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom(); // ������ �濡 ����, ��Ī �ý��ۿ� ��� ����, ���� �� ���� ������ ���� OnJoinedRoom �Ǵ� OnJoinRoomFailed �ݹ� �Լ� ȣ��
    public void LeaveRoom() => PhotonNetwork.LeaveRoom(); // �� ������, PhotonNetwork.NetworkClientState�� �ٽ� ConnectedToMaster�� ��
    public override void OnCreatedRoom() => print("�� ����� �Ϸ�"); // PhotonNetwork.CreateRoom�� ���� ���������� ���� �����Ǹ� �ڵ����� �ݹ�Ǵ� �Լ�
    public override void OnJoinedRoom() => print("�� ���� �Ϸ�"); // PhotonNetwork.CreateRoom�� ���� ���������� �濡 �����Ǹ� �ڵ����� �ݹ�Ǵ� �Լ�
    public override void OnCreateRoomFailed(short returnCode, string message) => print("�� ����� ����"); // �� ����⿡ �������� ���(ex : �� �̸� �ߺ�) �ݹ�Ǵ� �Լ�
    public override void OnJoinRoomFailed(short returnCode, string message) => print("�� ���� ����"); // �� ������ �������� ���(ex : �� ���� �ʰ�) �ݹ�Ǵ� �Լ�

    /// <summary>
    /// ������Ʈ�� ���� �� ��ũ��Ʈ ������Ʈ�� ��Ŭ������ ��� �ߴ� ���ؽ�Ʈ �޴�
    /// </summary>
    [ContextMenu("����")]
    void Info()
    {
        if (PhotonNetwork.InRoom) // ���� �濡 �����Ǿ��ִ� ���
        {
            print("���� �� �̸� : " + PhotonNetwork.CurrentRoom.Name);
            print("���� �� �ο� �� : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("���� �� �ִ� �ο� �� : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "�濡 �ִ� �÷��̾� ��� : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) // �÷��̾� ����Ʈ�� ���̸�ŭ �÷��̾� ����Ʈ�� �����
                playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            print(playerStr);
        }
        else // ����, �κ񿡴� ����Ǿ� ������ ���� �濡 �����Ǿ����� ���� ���
        {
            print("�κ� ������ �ο� �� : " + PhotonNetwork.CountOfPlayers);
            print("�� ���� : " + PhotonNetwork.CountOfRooms);
            print("��� �濡 �ִ� �ο� ��" + PhotonNetwork.CountOfPlayersInRooms);
            print("�κ� �ִ��� ���� : " + PhotonNetwork.InLobby);
            print("������ ���� �ƴ��� ���� : " + PhotonNetwork.IsConnected);
        }
    }
}
