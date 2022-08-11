using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ExitPortalKey : MonoBehaviour
{
    ExitPortal portal;

    [SerializeField] float camSpeed = 3.0f;

    private void Awake()
    {
        portal = FindObjectOfType<ExitPortal>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            portal.ShowPortal();
            //StartCoroutine(CamMove_Portal());
            Destroy(this.gameObject);
        }
    }

    //IEnumerator CamMove_Portal()
    //{
    //    while ((portal.transform.position - Camera.main.transform.position).sqrMagnitude > 0.1f)
    //    {
    //        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, portal.transform.position, camSpeed);

    //        yield return null;
    //    }
    //    yield return new WaitForSeconds(1.0f);
    //    while ((portal.transform.position - Camera.main.transform.position).sqrMagnitude > 0.1f)
    //    {
    //        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, portal.transform.position, camSpeed);

    //        yield return null;
    //    }
    //}
}
