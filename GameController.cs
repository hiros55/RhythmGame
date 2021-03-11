using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject buttonPrefab;
    //パネルの子要素
    public GameObject scrollView;
    //csv用のスクロールビューの子要素
    public GameObject csvContent;
    //音楽用のスクロールビューの子要素
    public GameObject audioContent;
    public Text descriptiontext;
    public int score;

    //ノーツとなるキューブをprefabから取得
    [SerializeField] private GameObject Cube;
    //ポインター
    [SerializeField] private GameObject OvrGazePointer;

    [SerializeField] private GameObject StartButton;
    
     //スコア用テキスト
    [SerializeField] private Text Scoretext;
    private AudioSource audioSource;

    private bool startBool = false;
    //タイミング用インデックス
    private int timingIndex = 0;
    private float[] timing;
    private CubeTranslate cubeTranslate;

    void Start()
    {
        timing = new float[4096];
        audioSource = GameObject.Find("GameMusic").GetComponent<AudioSource>();
        cubeTranslate = Cube.GetComponent<CubeTranslate>();
    }

    public void StartGame()
    {
        startBool = true;
        OvrGazePointer.SetActive(false);
        StartButton.SetActive(false);
        AudioLoad();
        CsvLoad();
    }

    //音楽ロード&再生
    void AudioLoad()
    {
        string path = FileChoice.PickAudioPath;
        WWW www = new WWW("file:///" + path);
        audioSource.clip = www.GetAudioClip(true,true);
        audioSource.Play();
    }


    void Update()
    {
        if (startBool)
        {
            CubeTiming();
        }
        Scoretext.text = "Score：" + score;

        if (Input.GetKeyDown(KeyCode.G))
        {
            StartGame();
        }
    }

    //キューブの生成
    //ToDo:CubeTranslateのvelocityパラメータを変化させ、Cubeの運動方法の種類を増やす
    //ToDo:Instantiateでは処理負荷がかかってしまうため、シーンの最初で一括生成する
    void CubeTiming()
    {
        {
            while (timing[timingIndex]  < GetTime() && timing[timingIndex] != 0)
            {
                int d = timingIndex % 5;

                switch (d)
                {
                    case 0:
                        Inst(Cube, 0.5f, 0,60);
                        break;
                    case 1:
                        Inst(Cube, -0.5f, 0, 60);
                        break;
                    case 2:
                        Inst(Cube, 0, 0.5f, 60);
                        break;
                    case 3:
                        Inst(Cube, 0, 0, 60);
                        break;
                    case 4:
                        Inst(Cube, 0, 1, 60);
                        break;
                }

                timingIndex++;
            }
        }
    }
    //Instantiateのラッパー関数
    GameObject Inst(GameObject cube,  float x, float y,float z)
    {
        return Instantiate(cube, new Vector3(x, y, z), Quaternion.identity);
    }


     public void CsvLoad()
    {
        string path = FileChoice.PickCsvPath;
        StreamReader streamReader = new StreamReader(path);

        while (!streamReader.EndOfStream)
        {
            string line = streamReader.ReadToEnd();
            string s = line;
            string str = s.TrimEnd(',');
            //カンマ区切りでタイミングの配列を生成する
            string[] values = str.Split(',');

            for (int i = 0; i < values.Length; i++)
            {
                float tmp = float.Parse(values[i]);
                //キューブ移動のインターバル分引いたタイミングを代入
                timing[i] = tmp - (cubeTranslate.interval* audioSource.clip.frequency);
            }
        }
        
    }

    //現在のタイミングを取得
    float GetTime()
    {
        return audioSource.timeSamples;
    }

}
