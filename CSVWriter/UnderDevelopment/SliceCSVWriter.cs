using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SliceCSVWriter : MonoBehaviour
{
    [SerializeField] private GameObject cube_2;
    [SerializeField] private GameObject Button;
    [SerializeField] private GameObject Text;//
    AudioSource audioSource;
    private float StartTime;
    public bool StartBool = true;
    private float t = 1;
    private VRKeyboard.Utils.KeyboardManager keyboardManager;
    

    void Start()
    {
        audioSource = GameObject.Find("GameMusic").GetComponent<AudioSource>();
        keyboardManager = GetComponent<VRKeyboard.Utils.KeyboardManager>();
    }

    public void AusioStart()
    {
        Button.SetActive(false);
        Text.SetActive(false);
        WWW www = new WWW("file://" + AudioButton.PickAudioPath);
        AudioClip audioclip = www.GetAudioClip(true, true);
        audioSource.clip = audioclip;
        audioSource.Play();
        StartTime = Time.time;
        StartBool = true;
    }

    void FixedUpdate()
    {
       /* if (bool_1 == true)
        {
            TimeSet();
        }*/

        if (t < 0.6f)
        {
            Instantiate(cube_2, new Vector3(0, 2, 6.9f), Quaternion.identity);
            //bool_1 = false;
            t = 1;
        }
    }
    

    public void Timing()
    {
        WriteCSV(ReturnTime() + "," );
    }

    public float ReturnTime()
    {
        return Time.time - StartTime;
    }

    public void WriteCSV(string text)
    {
        StreamWriter streamWriter;
        FileInfo fileInfo;

        if(Application.platform == RuntimePlatform.Android)
        fileInfo = new FileInfo(Application.persistentDataPath + "/text/" + VRKeyboard.Utils.KeyboardManager.NotesFileName + ".csv");
        else
        fileInfo = new FileInfo(Application.dataPath + "/" + "Resources/" + VRKeyboard.Utils.KeyboardManager.NotesFileName + ".csv");
        
        streamWriter = fileInfo.AppendText();
        streamWriter.WriteLine(text);
        streamWriter.Flush();
        streamWriter.Close();
    }

    private void TimeSet()
    {
        t -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

}

