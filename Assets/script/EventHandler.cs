using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo.Players;
using System;



namespace SonicBloom.Koreo
{
    public class EventHandler : MonoBehaviour
    {

        public string TrackEventID_Cherry;
        public string TrackEventID_Gem;


        void Start()
        {

            Koreographer.Instance.RegisterForEvents(TrackEventID_Cherry, GenerateApple);
            Koreographer.Instance.RegisterForEvents(TrackEventID_Gem, GenerateGem);


        }

        //在特定时间点生成苹果
        private void GenerateApple(KoreographyEvent koreoEvent)
        {
            //Debug.Log("生成樱桃");
            GameObject obj = GameObject.Instantiate(Resources.Load("Item/Cherry")) as GameObject;
            obj.transform.position = new Vector3(8, -1.5f, 0);
        }

        //生成宝石
        private void GenerateGem(KoreographyEvent koreoEvent)
        {
            //Debug.Log("生成宝石");
            GameObject obj = GameObject.Instantiate(Resources.Load("Item/Gem")) as GameObject;
            obj.transform.position = new Vector3(8, 2.12f, 0);
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}

