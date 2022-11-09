using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsManager : MonoBehaviour
{

    PlayerSkins skins;

    public SpriteRenderer[] characterBodyPartsFront;
    public SpriteRenderer[] characterBodyPartsBack;
    public SpriteRenderer[] characterBodyPartsSide;

    void Start() {

        skins = GetComponent<PlayerSkins>();
        ChangeSkin( UtilityManager.UtilityInstance.EquipedSkin() );

    }

    public void ChangeSkin( string skinName ) {

        print("<color=yelow>Wear:" + skinName + "</color>");

        for (int i = 0; i <= skins.skins.Length - 1; i++)  {

            if (skins.skins[i].skinName == skinName) {

                WearIndividual(characterBodyPartsFront.Length, characterBodyPartsFront, skins.skins[i].frontSprites);
                WearIndividual(characterBodyPartsBack.Length, characterBodyPartsBack, skins.skins[i].backSprites);
                WearIndividual(characterBodyPartsSide.Length, characterBodyPartsSide, skins.skins[i].sideSprites);

            }

        }

    }

    public void WearIndividual(int arrLength, SpriteRenderer[] bodyParts, Sprite[] skinParts ) {

        for (int i = 0; i <= arrLength - 1; i++) {

            bodyParts[i].sprite = skinParts[i];

        }

    }
}
