using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireControl : MonoBehaviour
{
    public List<ParticleSystem> particleSystems;

    // Update is called once per frame
    void Update()
    {
        foreach (ParticleSystem ps in particleSystems)
        {
            if(ps != null){
                ps.Simulate(Time.unscaledDeltaTime, true, false);
            }
        }
    }
}
