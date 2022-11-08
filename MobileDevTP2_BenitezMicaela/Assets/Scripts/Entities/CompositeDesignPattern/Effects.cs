using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : Composite
{
    public ParticleSystem effect = null;

    public override void Play()
    {
        effect.Play();
    }
}
