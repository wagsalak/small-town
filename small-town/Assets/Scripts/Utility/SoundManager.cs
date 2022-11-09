using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager SoundManagerInstance { get; private set; }

    public AudioSource BGM;
    public AudioSource attack;
    public AudioSource click;

    private void Awake()
    {
        if (SoundManagerInstance == null) {

            SoundManagerInstance = this;

        } else {

            Destroy(this.gameObject);

        }
    }

    public void PlayBGM() {  }
    public void PlayAttack() { attack.Play(); }
    public void PlayClick() { click.Play(); }

}
