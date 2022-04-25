using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviour
{
    public PhotonView pv;
    public SpriteRenderer sr;
    float speed = 5f;

    void Update()
    {
        if (pv.IsMine) // Controll Locally가 true이면
        {
            float axis = Input.GetAxisRaw("Horizontal");
            transform.Translate(new Vector3(axis * Time.deltaTime * 7, 0, 0));

            if (axis != 0)
                pv.RPC("FlipXRPC", RpcTarget.AllBuffered, axis); // 접속한 모든 사용자에게 상태 동기화를 진행
        }
    }

    [PunRPC]
    void FlipXRPC(float axis)
    {
        sr.flipX = axis == -1;
    }
}
