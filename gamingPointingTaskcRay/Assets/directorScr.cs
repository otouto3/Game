using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using Random = UnityEngine.Random;

// # TODO
// マウスからマウスの距離にしないと行けなさそう

public class directorScr : MonoBehaviour
{
    GameObject msgtxt;
    GameObject clickedObject;
    GameObject target, target2, target3, target4, target5;
    float start_time, diff_time, diff_distance;
    Vector2 start_positon, clickPoint, first_mouse_position, move_target, pre_mouse_position;
    public int hit, miss, sum, target_point = 0;
    bool first_click = false;

    public string target1Tag = "Target1";
    public string target2Tag = "Target2";
    public string target3Tag = "Target3";
    public string target4Tag = "Target4";
    public string target5Tag = "Target5";

    public List<string> targetList = new List<string>();
    RaycastHit2D col;



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
        targetList = new List<string>();

        // 終了条件
        if (Input.GetKey(KeyCode.Escape)) Quit();
        if (hit == 30) Quit();

        //　データを集める処理
        if (Input.GetMouseButtonDown(0) && first_click)
        {
            // マウスのポジションを得る
            clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //メインカメラ上のマウスカーソルのある位置からRayを飛ばす
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //レイヤーマスク作成
            int layerMask = 1 << 8 | 1 << 9 | 1 << 10 | 1 << 11 | 1 << 12;
            //Rayの長さ
            float maxDistance = 30;
            RaycastHit2D col = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, maxDistance, layerMask);


            // // SpriteのCollider2dとの接触を検知する
            // // 指定した位置にオブジェクトがあれば、オブジェクトを取り出す
            // // なければnullを返す
            // Collider2D collision2d = Physics2D.OverlapPoint(clickPoint);
            if (col.collider)
            {
                diff_time = Time.time - start_time;
                start_time = Time.time;

                foreach (RaycastHit2D hit_target in Physics2D.RaycastAll(clickPoint, Vector2.zero))
                {
                    // name or tagなどを配列に格納して、targetが含まれているかみていく
                    //Debug.Log(hit_target.collider.gameObject.name);
                    targetList.Add(hit_target.collider.gameObject.name);
                }

                // クリックされた GameObject clickedObjectを取得
                // clickedObject = collision2d.transform.gameObject;
                // 1回目のみ、開始時のマウスの位置からターゲットへの距離を計算
                if (sum == 0 || hit == 0)
                {
                    //diff_distance = Vector2.Distance(target.transform.position, first_mouse_position);
                    firstCalCol(col.collider);
                }
                else
                {
                    //calCol(col.collider);
                    searchTarget(targetList);
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

    // targetList内を検索し、target_pointなどを設定する
    // target1から検索する 重なりが上から検索する
    void searchTarget(List<string> list)
    {
        if (sum == 0 || hit == 0) pre_mouse_position = first_mouse_position;
        if (list.Contains("Target1"))
        {
            //diff_distance = Vector2.Distance(target.transform.position, start_positon);
            diff_distance = Vector2.Distance(pre_mouse_position, start_positon);
            //start_positon = target.transform.position;
            pre_mouse_position = start_positon;
            target_point = 1;
            Debug.Log("target1です");
        }
        else if (list.Contains("Target2"))
        {
            diff_distance = Vector2.Distance(target2.transform.position, start_positon);
            start_positon = target2.transform.position;
            target_point = 2;
            Debug.Log("target2です");
        }
        else if (list.Contains("Target3"))
        {
            diff_distance = Vector2.Distance(target3.transform.position, start_positon);
            start_positon = target3.transform.position;
            target_point = 3;
            Debug.Log("target3です");
        }
        else if (list.Contains("Target4"))
        {
            diff_distance = Vector2.Distance(target4.transform.position, start_positon);
            start_positon = target4.transform.position;
            target_point = 4;
            Debug.Log("target4です");
        }
        else if (list.Contains("Target5"))
        {
            diff_distance = Vector2.Distance(target5.transform.position, start_positon);
            start_positon = target5.transform.position;
            target_point = 5;
            Debug.Log("target5です");
        }



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