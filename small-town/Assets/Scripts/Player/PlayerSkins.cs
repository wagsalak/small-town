using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkins : MonoBehaviour
{
    [System.Serializable]
    public class Skins {

        public string skinName;
        public Sprite[] frontSprites;
        public Sprite[] backSprites;
        public Sprite[] sideSprites;

    }

    public Skins[] skins;

}
