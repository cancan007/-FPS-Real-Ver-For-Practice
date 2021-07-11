using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSound : MonoBehaviour
{
    public AudioSource stage;
    public AudioClip stage1, stage2, stage3, stage4;
    public bool soundFlag = false;
    public bool flag = true;
    public float sec1, sec2, sec3, sec4 = 0;

    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(flag && Player.transform.position.z > 0)
        {
            MakeSound(stage1);
            flag = false;
            sec1 += Time.deltaTime;
        }
        else if (!flag && sec1 > 20)  //77
        {
            StopSound();
            sec1 = 0;
            flag = true;
        }

        if(flag && (Player.transform.position.z <= 0 && Player.transform.position.z > -45))
        {
            MakeSound(stage2);
            flag = false;
            sec2 += Time.deltaTime;
        }
        else if (!flag && sec2 > 20)  //64
        {
            StopSound();
            sec2 = 0;
            flag = true;
            
        }

        if (flag && Player.transform.position.z < -45)
        {
            MakeSound(stage3);
            flag = false;
            sec3 += Time.deltaTime;
        }
        else if (!flag && sec3 > 20)  //77
        {
            StopSound();
            sec3 = 0;
            flag = true;
        }

        //if (flag && (Player.transform.position.x > 20 || Player.transform.position.x < -20))
        //{
            //MakeSound(stage4);
           // flag = false;
            //seconds += Time.deltaTime;
        //}
        //else if (!flag && seconds > 44)
        //{
           // flag = true;
           // seconds = 0;
           // StopSound();
        //}

    }

    public void MakeSound(AudioClip clip)
    {
        stage.loop = true;

        stage.pitch = 1.0f;

        stage.clip = clip;

        stage.Play();

        soundFlag = true;
    }

    public void StopSound()
    {
        stage.Stop();  // 音の再生を止める

        stage.loop = false;

        stage.pitch = 1.0f;

        soundFlag = false;
    }
}
