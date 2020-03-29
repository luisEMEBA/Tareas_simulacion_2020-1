using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unamasa : MonoBehaviour
{
    public GameObject masa_1;
    public GameObject resorte1;
    Vector3 escala_resote1 = new Vector3(1f, 1f, 1f);
    Vector3 posicion_masa1;
    Vector3 posicion_resorte1;
    float masa1 = 0.5f;
    float k_masa_1 = 0.9f;
    float d_masa_1 = 0.9f;
    float R_masa_1 = 8f;
    float vs1 = 0;
    float ac1 = 0;
    float f1 = 0;

    //x
    float vs1x = 0;
    float ac1x = 0;
    float f1x = 0;
    float angulo = 0;

    //empieza variables ensayo arrastre.
    private Camera cam;
    private GameObject go;
    public static string btnName;
    private Vector3 screenSpace;
    private Vector3 offset;
    private bool isDrage = false;
    int control = 0;
    Vector3 vector_control;
    int contador_prueba = 0;
    //finaliza variables ensayo arrastre.
    void Start()
    {
        //inicializamos cámara ensayo movimiento
        cam = Camera.main;
        //finaliza cámara ensayo movimiento
        posicion_resorte1 = resorte1.gameObject.GetComponent<Transform>().position;
        posicion_masa1 = masa_1.gameObject.GetComponent<Transform>().position;
    }
    float fuerza(float constante_resorte, Vector3 posishon, float punto_reposo)
    {
        float fuerza = (-1 * (constante_resorte)) * (posishon.y - punto_reposo);
        return fuerza;
    }

    float fuerzax(float constante_resorte, Vector3 posishon)
    {
        float fuerza = (-1 * (constante_resorte)) * (posishon.x);
        return fuerza;
    }


    float aceleracion(float fuerza_var, float masa_var)
    {
        float aceleracion = fuerza_var / masa_var;
        return aceleracion;
    }
    float velocidad(float damping, float v0, float aceleracion_var)
    {
        float vs = damping * (v0 + aceleracion_var);
        if (Mathf.Abs(vs) < 0.001)
        {
            v0 = 0;
            vs = 0;
        }
        return vs;
    }
    void Update()
    {
        //empieza arrastre
        //Overall initial position
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //Ray from camera to click coordinate
        RaycastHit hitInfo;
        if (isDrage == false)
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                //The scribed rays can only be seen in the scene view
                Debug.DrawLine(ray.origin, hitInfo.point);
                go = hitInfo.collider.gameObject;
                print(btnName);
                screenSpace = cam.WorldToScreenPoint(go.transform.position);
                offset = go.transform.position - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
                //The name of the object
                btnName = go.name;
                //Name of component
            }
            else
            {
                btnName = null;
            }
            if (control == 1 && vector_control != null)
            {
                posicion_masa1 = vector_control;
                control = 2; // estado dos = cálculo
            }
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            Vector3 currentPosition = cam.ScreenToWorldPoint(currentScreenSpace) + offset;
            if (btnName != null)
            {
                if (btnName == "Masa_1")
                {
                    Vector3 restablecer = new Vector3(0, 0, posicion_masa1.z);
                    restablecer.y = currentPosition.y;
                    restablecer.x = currentPosition.x;
                    vector_control = restablecer;
                    go.transform.position = vector_control;
                    resorte1.gameObject.GetComponent<Transform>().position = vector_control;
                    control = 1; // estado uno = almacenaje de posición
                    
                    
                    angulo = (Mathf.Atan(vector_control.x / (12.96f - vector_control.y))) * 180 / Mathf.PI;

                    
                    escala_resote1.y =Mathf.Sqrt(Mathf.Pow(12.96f - vector_control.y,2)+Mathf.Pow(vector_control.x,2));

                    
                    resorte1.transform.localScale = escala_resote1;
                    resorte1.transform.rotation = Quaternion.Euler(0, 0, angulo);
                }
            }
            isDrage = true;
        }
        else
        {
            isDrage = false;
        }
        //finaliza arrastre
        if (control == 2)
        {
            f1 = fuerza(k_masa_1, posicion_masa1, R_masa_1);
            ac1 = aceleracion(f1, masa1);
            vs1 = velocidad(d_masa_1, vs1, ac1);
            posicion_masa1.y = posicion_masa1.y + vs1;
            if (posicion_masa1.y >= 12.96f)
            {
                posicion_masa1.y = posicion_masa1.y - (posicion_masa1.y - 12.96f);
                vs1 = vs1 * d_masa_1;
            }
            if (posicion_masa1.y <= 1f)
            {
                posicion_masa1.y = posicion_masa1.y + (1f - posicion_masa1.y);
                vs1 = vs1 * d_masa_1;
            }
            ///x

            f1x = fuerzax(k_masa_1, posicion_masa1);
            ac1x = aceleracion(f1x, masa1);
            vs1x = velocidad(d_masa_1, vs1x, ac1x);
            Debug.Log(ac1x);


                posicion_masa1.x = posicion_masa1.x + vs1x;
            


            masa_1.gameObject.GetComponent<Transform>().position = posicion_masa1;
            resorte1.gameObject.GetComponent<Transform>().position = posicion_masa1;



            angulo = (Mathf.Atan((posicion_masa1.x- 0.22f) / (12.96f - posicion_masa1.y))) * 180 / Mathf.PI;


            escala_resote1.y = Mathf.Sqrt(Mathf.Pow(12.96f - posicion_masa1.y, 2) + Mathf.Pow(posicion_masa1.x- 0.22f, 2));


            resorte1.transform.localScale = escala_resote1;
            resorte1.transform.rotation = Quaternion.Euler(0, 0, angulo);
            if (vs1 == 0)
            {
                posicion_masa1.x = 0.22f;
                posicion_masa1.y = 8f;

                escala_resote1.y = Mathf.Sqrt(Mathf.Pow(12.96f - posicion_masa1.y, 2) + Mathf.Pow(posicion_masa1.x - 0.22f, 2));

                angulo = (Mathf.Atan((posicion_masa1.x - 0.22f) / (12.96f - posicion_masa1.y))) * 180 / Mathf.PI;

                
                escala_resote1.y = Mathf.Sqrt(Mathf.Pow(12.96f - posicion_masa1.y, 2) + Mathf.Pow(posicion_masa1.x - 0.22f, 2));

                resorte1.transform.rotation = Quaternion.Euler(0, 0, angulo);

                masa_1.gameObject.GetComponent<Transform>().position = posicion_masa1;
              
                resorte1.gameObject.GetComponent<Transform>().position = new Vector3(0.22f,resorte1.transform.position.y,resorte1.transform.position.z);
                control = 0; // estado inicial 

            }
        }
    }
}
