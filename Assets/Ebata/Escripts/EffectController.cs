using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Disappear", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Disappear()
    {
        Destroy(gameObject);
    }
}
