using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaChanger : MonoBehaviour
{
    FieldToVillage toVillage;
    VillageToField toField;

    Collider2D toVillageColl;
    Collider2D toFieldColl;

    CurrentMapIndicator mapIndicator;

    private void Awake()
    {
        toVillage = transform.GetChild(0).GetComponent<FieldToVillage>();
        toField = transform.GetChild(1).GetComponent<VillageToField>();

        toVillageColl = toVillage.GetComponent<Collider2D>();
        toFieldColl = toField.GetComponent<Collider2D>();

        mapIndicator = GetComponentInChildren<CurrentMapIndicator>();

        toVillage.OnVillageEnter = ActivateVillage;
        toField.OnFieldEnter = ActivateField;

        toField.enabled = false;
    }

    void ActivateVillage()
    {
        toFieldColl.enabled = true;
        toVillageColl.enabled = false;

        SoundManager.Inst.ChangeBGM(SoundID.BGM_Village);

        mapIndicator.text.text = "마  을";
        mapIndicator.text.color = Color.white;
        StartCoroutine(mapIndicator.ShowMapName());
    }

    void ActivateField()
    {
        toFieldColl.enabled = false;
        toVillageColl.enabled = true;

        SoundManager.Inst.ChangeBGM(SoundID.BGM_Field);

        mapIndicator.text.text = "몬스터 필드";
        mapIndicator.text.color = Color.white;
        StartCoroutine(mapIndicator.ShowMapName());
    }
}
