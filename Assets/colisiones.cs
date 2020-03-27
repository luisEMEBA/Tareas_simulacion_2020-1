using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class colisiones : MonoBehaviour
{
 //EMPIEZA CREACIÓN DE VARIABLES DE INTERFAZ GRÁFICA
public Text ingreso_masa_obj_1, ingreso_masa_obj_2, ingreso_velocidad_obj1, ingreso_velocidad_obj2, ingreso_coeficiente_e, letrero;
 //FINALIZA CREACIÓN DE VARIABLES DE INTERFAZ GRÁFICA
 //EMPIEZA CREACIÓN VARIABLES FÍSICAS
string masa_1s, masa_2s, velocidad_inicial_1s, velocidad_inicial_2s, coeficiente_es;
float masa_1, masa_2, velocidad_inicial_1, velocidad_inicial_2, coeficiente_e;
float velocidad_final_1, velocidad_final_2;
 //FINALIZA CREACIÓN VARIABLES FÍSICAS
 //EMPIEZA CREACIÓN DE VARIABLES OBJETOS DE JUEGO
 public GameObject objeto_1, objeto_2;
 Vector3 posicion_obj_1, posicion_obj_2;
 Vector3 posicion_inicial_para_reinicio_1, posicion_inicial_para_reinicio_2;
 //FINALIZA CREACIÓN VARIABLES OBJETOS DE JUEGO
 //EMPIEZA CREACIÓN VARIABLES DE CONTROL DE ESTADOS
 int control = 0;
 //FINALIZA CREACIÓN DE VARIABLES DE CONTROL DE ESTADOS
    void Start()
    {
        posicion_inicial_para_reinicio_1 = objeto_1.gameObject.GetComponent<Transform>().position;
        posicion_inicial_para_reinicio_2 = objeto_2.gameObject.GetComponent<Transform>().position;
    }
//EMPIEZA DEFINICIÓN DE FUNCIONES PARA EL CONTROL DE ESTADOS
public void iniciar(){
masa_1s = ingreso_masa_obj_1.text;
masa_2s = ingreso_masa_obj_2.text;
velocidad_inicial_1s = ingreso_velocidad_obj1.text;
velocidad_inicial_2s = ingreso_velocidad_obj2.text;
coeficiente_es = ingreso_coeficiente_e.text;
if(masa_1s == "" || masa_2s == "" || velocidad_inicial_1s == "" || velocidad_inicial_2s == "" || coeficiente_es == ""){
letrero.text = "Para empezar digite de manera correcta todos los valores iniciales.";
}
else{
float.TryParse(masa_1s, out masa_1);
float.TryParse(masa_2s, out masa_2);
float.TryParse(velocidad_inicial_1s, out velocidad_inicial_1);
float.TryParse(velocidad_inicial_2s, out velocidad_inicial_2);
Debug.Log("Datos : " + velocidad_inicial_1s + " " + masa_1s + " " + velocidad_inicial_2s + " " + masa_2s + " " + coeficiente_es);
control = 1;    
}
}
public void reiniciar(){
ingreso_velocidad_obj2.text = "";
ingreso_velocidad_obj1.text = "";
ingreso_masa_obj_2.text = "";
ingreso_masa_obj_1.text = "";
velocidad_final_1 = 0;
velocidad_final_2 = 0;
objeto_1.gameObject.GetComponent<Transform>().position = posicion_inicial_para_reinicio_1;
objeto_2.gameObject.GetComponent<Transform>().position = posicion_inicial_para_reinicio_2;
control = 0;
}
//FINALIZA DEFINICIÓN DE FUNCIONES PARA EL CONTROL DE ESTADOS
//EMPIEZA DEFINICIÓN DE FUNCIONES PARA CÁLCULOS
float velocidad1(float m1, float m2, float v1, float v2, float e){
float velocidad1 = (((m1 - (e*m2))/(m1 + m2))*v1) + (((1+e)*m2/(m1+m2))*v2);
return velocidad1;
}
float velocidad2(float m1, float m2, float v1, float v2, float e){
float velocidad2 = (((1+e)*m1/(m1+m2))*v1) + (((m2 - (e*m1))/(m1 + m2))*v2);
return velocidad2;
}
//FINALIZA DEFINICIÓN DE FUNCIONES PARA CÁLCULOS
    void Update()
    {
        if(control == 1){
        posicion_obj_1 = objeto_1.gameObject.GetComponent<Transform>().position;
        posicion_obj_2 = objeto_2.gameObject.GetComponent<Transform>().position;
        if(posicion_obj_1.x - posicion_obj_2.x <= 2){
        //se empieza cálculo de velocidades
        Debug.Log("Prueba.");
        //finaliza cálculo de velocidades
        }
        else{
        //Dado que no están cerca aún, se mueven en dirección a la otra
        posicion_obj_1.x = posicion_obj_1.x - velocidad_inicial_1;
        posicion_obj_2.x = posicion_obj_2.x + velocidad_inicial_2;
        //finaliza movimiento normal    
        }
        objeto_1.transform.position = posicion_obj_1;
        objeto_2.transform.position = posicion_obj_2;
        }
    }
}
