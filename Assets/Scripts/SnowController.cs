using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class SnowController : MonoBehaviour
{
    ParticleSystem ps;
    ParticleSystem.MainModule mainModule;
    ParticleSystem.EmissionModule emissionModule;

    [Header("Modificabili in Play")]
    public float startSpeed = 0.5f;
    public float startSize = 0.2f;
    public float emissionRate = 20f;
    public float gravityModifier = 0.1f;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        mainModule = ps.main;
        emissionModule = ps.emission;
    }

    private void Start()
    {
       mainModule.simulationSpace = ParticleSystemSimulationSpace.World;
    }

    void Update()
    {
        // Modulo Main
        mainModule.startSpeed = startSpeed;
        mainModule.startSize = startSize;
        mainModule.gravityModifier = gravityModifier;

        // Modulo Emission
        emissionModule.rateOverTime = emissionRate;
    }
}