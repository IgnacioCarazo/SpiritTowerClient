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

   private bool openChest = false;
   
   public GameObject coin;
   public GameObject coin1; 
   public GameObject coin2;
   public GameObject coin3;
   public GameObject coin4;
   public GameObject coin5;


   private void Start()
   {
      this.spriteRenderer.sprite = closedSprite;
      coin.SetActive(false);
      coin1.SetActive(false);
      coin2.SetActive(false);
      coin3.SetActive(false);
      coin4.SetActive(false);
      coin5.SetActive(false);
   }

   void OnCollisionEnter2D(Collision2D other)
   {
      if (other.gameObject.name == "Greivin" & openChest==false)
      {
         spriteRenderer.sprite = openSprite;
         Debug.Log("Colision :VAVAVAV");
         coin.SetActive(true);
         coin1.SetActive(true);
         coin2.SetActive(true);
         coin3.SetActive(true);
         coin4.SetActive(true);
         coin5.SetActive(true);
         openChest = true;
      }
     
   }
}
