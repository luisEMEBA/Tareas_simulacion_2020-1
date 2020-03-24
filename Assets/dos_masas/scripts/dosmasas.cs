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
        posicion_masa2 = masa_2.gameObject.GetComponent<Transform>().position;
        posicion_masa1 = masa_1.gameObject.GetComponent<Transform>().position;
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
    posicion_masa2 = masa_2.gameObject.GetComponent<Transform>().position;
    control = 2;
    }
    if(control == 3 && vector_control != null){
    posicion_masa2 = vector_control;
    posicion_masa1 = masa_1.gameObject.GetComponent<Transform>().position;
    control = 4;
    }
  }
  if(Input.GetMouseButton(0))
  {
    Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
    Vector3 currentPosition = cam.ScreenToWorldPoint(currentScreenSpace) + offset;
    if (btnName != null)
    { 
        /*
//ESTE ES EL CONDICIONAL PARA CUANDO LA MASA SOBRE LA CUAL SE PRESIONA EL RATÓN ES 
LA MASA DE ARRIBA (MASA 1), de modo que aquí se encuentran condicionales para cuando 
las dos masas se separan mucho o ya de plano se acercan mucho, de ahí el hecho de que se le sume
dos unidades y media, o se le resten.

Por otro lado, como sumercé va a meterle las otras dimensiones es menester que en la línea número
140 (restablecer.y = currentPosition.y;) haga también sumercé restablecer.x = currentPosition.x;
haiendo por supuesto cambiado en la línea 139 (Vector3 restablecer = new Vector3(posicion_masa1.x, 0 , posicion_masa1.z);)
el posicion_masa1.x por 0, de esta manera se piensa entonces el hacerlo en tres dimensiones.
(Lo anterior también debe ser entonces aplicado en el caso que se presione la masa de abajo)
        */
      if(btnName == "masa_1"){ 
      Vector3 restablecer = new Vector3(posicion_masa1.x, 0 , posicion_masa1.z);
      restablecer.y = currentPosition.y;
      vector_control = restablecer;
      go.transform.position = vector_control;
      resorte1.gameObject.GetComponent<Transform>().position = vector_control ;
      control = 1; 
      /*
Si mi chino se fija, en este caso (masa 1) control se iguala a 1, mientras que en caso de que sea masa 2 
(línea número 182) se iguala a 3, del mismo modo en que arriba con un detonador se iguala a 2 la variable controlador
y en el otro se iguala a cuatro. Esto se debe a que para cada caso las variables se inicializan de una manera distinta.
      */
      escala_resote1.y=Mathf.Abs(11f - vector_control.y);
      resorte1.transform.localScale = escala_resote1;
      //empieza movimiento de masa número dos influenciado por el arrastre de masa uno
      Vector3 movimiento_alterno;
      if((restablecer.y - posicion_masa2.y)<=2.5f){
      movimiento_alterno = new Vector3(posicion_masa2.x, (restablecer.y-2.5f), posicion_masa2.z);
      masa_2.gameObject.GetComponent<Transform>().position = movimiento_alterno;

      resorte2.gameObject.GetComponent<Transform>().position = movimiento_alterno;
      escala_resorte2.y = Mathf.Abs(vector_control.y - movimiento_alterno.y);
      }
      if((restablecer.y - posicion_masa2.y)>=4f){
      movimiento_alterno = new Vector3(posicion_masa2.x, (restablecer.y - 4f), posicion_masa2.z);
      masa_2.gameObject.GetComponent<Transform>().position = movimiento_alterno;   

      resorte2.gameObject.GetComponent<Transform>().position = movimiento_alterno;
      escala_resorte2.y = Mathf.Abs(vector_control.y - movimiento_alterno.y);
      }
      else{    
      escala_resorte2.y = Mathf.Abs(vector_control.y - resorte2.gameObject.GetComponent<Transform>().position.y);
      }
      resorte2.transform.localScale = escala_resorte2;
      //finaliza movimiento influenciado de masa dos
      }
      if(btnName == "masa_2"){
      Vector3 restablecer = new Vector3(posicion_masa2.x, 0 , posicion_masa2.z);
      Debug.Log("La posición inicial de la masa 2 es : " + posicion_masa2.y);
      restablecer.y = currentPosition.y + 3;
      vector_control = restablecer;
      go.transform.position = vector_control;
      Debug.Log("La posición con la que queda es : " + vector_control.y);
      resorte2.gameObject.GetComponent<Transform>().position = vector_control ;
      control = 3;
      escala_resorte2.y=Mathf.Abs(masa_1.gameObject.GetComponent<Transform>().position.y - vector_control.y);
      resorte2.transform.localScale = escala_resorte2;
      //empieza movimiento influenciado de la masa uno
      Vector3 movimiento_alterno;
      if((posicion_masa1.y - restablecer.y)<=2.5f){
      movimiento_alterno = new Vector3(posicion_masa1.x, (restablecer.y+2.5f), posicion_masa1.z);
      masa_1.gameObject.GetComponent<Transform>().position = movimiento_alterno;

      resorte1.gameObject.GetComponent<Transform>().position = movimiento_alterno;
      escala_resote1.y = Mathf.Abs(17f - movimiento_alterno.y);
      }
      /*
Todos estos son puros condicionales para cuando las masas están o muy lejos o muy cerca, mi chino.
      */
      if((restablecer.y - posicion_masa2.y)>=4f){
      movimiento_alterno = new Vector3(posicion_masa1.x, (restablecer.y + 4f), posicion_masa1.z);
      masa_1.gameObject.GetComponent<Transform>().position = movimiento_alterno;   

      resorte1.gameObject.GetComponent<Transform>().position = movimiento_alterno;
      escala_resote1.y = Mathf.Abs(17f - movimiento_alterno.y);
      }
      else{    
      escala_resote1.y = Mathf.Abs(17f - masa_1.gameObject.GetComponent<Transform>().position.y);
      }
      resorte2.transform.localScale = escala_resote1;
      //finaliza movimiento influenciado de la masa uno
      }
    }
    isDrage = true;
  }
  else
  {
    isDrage = false;
  }
        //finaliza arrastre
        if(control == 2 || control == 4){
         ///EMPIEZA MOVIMIENTO DEL SEGUNDO RESORTE
         f2 = fuerza(kk, posicion_masa2, R) + (f1 * d_masa_1);
         ac2 = aceleracion(f2, masa2);
         vs2 = velocidad(d, vs2, ac2);
         posicion_masa2.y = posicion_masa2.y + vs2;
        //FINALIZA MOVIMIENTO DEL SEGUNDO RESORTE  
        //EMPIEZA MOVIMIENTO PRIMER RESORTE Y MASA
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
        /*
Nota: Fíjese que las "posicion_masa1 y posicion_masa2 se inicializan en el Setup
esto, para poder calcular con ellas durante la operación de arrastre y movimiento.
De modo que no es necesario ya inicializarlas aquí en el Update.
Eso es todo en términos de arrastre. 
        */
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
        if(vs1 == 0 && vs2 == 0){
        control = 0; // estado inicial 
        }
        //FINALIZA ACTUALIZACIÓN
        }
    }
}