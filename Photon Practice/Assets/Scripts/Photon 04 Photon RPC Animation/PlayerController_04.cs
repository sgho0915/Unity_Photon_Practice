using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerController_04 : MonoBehaviour, IPunObservable
{
    public PhotonView pv;
    public SpriteRenderer sr;
    public Text txt;

    float speed = 5f;

    void Update()
    {
        if (pv.IsMine) // Controll Locally�� true�̸�
        {
            // axis ���� 0 = ��������, -1 = ����, 1 = ������
            float axis = Input.GetAxisRaw("Horizontal");
            transform.Translate(new Vector3(axis * Time.deltaTime * 7, 0, 0));

            if (axis != 0) // �Է��� ������ walk true
            {
                pv.RPC("FlipXRPC", RpcTarget.AllBuffered, axis); // ������ ��� ����ڿ��� ���� ����ȭ�� ����
            }
        }
    }

    [PunRPC]
    void FlipXRPC(float axis)
    {
        sr.flipX = axis == -1;
    }

    [ContextMenu("���ϱ�")]
    public void Plus() => txt.text = (int.Parse(txt.text) + 1).ToString(); // ��Ŭ�� ���ϱ⸦ ������ �Ӹ� �� �ؽ�Ʈ�� ���ڰ� +1 ����
    // ���� ����ȭ�� ���� IPunObservable �߰�

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) stream.SendNext(txt.text); // �ؽ�Ʈ�� ������ ����
        else txt.text = (string)stream.ReceiveNext(); // �ؽ�Ʈ�� �������� ����
    }
}
