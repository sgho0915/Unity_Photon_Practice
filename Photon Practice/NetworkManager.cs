using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks //MonoBehaviourPunCallbacks 클래스 상속
{
    public Text statusText;
    public InputField nickNameInput, roomNameInput;

    private void Awake() => Screen.SetResolution(960, 540, false); // 가로, 세로, 전체화면 유무
    // 메서드 => 람다 연산자를 통해 코드를 간결하게 유지 함.

    void Update() => statusText.text = PhotonNetwork.NetworkClientState.ToString(); // 네트워크 연결 상태를 문자열로 출력, 업데이트

    public void Connect() => PhotonNetwork.ConnectUsingSettings(); // 서버에 접속

    public override void OnConnectedToMaster() // 서버에 연결이 되면 콜백함수로 OnConnectedToMaster를 오버라이드 하여 호출
    {
        print("서버 접속 완료");
        PhotonNetwork.LocalPlayer.NickName = nickNameInput.text; // 현재 플레이어의 닉네임을 인풋필드에 적은 닉네임 텍스트로 설정
    }

    public void Disconnect() => PhotonNetwork.Disconnect(); // 서버와의 접속을 끊음
    public override void OnDisconnected(DisconnectCause cause) => print("연결 끊김"); // 서버와의 연결이 끊겼을 경우

    public void JoinLobby() => PhotonNetwork.JoinLobby(); // 로비에 접속, 대형 게임의 경우 로비를 여러개 사용하는 경우도 있음(ex : 크아)

    public override void OnJoinedLobby() => print("로비 접속 완료"); // 로비에 접속이 완료됐을 경우


    /// <summary>
    /// 이하 방 만들기 관련 메서드
    /// 아래의 메서드들이 실행되려면 PhotonNetwork.ConnectUsingSettings()를 통해 서버에 접속이 되어있거나
    /// PhotonNetwork.JoinLobby()를 통해 로비에 접속이 선행돼야함.
    /// </summary>
    public void CreateRoom() => PhotonNetwork.CreateRoom(roomNameInput.text, new RoomOptions { MaxPlayers = 2 }); // 방 만들기 : 방의 이름, 최대 입장 가능 인원 수 지정
    public void JoinRoom() => PhotonNetwork.JoinRoom(roomNameInput.text); // 방 참가 : 입력한 방의 이름으로 방 참가 시도
    public void JoinOrCreateRoom() => PhotonNetwork.JoinOrCreateRoom(roomNameInput.text, new RoomOptions { MaxPlayers = 2 }, null); // 입력한 이름으로 된 방이 없다면 그 이름으로 방을 생성, 방이 있다면 그 방에 참가
    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom(); // 랜덤한 방에 참가, 매칭 시스템에 사용 가능, 성공 및 실패 유무에 따라 OnJoinedRoom 또는 OnJoinRoomFailed 콜백 함수 호출
    public void LeaveRoom() => PhotonNetwork.LeaveRoom(); // 방 떠나기, PhotonNetwork.NetworkClientState는 다시 ConnectedToMaster가 됨
    public override void OnCreatedRoom() => print("방 만들기 완료"); // PhotonNetwork.CreateRoom을 통해 성공적으로 방이 생성되면 자동으로 콜백되는 함수
    public override void OnJoinedRoom() => print("방 참가 완료"); // PhotonNetwork.CreateRoom을 통해 성공적으로 방에 참가되면 자동으로 콜백되는 함수
    public override void OnCreateRoomFailed(short returnCode, string message) => print("방 만들기 싪패"); // 방 만들기에 실패했을 경우(ex : 방 이름 중복) 콜백되는 함수
    public override void OnJoinRoomFailed(short returnCode, string message) => print("방 참가 실패"); // 방 참가에 실패했을 경우(ex : 방 정원 초과) 콜백되는 함수

    /// <summary>
    /// 오브젝트에 속한 이 스크립트 컴포넌트를 우클릭했을 경우 뜨는 콘텍스트 메뉴
    /// </summary>
    [ContextMenu("정보")]
    void Info()
    {
        if (PhotonNetwork.InRoom) // 현재 방에 참가되어있는 경우
        {
            print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            print("현재 방 인원 수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("현재 방 최대 인원 수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) // 플레이어 리스트의 길이만큼 플레이어 리스트를 출력함
                playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            print(playerStr);
        }
        else // 서버, 로비에는 연결되어 있지만 현재 방에 참가되어있지 않은 경우
        {
            print("로비에 접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
            print("방 개수 : " + PhotonNetwork.CountOfRooms);
            print("모든 방에 있는 인원 수" + PhotonNetwork.CountOfPlayersInRooms);
            print("로비에 있는지 여부 : " + PhotonNetwork.InLobby);
            print("서버에 연결 됐는지 여부 : " + PhotonNetwork.IsConnected);
        }
    }
}
