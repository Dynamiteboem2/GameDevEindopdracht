using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel2 : MonoBehaviour
{
  private void OnTriggerEnter(Collider other)
  {
    if (other.tag =="Player")
    {
        SceneManager.LoadSceneAsync(3);
    }
  }
}