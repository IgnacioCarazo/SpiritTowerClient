using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JarronesScript : MonoBehaviour
{
public void onHit(){
        StartCoroutine(destroyEn());
    }

    IEnumerator destroyEn(){
    yield return new WaitForSeconds(.1f);
       this.gameObject.SetActive(false);
    }
}
