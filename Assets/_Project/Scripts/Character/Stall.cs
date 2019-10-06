using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stall : MonoBehaviour
{
    [Header("Stall Setup")]
    [Tooltip("How long the character will be stalled for.")]
    [SerializeField] private float stallTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyStall()
    {
        print("Stalled");
    }
}
