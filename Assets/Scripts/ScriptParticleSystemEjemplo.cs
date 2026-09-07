using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptParticleSystemEjemplo : MonoBehaviour
{
    #region Variables
    //Sistemi particellari e moduli
    ParticleSystem pSystemRain, pSystemRipples, pSystemSplash;
    ParticleSystem.MainModule pSMMLluvia, pSMMOndas;
    ParticleSystem.EmissionModule pSEMLluvia, pSEMOndas;
    ParticleSystem.ShapeModule pSSMLluvia, pSSMOndas;

    public bool active;
    public float maxParticles;

    float _i;
    public float defaultInt;
    public float rainIntensity //Controllo dell'intensitÃ .
    {
        get { return _i; }
        set
        {
            if (value < 0)
            {
                _i = 0;
            }
            else if (value >= maxParticles)
            {
                _i = maxParticles;
            }
            else
            {
                _i = value;
            }
        }
    }

    public float iVel;
    public Vector3 size, pos;

    #endregion
    #region Monobehaviour
    void Awake()
    {
        //Assegnamento degli oggetti.
        pSystemRain = transform.GetChild(0).GetComponent<ParticleSystem>();
        pSystemRipples = transform.GetChild(1).GetComponent<ParticleSystem>();
        pSystemSplash = pSystemRipples.transform.GetChild(0).GetComponent<ParticleSystem>();

        //Assegnamento dei monuli.
        pSMMLluvia = pSystemRain.main;
        pSMMOndas = pSystemRipples.main;

        pSEMLluvia = pSystemRain.emission;
        pSEMOndas = pSystemRipples.emission;

        pSSMLluvia = pSystemRain.shape;
        pSSMOndas = pSystemRipples.shape;

        //Play o no.
        pSMMLluvia.playOnAwake = active;
        pSMMOndas.playOnAwake = active;
    }
    void Start()
    {
        OnOff(active);
        SetIntensidad(0f, iVel);
    }

    void Update()
    {
        OnOff(active);
        if (!active)
        {
            return;
        }
        SetIntensidad(defaultInt, iVel);
        SetShape();
    }
    #endregion
    #region FuncionesPropias
    // Acceso e spento.   
    public void OnOff(bool b)
    {
        if (pSystemRain.isPlaying ^ b) //Se non coincidono i bool.
        {
            if (b) //Play
            {
                pSystemRain.Play();
                pSystemRipples.Play();
            }
            else //Pausa
            {
                SetIntensidad(0f, iVel);

                if (pSEMLluvia.rateOverTimeMultiplier == 0)
                {
                    pSystemRain.Stop();
                    pSystemRipples.Stop();
                }
            }
        }

    }
    //Controllo di intensitÃ  con lerp.
    public void SetIntensidad(float i, float t)
    {
        rainIntensity = i;
        if (pSEMLluvia.rateOverTimeMultiplier != rainIntensity)
        {
            pSEMLluvia.rateOverTimeMultiplier = Mathf.Lerp(pSEMLluvia.rateOverTimeMultiplier, rainIntensity, t * Time.deltaTime);
            pSEMOndas.rateOverTimeMultiplier = Mathf.Lerp(pSEMLluvia.rateOverTimeMultiplier, rainIntensity / 2f, t * Time.deltaTime);
        }
        //Approssimazione per evitare un lerp troppo lungo
        if (pSEMLluvia.rateOverTimeMultiplier <= defaultInt * 0.1f && rainIntensity == 0)
        {
            pSEMLluvia.rateOverTimeMultiplier = 0f;
            pSEMOndas.rateOverTimeMultiplier = 0f;
        }
        if (pSEMLluvia.rateOverTimeMultiplier > defaultInt * 0.98f)
        {
            pSEMLluvia.rateOverTimeMultiplier = rainIntensity;
            pSEMOndas.rateOverTimeMultiplier = rainIntensity;
        }
    }
    //Forma.
    public void SetShape()
    {
        if (size.x <= 0 || size.y <= 0 || size.z <= 0) //Dimensione predefinita.
        {
            size = new Vector3(20, 20, 1);
        }
        pSSMLluvia.scale = size;
        pSSMOndas.scale = size;

        pSSMLluvia.position = pos;
        pSSMOndas.position = new Vector3(pos.x, 0.1f, pos.y); //Le onde sono sempre un po' sopra il suolo (a causa della gerarchia degli oggetti gli assi non coincidono).
    #endregion
    }
}
