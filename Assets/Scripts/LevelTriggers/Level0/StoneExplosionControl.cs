using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneExplosionControl : MonoBehaviour
{
    private void OnParticleSystemStopped() {
        Destroy(gameObject);
    }
}
