using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
   public GameObject damageTextPrefab;
   public GameObject healthTextPrefab;
   //public GameObject coinTextPrefab;
   public Canvas gameCanvas;


   private void Awake()
   {
      gameCanvas = FindObjectOfType<Canvas>();
   }

   private void OnEnable()
   {
      CharacterEvents.characterDamaged += CharacterTookDamage;
      CharacterEvents.characterHealed += CharacterHealed;
      //CharacterEvents.characterTookCoin += CharacterTookCoin;
   }

   private void OnDisable()
   {
      CharacterEvents.characterDamaged -= CharacterTookDamage;
      CharacterEvents.characterHealed -= CharacterHealed;
   }

   public void CharacterTookDamage(GameObject character, int damageReceived)
   {
      Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
      TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
         .GetComponent<TMP_Text>();
      tmpText.text = damageReceived.ToString();
   }

   public void CharacterHealed(GameObject character, int healthRestored)
   {
      Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
      TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
         .GetComponent<TMP_Text>();
      tmpText.text = healthRestored.ToString(); 
   }

   /*public void CharacterTookCoin(GameObject character, int parCoin)
   {
      Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
      TMP_Text tmpText = Instantiate(coinTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
         .GetComponent<TMP_Text>();
      tmpText.text = parCoin.ToString(); 
   }*/

   
   public void OnExitGame(InputAction.CallbackContext context)
   {
      if (context.started)
      {
         #if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
         #endif
         #if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
         #elif (UNITY_STANDALONE)
            Application.Quit();
         #elif (UNITY_WEBGL)
            SceneManager.Loadscene("QuitScene");
         #endif

      }
   }
}

