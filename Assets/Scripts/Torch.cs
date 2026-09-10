using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] private Light torch;
    private bool isOn;
    // Start is called before the first frame update
    void Start()
    {
        torch.enabled = true;  
    }
    private void OnEnable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnTorch += AccendiTorcia;
        }
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnTorch -= AccendiTorcia;
        }
    }
    
    public void AccendiTorcia()
    {
        isOn = !isOn;
        torch.enabled = isOn;
    }
}
