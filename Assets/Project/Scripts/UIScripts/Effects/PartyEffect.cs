using Project.Scripts.Tiles;
using UnityEngine;

namespace Project.Scripts.UIScripts.Effects
{
    public class PartyEffect : MonoBehaviour
    {
        private void Start()
        {
            TileFieldManager.Instance.OnCombo += StartEffect;
            TileFieldManager.Instance.OnDoneFalling += StopEffect;
            gameObject.SetActive(false);
        }

        private void StartEffect(TileFieldManager.ComboAppraisal appraisal)
        {
            if(appraisal == TileFieldManager.ComboAppraisal.Party) gameObject.SetActive(true);
        }

        private void StopEffect()
        {
            gameObject.SetActive(false);
        }
    }
}
