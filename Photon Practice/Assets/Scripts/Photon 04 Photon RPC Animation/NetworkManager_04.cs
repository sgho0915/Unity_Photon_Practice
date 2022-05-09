using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager_04 : MonoBehaviourPunCallbacks
{
    // PhotonView ������Ʈ, RPC ����, Transform ����ȭ
    // 1. ����ȭ�ϰ��� �ϴ� ������Ʈ �ν����Ϳ� Photon View, Photon Transform View �߰�
    // 2. Photon View ������Ʈ�� Observed Components�� Photon Transform View ������Ʈ �巡�׾� ���

    // ������ ���� ���� ����� ������ Ŭ���̾�Ʈ�� �Ǳ� ������ Photon View�� Controll Locally�� true ���¿��� ����ȭ�� ��
    // ������ �÷��̾ 2�� �̻��� ��� �ٸ� ������� �����ؾ���
    // 1. ������Ʈ �ǿ� Resources ���� ����
    // 2. �÷��̾� ������Ʈ�� Resources ������ �巡�� �� ����� ���������� ����
    // 3. ���̾��Ű�� �ִ� ���� �÷��̾� ������Ʈ�� ��������
    // 4. �濡 �������� ���, �� OnJoinedRoom �Լ����� �÷��̾� �������� �������� (PhotonNetwork.Instantiate("Resources ������ �ִ� ������ ������ �̸�", Vector3.zero, Quaternion.identity);)
    // 5. �÷��̾� �̵� ��ũ��Ʈ �ۼ� �� ���� �� ��ũ��Ʈ�� �޷��ֱ� ������ Controll Locally�� true�̴� false�̴� �� �� ������
    // 6. �ش� ������ �ذ��ϱ� ���� �÷��̾� �̵� ��ũ��Ʈ�� public���� PhotonView pv�� ������
    // 7. �÷��̾� ������ �ν������� Photon View ��ũ��Ʈ�� �÷��̾� ��ũ��Ʈ�� ���� pv�� �巡�׾� �������

    // ������ Photon Transform View�� ���� Position, Rotation, Scale�� ����ȭ �ǰ� ������ ������ ����ȭ�� ���� ����
    // RPC �Լ��� ������ �ذ�����
    // 1. [PunRPC] �Լ��� ����� ����ȭ �� �ڵ带 �ۼ���
    // pv.rpc("�Լ� �̸�", RpcTarget.all, ����)�� ����� ä������

    // ������ �� ������ ���� ������ �������ϸ� flip ������ �ʱ�ȭ �Ǿ�����
    // pv.rpc �Լ����� RpcTarget.All�� �� ��� ȣ��Ǿ� ������� RpcTarget.AllBuffered�� �����ӵ� �� ȣ��ȴ�

    // �ִϸ��̼� ���� ����ȭ�� �ʿ��� ��
    // 1. Player �����տ� Photon Animator View ������Ʈ �߰�
    // 2. Synchronize Parameters�� walk �ɼ��� Discrete(���� ����)���� ����

    private void Awake()
    {
        Screen.SetResolution(960, 540, false);
        PhotonNetwork.ConnectUsingSettings(); // ������ ���� �õ�
    }

    // ������ ������ �Ǹ� �ٷ� ���� �����ϰ� ������
    public override void OnConnectedToMaster() => PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 6 }, null);

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Player_04", Vector3.zero, Quaternion.identity);
    }
}
