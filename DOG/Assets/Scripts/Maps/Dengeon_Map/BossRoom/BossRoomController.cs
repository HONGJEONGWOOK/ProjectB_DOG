using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossRoomController : MonoBehaviour
{
    // 보스 방 입장 시
    // 1. 카메라 무빙
    // 2. UI 활성화
    // 3. 전투
    // 4. 보스 사망 시 포탈 열기
    // 5. 필드로 이동.

    Camera playerCam;
    BossTextController textController;
    Player_Hero player;

    [SerializeField] private Transform bossPosition;
    [SerializeField] private Transform playerPosition;
    private float playerCamZ;

    [Range(0.001f, 0.1f)]
    [SerializeField] private float cameraSpeed = 0.02f;

    public delegate void Show_UI_Delegate();
    public Show_UI_Delegate onBossEntry;
    public System.Action onReadyToFight;

    private void Awake()
    {
        player = FindObjectOfType<Player_Hero>();
        playerCam = player.transform.Find("Main Camera").GetComponent<Camera>();
        playerCamZ = playerCam.transform.position.z;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        GameObject obj =  MonsterManager.GetPooledMonster(MonsterManager.PooledMonster[MonsterManager.Inst.BossID]);
        obj.transform.position = bossPosition.position;
        obj.SetActive(true);

        player.transform.position = playerPosition.position;
        player.Actions.Disable();
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        StartCoroutine(FirstCamMove());
    }

    IEnumerator FirstCamMove()
    {
        yield return new WaitForSeconds(2.0f);
        while ((playerCam.transform.position - bossPosition.position).sqrMagnitude > playerCamZ * playerCamZ + 0.1f)
        {
            playerCam.transform.position = new Vector3(0,
                Mathf.Lerp(playerCam.transform.position.y, bossPosition.position.y, cameraSpeed), playerCamZ) ;

            yield return null;
        }
        // Boss UI 활성화
        onBossEntry.Invoke();      //UI 활성화
        textController = FindObjectOfType<Boss>().TextController;
        yield return new WaitForSeconds(textController.TypingTime + 2.0f);
        textController.gameObject.SetActive(false);
        StartCoroutine(CamBacktoPlayer());
    }

    IEnumerator CamBacktoPlayer()
    {
        while ((playerCam.transform.position - playerPosition.position).sqrMagnitude > playerCamZ * playerCamZ + 0.1f)
        {
            playerCam.transform.position = new Vector3(0,
                Mathf.Lerp(playerCam.transform.position.y, playerPosition.position.y, cameraSpeed), playerCamZ);
            yield return null;
        }
        // player + boss 움직임 활성화
        onReadyToFight?.Invoke();
        player.Actions.Enable();
    }
}
