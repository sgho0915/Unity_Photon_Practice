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
        if (pv.IsMine) // Controll Locally가 true이면
        {
            // axis 값은 0 = 정지상태, -1 = 왼쪽, 1 = 오른쪽
            float axis = Input.GetAxisRaw("Horizontal");
            transform.Translate(new Vector3(axis * Time.deltaTime * 7, 0, 0));

            if (axis != 0) // 입력이 있으면 walk true
            {
                pv.RPC("FlipXRPC", RpcTarget.AllBuffered, axis); // 접속한 모든 사용자에게 상태 동기화를 진행
            }
        }
    }

    [PunRPC]
    void FlipXRPC(float axis)
    {
        sr.flipX = axis == -1;
    }

    [ContextMenu("더하기")]
    public void Plus() => txt.text = (int.Parse(txt.text) + 1).ToString(); // 우클릭 더하기를 누르면 머리 위 텍스트의 숫자가 +1 증가
    // 변수 동기화를 위해 IPunObservable 추가

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) stream.SendNext(txt.text); // 텍스트를 서버에 보냄
        else txt.text = (string)stream.ReceiveNext(); // 텍스트를 서버에서 받음
    }
}
