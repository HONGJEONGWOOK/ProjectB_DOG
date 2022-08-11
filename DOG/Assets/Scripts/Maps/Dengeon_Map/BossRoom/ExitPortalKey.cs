using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ExitPortalKey : MonoBehaviour
{
    ExitPortal portal;
    Camera playerCam;
    float playerCamZ;

    [Range(0.001f, 0.1f)]
    [SerializeField] float camSpeed = 0.05f;

    private void Awake()
    {
        portal = FindObjectOfType<ExitPortal>();
        playerCam = FindObjectOfType<Camera>();
        playerCamZ = playerCam.transform.position.z;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            portal.ShowPortal();
            GameManager.Inst.MainPlayer.Actions.Disable();
            StartCoroutine(CamMove_Portal());
            Destroy(this.gameObject);
        }
    }

    IEnumerator CamMove_Portal()
    {
        while ((portal.transform.position - playerCam.transform.position).sqrMagnitude > playerCamZ * playerCamZ + 0.1f)
        {
            playerCam.transform.position = Vector3.MoveTowards(transform.position, portal.transform.position + new Vector3(0, 0, playerCamZ), camSpeed);
            Debug.Log((portal.transform.position - playerCam.transform.position).sqrMagnitude);
            Debug.Log(playerCamZ * playerCamZ + 0.1f);
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
        while ((playerCam.transform.position - portal.transform.position).sqrMagnitude > playerCamZ * playerCamZ + 0.1f)
        {
            playerCam.transform.position = Vector3.Lerp(playerCam.transform.position, portal.transform.position + new Vector3(0, 0, playerCamZ), camSpeed) ;

            yield return null;
        }
        GameManager.Inst.MainPlayer.Actions.Enable();
        yield return null;
    }
}
