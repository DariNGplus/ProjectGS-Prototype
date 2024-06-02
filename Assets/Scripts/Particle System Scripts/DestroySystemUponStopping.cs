using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySystemUponStopping : MonoBehaviour
{
    public GameObject Parent;

    private void OnParticleSystemStopped()
    {
        Destroy(Parent);
    }
}
