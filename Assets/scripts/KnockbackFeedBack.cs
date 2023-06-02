using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnockbackFeedBack : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb2d;

    [SerializeField]
    private float strength = 16f, delay = 0.15f;

    public UnityEvent OnBegin, OnDone;
    public void PlayFeedBack(GameObject sender)
    {
        StopAllCoroutines(); //turns off all other  coroutines that the boss  prefab  is  affected  by
        OnBegin?.Invoke();//invokes the OnBegin event settings (turns off boss script)
        Vector2 direction = (transform.position - sender.transform.position).normalized;
        rb2d.AddForce(direction * strength, ForceMode2D.Impulse);
        StartCoroutine(Reset());
    }
    private IEnumerator Reset()//delay before next invoke
    {
        yield return new WaitForSeconds(delay);
        rb2d.velocity = Vector3.zero;
        OnDone?.Invoke();//turns  boss  script back on
    }
}
