using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public float runSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform playerTransform = GetComponent<Transform>();
        playerTransform.Translate(Vector2.right *Input.GetAxisRaw("Horizontal") * runSpeed * Time.deltaTime);
    }
}
