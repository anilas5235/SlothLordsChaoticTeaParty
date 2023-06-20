using System;
using Project.Scripts.DialogScripts;
using UnityEngine;

namespace Project.Scripts.UIScripts.Effects
{
    public class SpaceTipText : BreathingText
    {
        private void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
    
