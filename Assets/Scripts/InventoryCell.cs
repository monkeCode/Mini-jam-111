using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour, IPointerClickHandler
{
   private Item _item;
   public Action<Item> OnClick;
   
   public void SetItem(Item item)
   {
      var img = GetComponent<Image>();
      if(item == null)
      {
         _item = null;
         img.enabled = false;
         return;
      }
      _item = item;
      img.enabled = true;
      img.sprite = item.Icon;
      
   }

   public void OnPointerClick(PointerEventData eventData)
   {
      if(_item != null)
         OnClick?.Invoke(_item);
   }
}
