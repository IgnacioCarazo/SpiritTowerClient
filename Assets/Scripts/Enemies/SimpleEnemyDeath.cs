using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyDeath : MonoBehaviour
{
    public void onDeath(){
        StartCoroutine(destroyEn());
    }

    IEnumerator destroyEn(){
    yield return new WaitForSeconds(.3f);
       Destroy(gameObject);
    }
}
