using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using Random = UnityEngine.Random;

public class directorScr : MonoBehaviour
{
    GameObject msgtxt;
    GameObject clickedObject;
    GameObject target, target2, target3, target4, target5;
    float start_time, diff_time, diff_distance;
    Vector2 start_positon, clickPoint, first_mouse_position, move_target;
    public int hit, miss, sum, target_point = 0;
    bool first_click = false;

    public string target1Tag = "Target1";
    public string target2Tag = "Target2";
    public string target3Tag = "Target3";
    public string target4Tag = "Target4";
    public string target5Tag = "Target5";



    void Start()
    {
        msgtxt = GameObject.Find("MsgTxt");
        msgtxt.GetComponent<Text>().text = "click to start";
        target = GameObject.Find("Target1");
        target2 = GameObject.Find("Target2");
        target3 = GameObject.Find("Target3");
        target4 = GameObject.Find("Target4");
        target5 = GameObject.Find("Target5");
    }

    void Update()
    {
        // 終了条件
        if (Input.GetKey(KeyCode.Escape)) Quit();
        if (hit == 30) Quit();

        //　データを集める処理
        if (Input.GetMouseButtonDown(0) && first_click)
        {
            // マウスのポジションを得る
            clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // SpriteのCollider2dとの接触を検知する
            // 指定した位置にオブジェクトがあれば、オブジェクトを取り出す
            // なければnullを返す
            Collider2D collision2d = Physics2D.OverlapPoint(clickPoint);
            if (collision2d)
            {
                diff_time = Time.time - start_time;
                start_time = Time.time;

                // クリックされた GameObject clickedObjectを取得
                clickedObject = collision2d.transform.gameObject;
                // 1回目のみ、開始時のマウスの位置からターゲットへの距離を計算
                if (sum == 0 || hit == 0)
                {
                    //diff_distance = Vector2.Distance(target.transform.position, first_mouse_position);
                    firstCalCol(collision2d);
                }
                else
                {
                    calCol(collision2d);
                }

                // targetの移動量を計算し、それぞれのtargetを動かす
                move_target = new Vector2(Random.Range(-7.0f, 7.0f), Random.Range(-4.0f, 4.0f));
                target.transform.position = move_target;
                target2.transform.position = move_target;
                target3.transform.position = move_target;
                target4.transform.position = move_target;
                target5.transform.position = move_target;

                Debug.Log("Distance:" + diff_distance.ToString());

                float scale_value = 1.0f;
                // float scale = Random.Range(-scale_value, scale_value);
                float scale = Random.Range(scale_value, scale_value);
                target.transform.localScale = new Vector2(scale, scale);

                DateTime now = DateTime.Now;
                string data = now.ToLongTimeString() + "," + hit.ToString() + "," + miss.ToString() + "," + diff_time.ToString() + "," + diff_distance.ToString() + "," + target_point.ToString();
                WritePointingData(data);

                hit++;
            }
            else
            {
                // その下にオブジェクトがない
                miss++;
            }

            string res = hit.ToString() + "hit" + " " + miss.ToString() + "miss" + " " + diff_time.ToString() + "s";
            msgtxt.GetComponent<Text>().text = res;

            // DateTime now = DateTime.Now;
            // string data = now.ToLongTimeString() + "," + hit.ToString() + "," + miss.ToString() + "," + diff_time.ToString() + "," + diff_distance.ToString();
            // WritePointingData(data);
            sum++;
        }

        if (Input.GetMouseButtonDown(0) && !first_click) First();
    }

    void WritePointingData(string txt)
    {
        using (StreamWriter stream_writer = new StreamWriter("./PointingLog.txt", true))
        {
            stream_writer.WriteLine(txt);
            stream_writer.Close();
        }
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        UnityEngine.Application.Quit();
#endif
    }

    void First()
    {
        // マウスのポジションを得る
        first_mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(first_mouse_position);
        move_target = new Vector2(Random.Range(-7.0f, 7.0f), Random.Range(-4.0f, 4.0f));
        target.transform.position = move_target;
        target2.transform.position = move_target;
        target3.transform.position = move_target;
        target4.transform.position = move_target;
        target5.transform.position = move_target;
        start_positon = target.transform.position;

        msgtxt.GetComponent<Text>().text = "START!!!";
        start_time = Time.time;
        first_click = true;

    }
    void calCol(Collider2D col)
    {
        if (col.gameObject.tag == "Target1")
        {
            diff_distance = Vector2.Distance(target.transform.position, start_positon);
            start_positon = target.transform.position;
            target_point = 1;
            Debug.Log("target1です");
        }
        if (col.gameObject.tag == "Target2")
        {
            diff_distance = Vector2.Distance(target2.transform.position, start_positon);
            start_positon = target2.transform.position;
            target_point = 2;
            Debug.Log("target2です");
        }
        if (col.gameObject.tag == "Target3")
        {
            diff_distance = Vector2.Distance(target3.transform.position, start_positon);
            start_positon = target3.transform.position;
            target_point = 3;
            Debug.Log("target3です");
        }
        if (col.gameObject.tag == "Target4")
        {
            diff_distance = Vector2.Distance(target4.transform.position, start_positon);
            start_positon = target4.transform.position;
            target_point = 4;
            Debug.Log("target4です");
        }
        if (col.gameObject.tag == "Target5")
        {
            diff_distance = Vector2.Distance(target5.transform.position, start_positon);
            start_positon = target5.transform.position;
            target_point = 5;
            Debug.Log("target5です");
        }
    }

    void firstCalCol(Collider2D col)
    {
        if (col.gameObject.tag == "Target1")
        {
            diff_distance = Vector2.Distance(target.transform.position, first_mouse_position);
            start_positon = target.transform.position;
            target_point = 1;
            Debug.Log("target1です");
        }
        if (col.gameObject.tag == "Target2")
        {
            diff_distance = Vector2.Distance(target2.transform.position, first_mouse_position);
            start_positon = target2.transform.position;
            target_point = 2;
            Debug.Log("target2です");
        }
        if (col.gameObject.tag == "Target3")
        {
            diff_distance = Vector2.Distance(target3.transform.position, first_mouse_position);
            start_positon = target3.transform.position;
            target_point = 3;
            Debug.Log("target3です");
        }
        if (col.gameObject.tag == "Target4")
        {
            diff_distance = Vector2.Distance(target4.transform.position, first_mouse_position);
            start_positon = target4.transform.position;
            target_point = 4;
            Debug.Log("target4です");
        }
        if (col.gameObject.tag == "Target5")
        {
            diff_distance = Vector2.Distance(target5.transform.position, first_mouse_position);
            start_positon = target5.transform.position;
            target_point = 5;
            Debug.Log("target5です");
        }
    }


}