using System;
using Project.Scripts.Tiles;
using UnityEngine;

namespace Project.Scripts.UIScripts.Effects
{
    public class PartyEffect : MonoBehaviour
    {
        private ParticleSystem convetie;

        private void Awake()
        {
            convetie = GetComponent<ParticleSystem>();
        }

        private void Start()
        {
            TileFieldManager.Instance.OnCombo += StartEffect;
            TileFieldManager.Instance.OnDoneFalling += StopEffect;
        }

        private void StartEffect(TileFieldManager.ComboAppraisal appraisal)
        {
            if (appraisal == TileFieldManager.ComboAppraisal.Party) convetie.Play();
        }

        private void StopEffect()
        {
            convetie.Stop();
        }
    }
}
