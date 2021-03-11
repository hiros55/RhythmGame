using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

public class FileChoice : MonoBehaviour
{
    //csvファイルのパス受け取り
    public　static string PickCsvPath = null;
    //csvファイルのパス一覧配列
    private string[] CsvFileArray;
    //csvファイル選択用ボタン配列
    private GameObject[] CsvButtonArray;

    //mp3ファイルのパス受け取り
    public static string PickAudioPath = null;
    //mp3ファイルファイルのパス一覧配列
    private string[] AudioFileArray;
    //mp3ファイル選択用ボタン配列
    private GameObject[] AudioButtonArray;

    //csvファイルが選択されたか
    private bool pickCsv = false;
    //音楽が選択されたか
    private bool pickAudio = false;

    private GameController gameController;

    private void Start()
    {
        CsvFileArray = new string[256];
        AudioFileArray = new string[256];

        gameController = GetComponent<GameController>();
        StartCoroutine("FileNameSet");
    }

    //csvファイル、音楽ファイルが選択されたか判定
    private void PanelChange()
    {
        if(PickCsvPath != null)
            pickCsv = true;

        if (PickAudioPath != null)
            pickAudio = true;
    }

    //音楽、csvファイル選択コルーチン&UI制御
    IEnumerator FileNameSet()
    {
        gameController.audioContent.SetActive(false);
        gameController.descriptiontext.text = "タイミング保存ファイル選択";
        Pick(CsvFileArray, CsvButtonArray, gameController.csvContent, "text", "*.csv");
        Pick(AudioFileArray, AudioButtonArray, gameController.audioContent, "audio", "*.mp3");
        yield return new WaitWhile(() => !pickCsv);
        gameController.csvContent.SetActive(false);
        gameController.audioContent.SetActive(true);
        gameController.descriptiontext.text = "音楽ファイル選択";
        yield return new WaitWhile(() => !pickAudio);
        gameController.descriptiontext.text = "";
        gameController.scrollView.SetActive(false);
    }

    #region PICK
    void Pick(string [] FileArray,GameObject [] ObjectArray ,GameObject panelContent,string Path,string Extension)
    {
        string RemoveName;
        //複製ボタン代入用配列
        ObjectArray = new GameObject[32];
        
        if (Application.platform == RuntimePlatform.Android)
        {
            //パス一覧取得&配列へ代入
            FileArray = Directory.GetFiles(Application.persistentDataPath + "/"+ Path, Extension);
            RemoveName = Application.persistentDataPath + "/"+ Path;
        }
        else//デバッグ用
        {
            FileArray = Directory.GetFiles(Application.dataPath + "/Resources/"+Path, Extension);
            RemoveName = Application.dataPath + "/Resources/"+Path;
        }
        //Regexクラスのインスタンスを生成
        Regex reg = new Regex(RemoveName);

        for (int i = 0; i < FileArray.Length; i++)
        {
            string ButtonNameString = reg.Replace(FileArray[i],"");

            //ボタンを複製し、panel->contentの子要素へ追加
            ObjectArray[i] = Instantiate(gameController.buttonPrefab);
            ObjectArray[i].SetActive(true);
            ObjectArray[i].transform.SetParent(panelContent.transform, false);
            //ボタン名編集
            ObjectArray[i].transform.Find("Text").GetComponent<Text>().text =  ButtonNameString.Substring(1);
            
            //ボタン位置編集
            Vector3 pos = ObjectArray[i].transform.position;
            pos.y =  -  i;
            ObjectArray[i].transform.position = pos;
            Button b = ObjectArray[i].GetComponent<Button>();
            this.AddButtonEvent(b, FileArray[i], Path);
        }
    }

    //ラムダ式で匿名メソッドを作成しイベント設定の引数へ渡す
    void AddButtonEvent(Button b,string FileString,string Path)
    {
        b.onClick.AddListener(() => {
            this.PickEvent( FileString, Path);
        });
    }

    //押されたボタンが持つパスを代入
    public void PickEvent( string FileString,string Path)
    {
        
        if(Path == "text")
        {
            PickCsvPath = FileString;
        }
        if (Path == "audio")
        {
            PickAudioPath = FileString;
        }
            
        PanelChange();
    }

    #endregion

}
