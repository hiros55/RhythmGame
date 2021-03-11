using UnityEngine;
using System.IO;
using VRKeyboard.Utils;
using System.Collections;

public class ButtonCSVWriter : MonoBehaviour
{
    //UI全般キャンバス
    [SerializeField] private GameObject startCanvas;

    //ボタンが押されているか判定用オブジェクト
    [SerializeField] private GameObject pushBoolCube;
    //pushBoolCubeへの適用マテリアル
    [SerializeField] private Material greenTrue;
    [SerializeField] private Material redFalse;

    //スタート判定
    private bool startBool = false;

    //1ビートあたりの秒数
    private float secPerBeat;

    //1ビートあたりのサンプル数
    private float samplesPerBeat;

    private int bpm = 0;

    AudioSource audioSource;


    void Start()
    {
        audioSource = GameObject.Find("GameMusic").GetComponent<AudioSource>();
        StartCoroutine("AudioLoad");
    }

    //audioロード・audio情報取得
    IEnumerator AudioLoad()
    {
        //audioクリップが読み込まれるまでのコルーチン
        using (WWW www = new WWW("file://" + AudioButton.PickAudioPath))
        {
            yield return www;
            AudioClip audioclip = www.GetAudioClip(true, false);
            Debug.Log("audioclip.name"+audioclip.name);
            audioSource.clip = audioclip;
        }
        //bpmを取得(Download/UniBpmAnalyzer.cs)
        bpm = UniBpmAnalyzer.AnalyzeBpm(audioSource.clip);
        if (bpm < 0)
        {
            Debug.LogError("AudioClip is null.");
        }
        //1ビートあたりの秒数
        secPerBeat = 60 / bpm;
        //1ビートあたりのサンプル数 = ヘルツ単位のクリップのサンプル周波数 * 1ビートあたりの秒数
        samplesPerBeat = audioSource.clip.frequency * secPerBeat;
    }

    //音楽スタートに伴うUI制御
    public void AusioStart()
    {
        startBool = true;
        startCanvas.SetActive(false);
        audioSource.Play();
    }

    //取得したタイミングを引数に取り、csvファイルを生成して書き込む
    void NotesTime()
    {
        WriteCSV(GetTime() + ",");
    }

    //タイミングを取得
    float GetTime()
    {   //再生サンプル時間インデックス    ＝  現在の再生サンプル時間　/　クリップのサンプル周波数
        int timeSamplesIndex = audioSource.timeSamples / audioSource.clip.frequency;
        ///取得したタイミングより小さいサンプル数
        float minSample =Mathf.Abs(timeSamplesIndex * samplesPerBeat - audioSource.timeSamples);
        ///取得したタイミングより1ビート分大きいサンプル数
        float maxSample = Mathf.Abs((timeSamplesIndex + 1) * samplesPerBeat - audioSource.timeSamples);
        //取得したタイミング(再生サンプル時間)がどちらに近いか判断し、近い方の値を採用
        float currentTimig = Mathf.Min(minSample, maxSample);
        return currentTimig;
    }

    //csvファイルを生成
    void WriteCSV(string txt)
    {
        StreamWriter streamWriter;
        FileInfo fileInfo;
        if(Application.platform == RuntimePlatform.Android)
        {
            fileInfo = new FileInfo(Application.persistentDataPath + "/text/" + KeyboardManager.NotesFileName + ".csv");
        }
        else //デスクトップデバッグ用
        {
            fileInfo = new FileInfo(Application.dataPath + "/Resources/audio/" + KeyboardManager.NotesFileName + ".csv");
        }
        streamWriter = fileInfo.AppendText();
        streamWriter.WriteLine(txt);
        streamWriter.Flush();
        streamWriter.Close();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.H))
        {
            AusioStart();
        }

        //ボタンが押されているか判定
        if (OVRInput.Get(OVRInput.Button.One))
        {
            pushBoolCube.GetComponent<Renderer>().sharedMaterial = greenTrue;
        }
        else
        {
            pushBoolCube.GetComponent<Renderer>().sharedMaterial = redFalse;
        }


        if (startBool == true)
        {
            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                NotesTime();
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                NotesTime();
            }
        }
    }

}
