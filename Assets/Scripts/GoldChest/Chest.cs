using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
   [SerializeField]
   private SpriteRenderer spriteRenderer;
   [SerializeField]
   private Sprite openSprite;
   [SerializeField]
   private Sprite closedSprite;

   private void Start()
   {
      this.spriteRenderer.sprite = closedSprite;
   }

   void OnCollisionEnter2D(Collision2D other)
   {
      if (other.gameObject.name == "Greivin")
      {
         spriteRenderer.sprite = openSprite;
         Debug.Log("Colision :VAVAVAV");
      }
     
   }
}
