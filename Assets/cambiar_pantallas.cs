using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cambiar_pantallas : MonoBehaviour {

    public void a_1x1(string nombredeescena)
    {

        SceneManager.LoadScene(1);
}
    public void a_2x2(string nombredeescena)
    {

        SceneManager.LoadScene(2);
}
    public void a_3x3(string nombredeescena)
    {

        SceneManager.LoadScene(0);
}
}
