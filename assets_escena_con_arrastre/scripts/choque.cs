using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class choque : MonoBehaviour
{
    public GameObject masa_1;
    public GameObject masa_2;
    public GameObject Objetivo1;
    public GameObject Objetivo2;
    Vector3 posicion_objetivo1;
    Vector3 posicion_objetivo2;
    float variacion = -0.8f, angulo, velocidad_obj1x = 1, velocidad_obj2x = 0 , velocidad_obj1y = 0.4f, velocidad_obj2y = 0, e = 0.99999f;
    Vector3 posicion_obj1, posicion_obj2;
    float masa1 = 2.5f, masa2 = 20000;
    float vp1, vp2, vpp1, vpp2, vn1, vn2;
    //empieza variables ensayo arrastre.
    private Camera cam;
    private GameObject go;
    public static string btnName;
    private Vector3 screenSpace;
    private Vector3 offset;
    private bool isDrage = false;
    int control_1 = 0, control_2 = 0;
    Vector3 vector_control;
    int contador_prueba = 0;
    int control_angulo = 0;
    int control_direccion = 0;
    int control_fase = 0;
    float r1, r2;
    //finaliza variables ensayo arrastre.
    // Start is called before the first frame update
    void Start()
    {
       //inicializamos cámara ensayo movimiento
        cam = Camera.main;
        //finaliza cámara ensayo movimiento
        posicion_obj1 = masa_1.gameObject.GetComponent<Transform>().position;
        posicion_obj2 = masa_2.gameObject.GetComponent<Transform>().position;
        Debug.Log("Posicion " + posicion_obj1);
        posicion_objetivo1 = Objetivo1.gameObject.GetComponent<Transform>().position;
        posicion_objetivo2 = Objetivo2.gameObject.GetComponent<Transform>().position;
    }
    void calcular_angulo(Vector3 pos_i1, Vector3 pos_i2){
    float dis_x = 0, dis_y= 0;
    dis_x = Mathf.Abs(pos_i1.x - pos_i2.x);
    dis_y = Mathf.Abs(pos_i1.y - pos_i2.y);
    if(dis_x == 0){
    dis_x = 0.00001f;
    }
    if(dis_y == 0){
    dis_y = 0.00001f;
    }
    angulo = Mathf.Atan(dis_y/dis_x);    
    }

    void direccion(Vector3 pos1, Vector3 poso1, Vector3 pos2, Vector3 poso2){
    Vector3 direccion1, direccion2;
    direccion1 = poso1 - pos1;
    direccion2 = poso2 - pos2;
    direccion1 = direccion1.normalized;
    direccion2 = direccion2.normalized;
    velocidad_obj1x = (velocidad_obj1x) * direccion1.x;
    velocidad_obj1y = (velocidad_obj1y) * direccion1.y;

    velocidad_obj2x = (velocidad_obj2x) * direccion2.x;
    velocidad_obj2y = (velocidad_obj2y) * direccion2.y; 
    }
    void calculo_vp(float m1, float m2, float constante, float v1p, float v2p){
    vpp1 = (((m1 - (constante * m2))/(m1 + m2))*v1p) + ((((1+constante)*m2)/(m1+m2))*v2p);
    vpp2 = ((((1+constante)*m1)/(m1+m2))*v1p) + (((m2 - (constante*m1))/(m1+m2))*v2p);
    }
    void conversion_vp_a_vx_vy(){
    velocidad_obj1x = (vpp1*Mathf.Cos(angulo)) - (vn1*Mathf.Sin(angulo));
    velocidad_obj1y = (vpp1*Mathf.Sin(angulo)) + (vn1*Mathf.Cos(angulo));
    velocidad_obj2x = (vpp2*Mathf.Cos(angulo)) - (vn2*Mathf.Sin(angulo));
    velocidad_obj2y = (vpp2*Mathf.Sin(angulo)) + (vn2*Mathf.Cos(angulo));
    }

    // Update is called once per frame
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
            if (control_1 == 1 && vector_control != null || control_2 == 1 && vector_control != null)
            {
                if(control_1 == 1){
                posicion_obj1 = vector_control;
                control_1 = 2; // estado dos = cálculo
                }
                if(control_2 == 1){
                posicion_obj2 = vector_control;
                control_2 = 2; // estado dos = cálculo
                }
            }
            if(control_1 == 3 && vector_control != null || control_2 == 3 && vector_control != null){
              if(control_1 == 3){
              posicion_objetivo1 = vector_control;                 
              control_1 = 4;
              }
              if(control_2 == 3){
              posicion_objetivo2 = vector_control; 
              control_2 = 4;
              }
            }
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            Vector3 currentPosition = cam.ScreenToWorldPoint(currentScreenSpace) + offset;
            if (btnName != null)
            {

                    if(btnName == "Masa_1"){
                    Vector3 restablecer1 = new Vector3(0, 0, 0);
                    restablecer1.y = currentPosition.y;
                    restablecer1.x = currentPosition.x;
                    vector_control = restablecer1;
                    //go.transform.position = vector_control;
                    masa_1.gameObject.GetComponent<Transform>().position = vector_control;
                    control_1 = 1; // estado uno = almacenaje de posición                    
                    }
                    if(btnName == "Masa_2"){
                    Vector3 restablecer2 = new Vector3(0, 0, 0);
                    restablecer2.y = currentPosition.y;
                    restablecer2.x = currentPosition.x;
                    vector_control = restablecer2;
                    //go.transform.position = vector_control;
                    masa_2.gameObject.GetComponent<Transform>().position = vector_control;
                    control_2 = 1; // estado uno = almacenaje de posición
                    }
                    if(btnName == "Objetivo1" ){
                    
                    Vector3 objetivo = new Vector3(0,0,0);
                    objetivo.x = currentPosition.x;
                    objetivo.y = currentPosition.y;
                    vector_control = objetivo;

                    Objetivo1.gameObject.GetComponent<Transform>().position = vector_control;
                    control_1 = 3;
                    
                    }
                    if(btnName == "Objetivo2" ){
                    
                    Vector3 objetivo = new Vector3(0,0,0);
                    objetivo.x = currentPosition.x;
                    objetivo.y = currentPosition.y;
                    vector_control = objetivo;
                    posicion_objetivo2 = vector_control;
                    Objetivo2.gameObject.GetComponent<Transform>().position = vector_control;  
                    control_2 = 3;
                    
                    }
                 
            }
            isDrage = true;
        }
        else
        {
            isDrage = false;
        }
        //finaliza arrastre
        if(control_1 == 4 && control_2 == 4){
        
        
        
        
        //Cálculos necesarios para cuando se tocan los objetos
        if((Mathf.Abs(posicion_obj1.x - posicion_obj2.x)) <= 1 && (Mathf.Abs(posicion_obj1.y - posicion_obj2.y)) <= 1){
        //cálculo del ángulo de la línea de acción
        calcular_angulo(posicion_obj1, posicion_obj2); 
        //finaliza cálculo del ángulo de la línea de acción.
        //empieza cálculo de Vp
        vp1 = (velocidad_obj1x * Mathf.Cos(angulo)) + (velocidad_obj1y * Mathf.Sin(angulo));
        vp2 = (velocidad_obj2x * Mathf.Cos(angulo)) + (velocidad_obj2y * Mathf.Sin(angulo));;
        //finaliza cálculo de Vp
        //empieza cálculo de Vn
        vn1 = (-1*velocidad_obj1x*Mathf.Sin(angulo)) + (velocidad_obj1y * Mathf.Sin(angulo));
        vn2 = (-1*velocidad_obj2x*Mathf.Sin(angulo)) + (velocidad_obj2y * Mathf.Sin(angulo));
        //finaliza cálculo de Vn
        calculo_vp(masa1, masa2,e, vp1, vp2);
        conversion_vp_a_vx_vy();
        control_fase = 1;
        }
        //Finalizan cálculos necesarios para cuando los objetos se tocan.
        //Empieza movimiento de los objetos para chocar
        else{
            if(control_fase == 0){
            while(control_direccion == 0){
            direccion(posicion_obj1, posicion_objetivo1, posicion_obj2, posicion_objetivo2);
            control_direccion = 1;
            }
            }
        }
        //Finaliza movimiento de los objetos para chocar.
        posicion_obj1.x = posicion_obj1.x + velocidad_obj1x;
        posicion_obj1.y = posicion_obj1.y + velocidad_obj1y;
        posicion_obj2.x = posicion_obj2.x + velocidad_obj2x;
        posicion_obj2.y = posicion_obj2.y + velocidad_obj2y;
        masa_1.gameObject.GetComponent<Transform>().position = posicion_obj1;
        masa_2.gameObject.GetComponent<Transform>().position = posicion_obj2;
        }
    }
}
