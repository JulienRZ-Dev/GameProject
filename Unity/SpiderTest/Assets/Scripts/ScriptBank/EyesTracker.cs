using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesTracker : MonoBehaviour
{
    public GameObject eyes;

    private Vector3 mouse_pos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Calcul de la position de la souris par rapport au centre de l'écran
        mouse_pos = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0);
        //Positionnement des yeux
        eyes.transform.position = transform.position + mouse_pos.normalized * 0.12f;
    }
}
