using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager_Room : MonoBehaviourPunCallbacks
{
    [Header("DisconnectPanel")] // 인스펙터에 오브젝트 항목별 헤더로 구분지음
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

    #region 방 리스트 갱신
    // 이전 목록 버튼 : -2,   다음 목록 버튼 : -1,    셀 숫자
    public void MyListClick(int num)
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else PhotonNetwork.JoinRoom(myList[multiple + num].Name);
        MyListRenewal();
    }
    #endregion

    #region 서버연결
    void Awake() => Screen.SetResolution(960, 540, false);

    void Update()
    {
        statusText.text = PhotonNetwork.NetworkClientState.ToString();
        lobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "로비 / " + PhotonNetwork.CountOfPlayers + "접속"; // 로비에 있는 인원 수와 서버에 접속한 인원 수 출력
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings(); // 서버에 연결

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby(); // 서버에 연결되면 콜백되는 함수, 연결 시 바로 로비로 이동함

    public override void OnJoinedLobby() // 로비에 접속 시 자동으로 콜백되는 함수
    {
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
        PhotonNetwork.LocalPlayer.NickName = nickNameInput.text; // 닉네임 인풋필드에 입력한 텍스트를 서버에 접속한 로컬 플레이어의 닉네임으로 지정
        welcomeText.text = PhotonNetwork.LocalPlayer.NickName + "님 환영합니다";
        myList.Clear();
    }

    public void Disconnect() => PhotonNetwork.Disconnect(); // 포톤 서버와 연결을 끊음

    public override void OnDisconnected(DisconnectCause cause) // 서버와 연결이 끊기면 자동으로 콜백되는 함수
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(false);
    }
    #endregion

    #region 방
    // 방 제목이 공백이라면 랜덤한 숫자를 섞어 방 제목으로 생성하고 방 제목이 입력된 상태라면 입력한 방 제목으로 방 생성, 방 옵션으로 최대 접속 인원 설정
    public void CreateRoom() => PhotonNetwork.CreateRoom(roomInput.text == "" ? "Room" + Random.Range(0, 100) : roomInput.text, new RoomOptions { MaxPlayers = 2 });

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom(); // 랜덤한 방으로 입장함

    public void LeaveRoom() => PhotonNetwork.LeaveRoom(); // 방 떠나기

    public override void OnJoinedRoom() // 방에 참가했을 때 자동으로 콜백되는 함수
    {
        roomPanel.SetActive(true);
        RoomRenewal(); // 방 갱신
        chatInput.text = ""; // 채팅 인풋필드 초기화
        for (int i = 0; i < chatText.Length; i++)
            chatText[i].text = ""; // 스크롤뷰 콘텐츠 안에 있는 채팅 텍스트 내용들도 반복문을 통해 전부 초기화
    }

    public override void OnCreateRoomFailed(short returnCode, string message) { roomInput.text = ""; CreateRoom(); } // 같은 이름의 방이 존재해 방 만들기가 실패할 경우 방이름 인풋필드 내용이 초기화되며 CreateRoom 메서드 내용과 같이 랜덤한 이름의 방이 생성됨

    public override void OnJoinRandomFailed(short returnCode, string message) { roomInput.text = ""; CreateRoom(); } // 랜덤한 방 입장에 실패할 경우 방 이름 인풋필드 내용 초기화, CreateRoom 메서드 내용과 같이 랜덤한 방이 생성

    public override void OnPlayerEnteredRoom(Player newPlayer) // 새로운 플레이어가 방에 접속했을 경우 방에 있는 전체 유저에게 호출되는 콜백 함수
    {
        RoomRenewal(); // 플레이어가 들어갔다 나갔다 할 때마다 방 갱신
        pv.RPC("ChatRPC", RpcTarget.All, "<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
        // pv.RPC(함수 이름, 타겟, 매개변수);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) // 방에 있던 다른 플레이어가 방을 나갔을 경우
    {
        RoomRenewal();
        ChatRPC("<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>"); // 문자열 안에 <color>태그를 사용해 텍스트에 색을 입힐 수 있음
    }

    void RoomRenewal() // 방 갱신
    {
        listText.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            listText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", "); // 플레이어 리스트와 해당 배열 길이가 일치하면 플레이어 이름 뒤에 쉼표 안붙임 (ex : 김성호1, 김성호)
        roomInfoText.text = PhotonNetwork.CurrentRoom.Name + " / " + PhotonNetwork.CurrentRoom.PlayerCount + "명 / " + PhotonNetwork.CurrentRoom.MaxPlayers + "최대"; // 방이름 / 방 접속자 / 최대 접속 가능자 수
    }
    #endregion

    #region 채팅
    public void Send() // 방에 접속한 인원 전체에게 닉네임 : 입력 내용의 채팅 전송 후 채팅 인풋필드 초기화
    {
        pv.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + chatInput.text);
        chatInput.text = "";
    }

    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다, RPC 함수는 로비가 아닌 방에 들어가 있을 경우에만 쓸 수 있다.
    void ChatRPC(string msg) // string 형식의 매개변수를 통해 pv.RPC 함수의 타겟 다음 매개변수 역할을 함
    {
        bool isInput = false;
        for (int i = 0; i < chatText.Length; i++) // 스크롤뷰 콘텐츠 밑의 채팅 오브젝트들 중 비어있는 오브젝트를 찾고 매개변수 msg를 넣어줌
            if (chatText[i].text == "")
            {
                isInput = true; // 매개변수가 성공적으로 텍스트 오브젝트를 채울 경우 true가 됨
                chatText[i].text = msg;
                break;
            }
        if(!isInput) // 채팅 화면이 꽉차면 한 칸씩 위로 올림, 가장 위에 있던 인덱스는 삭제됨
        {
            for (int i = 1; i < chatText.Length; i++)
                chatText[i - 1].text = chatText[i].text;
            chatText[chatText.Length - 1].text = msg;
        }
    }
    #endregion
}