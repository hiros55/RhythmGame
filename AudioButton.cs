using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using VRKeyboard.Utils;
using System.Text.RegularExpressions;

//音楽選択クラス
public class AudioButton : MonoBehaviour
{
    [SerializeField] private float buttonPositionOffset;
    //選択音楽パス保存用string
    public static string PickAudioPath = null;
   
    //音楽選択ボタン複製用
    [SerializeField] private GameObject buttonPrefab;
    
    [SerializeField] private GameObject keyboard;

    [SerializeField] private GameObject panelContent;

    //音楽選択ボタン複製代入用
    private List <GameObject> PathList;
    //音楽選択パス一覧配列
    private string [] PathArry;
    //音楽名表示用string
    private string audioName;

    KeyboardManager keyboardManager;


    void Start()
    {
        keyboardManager = keyboard.GetComponent<KeyboardManager>();

        if (Application.platform == RuntimePlatform.Android)
        {
            //mp3ファイルのパスをすべて配列として取得
            PathArry = Directory.GetFiles(Application.persistentDataPath + "/audio", "*.mp3");
            audioName = Application.persistentDataPath + "/audio";
        }
        else//デバッグ用
        {
            PathArry = Directory.GetFiles(Application.dataPath + "/Resources/audio", "*.mp3");
            audioName = Application.dataPath + "/Resources/audio";
        }

        Regex reg = new Regex(audioName);
        PathList =  new List<GameObject>();
        for (int i = 0; i < PathArry.Length; i++)
        {
            string buttonName = reg.Replace(PathArry[i], "");
            //ボタン複製&配列へ代入
            GameObject buttonInst = Instantiate(buttonPrefab);
            buttonInst.SetActive(true);
            PathList.Add(buttonInst);

            //panel->contentの子要素へ追加
            PathList[i].transform.SetParent(panelContent.transform, false);
            //音楽名表示テキスト編集
            PathList[i].transform.Find("Text").GetComponent<Text>().text = buttonName.Substring(1);

            //ボタンの位置編集
            Vector3 pos = PathList[i].transform.position;
            pos.y -= i + buttonPositionOffset;
            PathList[i].transform.position = pos;
            Button b = PathList[i].GetComponent<Button>();

            //それぞれのボタンへ特定のイベントを渡す
            this.AddEvent(b,i);
        }
    }
    //ラムダ式で匿名メソッドを作成しイベント設定の引数へ渡す
    void AddEvent(Button b, int i)
    {
        b.onClick.AddListener(() => { this.Event(i); });    
    }

    //ボタンが押されたら、ボタンの親要素を削除し、次のシーンへ移動
    public void Event(int i)
    {
        PickAudioPath = PathArry[i];
        Destroy(panelContent);
        keyboardManager.SceneButton.SetActive(true);
    }


    //デバッグ用
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            keyboardManager.Enter();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            keyboardManager.GenerateInput("l");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            keyboardManager.Backspace();
        }
    }

}
