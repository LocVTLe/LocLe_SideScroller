using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[RequireComponent(typeof(ParticleSystem))]
public class EmitParticlesOnLand : MonoBehaviour
{

    public bool emitOnLand = true;
    public bool emitOnEnemyHit = true;
    public bool emitOnEnemyDeath = true;

#if UNITY_TEMPLATE_PLATFORMER

    ParticleSystem p;

    void Start()
    {
        p = GetComponent<ParticleSystem>();

        if (emitOnLand) {
            Platformer.Gameplay.PlayerLanded.OnExecute += PlayerLanded_OnExecute;
            void PlayerLanded_OnExecute(Platformer.Gameplay.PlayerLanded obj) {
                p.Play();
            }
        }

        if (emitOnEnemyHit)
        {
            Platformer.Gameplay.EnemyHit.OnExecute += EnemyHit_OnExecute;
            void EnemyHit_OnExecute(Platformer.Gameplay.EnemyHit obj)
            {
                p.Play();
            }
        }

        if (emitOnEnemyDeath) {
            Platformer.Gameplay.EnemyDeath.OnExecute += EnemyDeath_OnExecute;
            void EnemyDeath_OnExecute(Platformer.Gameplay.EnemyDeath obj) {
                p.Play();
            }
        }

    }

#endif

}
