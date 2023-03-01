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
        //Ϊ�����������¼���Ӷ��Ĺ�ϵ��
        EventCenter.AddListener(EventType.jump,JumpUp);
        EventCenter.AddListener(EventType.down,FallDown);
    }

    // Update is called once per frame
    void FixedUpdate()//����ʵ��֡��ƽ���ƶ�Ч��
    {
        //Movement();
    }

    //Ҫ��Rigidbody���Constraints���湴ѡ����z��Ƕȣ���ֹ������ת
    //һֱ���һ������ʱ����ɫ���ٶȻ���0
    public void Movement()
    {

        float UpOrDown = Input.GetAxisRaw("Vertical");

        //��ɫ��Ծ,��up������ʱ
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
            AudioClip clip = Resources.Load<AudioClip>("SoundEffect/��ӣ��");
            AudioSource.PlayClipAtPoint(clip, new Vector2(0, 0), 5);//���ض�ʱ��㲥��
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
