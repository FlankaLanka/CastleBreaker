using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueExplosion : Warhead {

    protected override void warheadStart(){

    }

    void Start()
    {
        var main = GetComponent<ParticleSystem>().main;
        main.stopAction = ParticleSystemStopAction.Callback;
        warheadStart();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleSystemStopped() {
        recycleWarhead();
    }

}
