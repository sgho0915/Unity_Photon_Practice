using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    static Managers Instance; // ���ϼ��� ����ȴ�
    public static Managers GetInstance() { return Instance; } // ������ �Ŵ����� ���� �´�

    // Start is called before the first frame update
    void Start()
    {
        // �ʱ�ȭ
        GameObject go = GameObject.Find("@Managers");
        Instance = go.GetComponent<Managers>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
