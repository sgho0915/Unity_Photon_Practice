using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    static Managers Instance; // 유일성이 보장된다
    public static Managers GetInstance() { return Instance; } // 유일한 매니저를 갖고 온다

    // Start is called before the first frame update
    void Start()
    {
        // 초기화
        GameObject go = GameObject.Find("@Managers");
        Instance = go.GetComponent<Managers>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
