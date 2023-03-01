using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControler : MonoBehaviour
{
    public static playerControler instance;

    public Rigidbody2D foxRb;
    public GameObject playerRun;
    public GameObject playerFly;
    public GameObject AudioPlayer;


    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        //为上跳，下落事件添加订阅关系。
        EventCenter.AddListener(EventType.jump,JumpUp);
        EventCenter.AddListener(EventType.down,FallDown);
    }

    // Update is called once per frame
    void FixedUpdate()//根据实际帧数平滑移动效果
    {
        //Movement();
    }

    //要在Rigidbody里的Constraints里面勾选冻结z轴角度，防止物体旋转
    //一直点击一个方向时，角色的速度会变成0
    public void Movement()
    {

        float UpOrDown = Input.GetAxisRaw("Vertical");

        //角色跳跃,当up被按下时
        switch (UpOrDown)
        {
            case 1f:
                playerRun.SetActive(false);
                playerFly.SetActive(true);
                AudioPlayer.GetComponent<AudioSource>().volume = 1.0f;
                break;
            case -1f:
                playerFly.SetActive(false);
                playerRun.SetActive(true);
                AudioPlayer.GetComponent<AudioSource>().volume = 0.5f;
                break;
            default:
                break;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection")
        {
            Destroy(collision.gameObject);
            AudioClip clip = Resources.Load<AudioClip>("SoundEffect/捡到樱桃");
            AudioSource.PlayClipAtPoint(clip, new Vector2(0, 0), 5);//在特定时间点播放
        }
    }

    public void JumpUp()
    {
        playerRun.SetActive(false);
        playerFly.SetActive(true);
    }

    public void FallDown()
    {
        playerFly.SetActive(false);
        playerRun.SetActive(true);
    }
}
