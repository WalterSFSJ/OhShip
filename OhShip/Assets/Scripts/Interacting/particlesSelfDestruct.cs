using System.Collections;
using UnityEngine;

public class particlesSelfDestruct : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(SelfDestroy());
    }

    IEnumerator SelfDestroy() {

        yield return new WaitForSeconds(1.0f);

        Destroy(gameObject);
    }
}
