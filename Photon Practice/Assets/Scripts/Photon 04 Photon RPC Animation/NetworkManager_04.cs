using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager_04 : MonoBehaviourPunCallbacks
{
    // PhotonView 컴포넌트, RPC 개념, Transform 동기화
    // 1. 동기화하고자 하는 오브젝트 인스펙터에 Photon View, Photon Transform View 추가
    // 2. Photon View 컴포넌트의 Observed Components에 Photon Transform View 컴포넌트 드래그앤 드롭

    // 하지만 먼저 들어온 사람이 마스터 클라이언트가 되기 때문에 Photon View의 Controll Locally가 true 상태여야 동기화가 됨
    // 서버에 플레이어가 2명 이상일 경우 다른 방식으로 접근해야함
    // 1. 프로젝트 탭에 Resources 폴더 생성
    // 2. 플레이어 오브젝트를 Resources 폴더에 드래그 앤 드랍해 프리팹으로 만듬
    // 3. 하이어라키에 있는 원본 플레이어 오브젝트는 삭제해줌
    // 4. 방에 입장했을 경우, 즉 OnJoinedRoom 함수에서 플레이어 프리팹을 복사해줌 (PhotonNetwork.Instantiate("Resources 폴더에 있는 복사할 프리팹 이름", Vector3.zero, Quaternion.identity);)
    // 5. 플레이어 이동 스크립트 작성 후 실행 시 스크립트가 달려있기 때문에 Controll Locally가 true이던 false이던 둘 다 움직임
    // 6. 해당 문제를 해결하기 위해 플레이어 이동 스크립트에 public으로 PhotonView pv를 선언함
    // 7. 플레이어 프리팹 인스펙터의 Photon View 스크립트를 플레이어 스크립트에 생긴 pv에 드래그앤 드랍해줌

    // 하지만 Photon Transform View를 통해 Position, Rotation, Scale만 동기화 되고 나머지 값들은 동기화가 되지 않음
    // RPC 함수로 문제를 해결해줌
    // 1. [PunRPC] 함수를 사용해 동기화 할 코드를 작성함
    // pv.rpc("함수 이름", RpcTarget.all, 인자)를 사용해 채워넣음

    // 하지만 또 연결을 끊은 다음에 재접속하면 flip 방향이 초기화 되어있음
    // pv.rpc 함수에서 RpcTarget.All은 그 즉시 호출되어 사라지나 RpcTarget.AllBuffered는 재접속될 때 호출된다

    // 애니메이션 변수 동기화에 필요한 것
    // 1. Player 프리팹에 Photon Animator View 컴포넌트 추가
    // 2. Synchronize Parameters의 walk 옵션을 Discrete(변동 가능)으로 변경

    private void Awake()
    {
        Screen.SetResolution(960, 540, false);
        PhotonNetwork.ConnectUsingSettings(); // 서버에 연결 시도
    }

    // 서버와 연결이 되면 바로 방을 생성하고 입장함
    public override void OnConnectedToMaster() => PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 6 }, null);

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Player_04", Vector3.zero, Quaternion.identity);
    }
}
