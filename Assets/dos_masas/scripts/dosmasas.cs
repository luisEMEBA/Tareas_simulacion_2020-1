using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dosmasas : MonoBehaviour
{
    public GameObject masa_1;
    public GameObject resorte1;
    public GameObject masa_2;
    public GameObject resorte2;
    //empieza variables ensayo arrastre.
    private Camera cam;
    private GameObject go;
    public static string btnName;
    private Vector3 screenSpace;
    private Vector3 offset;
    private bool isDrage = false;
    //finaliza variables ensayo arrastre.
    Vector3 escala_resorte2=new Vector3(1f,1f,1f);
    Vector3 posicion_resorte2;
    float xr0;
    Vector3 escala_resote1=new Vector3(1f,1f,1f);
    Vector3 posicion_masa1;
    Vector3 posicion_resorte1;
    float inicial_r2;
    //EMPIEZAN VARIABLES POR SEGUNDO MÉTODO
    //constantes segundo resorte y masa
    float masa2=0.8f;
    float kk = 0.2f;
    float d = 0.95f;
    float R = 3f;
    //constantes primer resorte y masa
    float masa1 = 0.5f;
    float k_masa_1 = 0.3f;
    float d_masa_1 = 0.9f;
    float R_masa_1 = 7f;
    //fin constantes
    //variables globales
    
    //variables primer resorte
    Vector3 posicion_masa2;
    float vs1 = 0;
    float ac1 = 0;
    float f1 = 0;
    //variables segundo resorte
    float vs2 = 0;
    float ac2 = 0;
    float f2 = 0;   
    //fin variables
    int control = 0;
    Vector3 vector_control;
    //FINALIZAN VARIABLES POR SEGUNDO MÉTODO

    void Start()
    {
        //inicializamos cámara ensayo movimiento
        cam = Camera.main;
        //finaliza cámara ensayo movimiento
        posicion_resorte1 = resorte1.gameObject.GetComponent<Transform>().position;
        xr0 = masa_1.gameObject.GetComponent<Transform>().position.y;
        Debug.Log("Posición inicial : " + xr0);
        posicion_resorte2 = resorte2.gameObject.GetComponent<Transform>().position;
        inicial_r2 = resorte2.gameObject.GetComponent<Transform>().position.y;
    }
float fuerza(float constante_resorte, Vector3 posishon, float punto_reposo){
 float fuerza = (-1*(constante_resorte)) * (posishon.y - punto_reposo);
 return fuerza;
}
float aceleracion(float fuerza_var, float masa_var){
float aceleracion = fuerza_var / masa_var;
return aceleracion;
}
float velocidad(float damping, float v0, float aceleracion_var){
    float vs = damping * (v0 + aceleracion_var);
    if(Mathf.Abs(vs)<0.001){
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
    if(Physics .Raycast (ray,out hitInfo))
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
    if(control == 1 && vector_control != null){
    posicion_masa1 = vector_control;
    control = 2;
    }
  }
  if(Input.GetMouseButton(0))
  {
    Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
    Vector3 currentPosition = cam.ScreenToWorldPoint(currentScreenSpace) + offset;
    if (btnName != null)
    { 
      if(btnName == "Masa_1"){
      Vector3 restablecer = new Vector3(posicion_masa1.x, 0 , posicion_masa1.z);
      restablecer.y = currentPosition.y;
      vector_control = restablecer;
      go.transform.position = vector_control;
      resorte1.gameObject.GetComponent<Transform>().position = vector_control ;
      control = 1;
      escala_resote1.y=Mathf.Abs(12.96f - vector_control.y);
      resorte1.transform.localScale = escala_resote1;
      //Vector3 movimiento_alterno = new Vector3(posicion_masa2.x);
      }
    }
    isDrage = true;
  }
  else
  {
    isDrage = false;
  }
        //finaliza arrastre
         ///EMPIEZA MOVIMIENTO DEL SEGUNDO RESORTE
         posicion_masa2 = masa_2.gameObject.GetComponent<Transform>().position;
         f2 = fuerza(kk, posicion_masa2, R) + (f1 * d_masa_1);
         ac2 = aceleracion(f2, masa2);
         vs2 = velocidad(d, vs2, ac2);
         posicion_masa2.y = posicion_masa2.y + vs2;
        //FINALIZA MOVIMIENTO DEL SEGUNDO RESORTE  
        //EMPIEZA MOVIMIENTO PRIMER RESORTE Y MASA
        posicion_masa1 = masa_1.gameObject.GetComponent<Transform>().position;
        f1 = fuerza(k_masa_1, posicion_masa1, R_masa_1);
        ac1 = aceleracion(f1, masa1);
        vs1 = velocidad(d_masa_1, vs1, ac1);
        posicion_masa1.y = posicion_masa1.y + vs1;
        //FINALIZAR MOVIMIENTO DEL PRIMER RESORTE Y MASA
        //ENSAYO CONDICIONALES
        if(posicion_masa1.y >= 11){
            posicion_masa1.y = posicion_masa1.y - (posicion_masa1.y - 11);
            vs1 = vs1*d;
        }
        if((posicion_masa1.y - posicion_masa2.y) <= 1.5f){
          posicion_masa2.y = posicion_masa2.y - (0.75f);
          posicion_masa1.y = posicion_masa1.y + (0.75f);
        }
        //FIN ENSAYO CONDICIONALES
        //EMPIEZA ACTUALIZACIÓN
        //actualización masa 2
         masa_2.gameObject.GetComponent<Transform>().position = posicion_masa2;
         resorte2.gameObject.GetComponent<Transform>().position = posicion_masa2;
         escala_resorte2.y = Mathf.Abs(posicion_masa1.y - posicion_masa2.y);
         resorte2.transform.localScale = escala_resorte2;
        //actualización masa 1
        masa_1.gameObject.GetComponent<Transform>().position = posicion_masa1;
        resorte1.gameObject.GetComponent<Transform>().position = posicion_masa1;
        escala_resote1.y=Mathf.Abs(11f - posicion_masa1.y);
        resorte1.transform.localScale = escala_resote1;
    
        //FINALIZA ACTUALIZACIÓN
    }
}