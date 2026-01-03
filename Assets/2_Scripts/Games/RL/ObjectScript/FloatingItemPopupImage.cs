using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Roguelike.Define;

namespace LUP.RL
{

    public class FloatingItemPopupImage : MonoBehaviour
    {
        public Image itemimage;
        public TextMeshPro owningAmount;
        public TextMeshPro gainedAmount;

        public FloatingImageState uiState = FloatingImageState.Sleep;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void InitFloatingItemImage(Texture2D itemImage, Int32 owningAmount, Int32  gainedAmount)
        {

        }
    }
}

