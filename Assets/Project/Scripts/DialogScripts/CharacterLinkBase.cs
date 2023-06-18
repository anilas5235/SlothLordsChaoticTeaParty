using UnityEngine;

namespace Project.Scripts.DialogScripts
{
    public abstract class CharacterLinkBase : MonoBehaviour
    {
        [SerializeField] protected CharacterAnimator myCharacterAnimator;

        protected virtual void Awake()
        {
            myCharacterAnimator = GetComponent<CharacterAnimator>();
        }

        protected virtual void ChangeCharacterMode(CharacterAnimator.CharacterMoods mood)
        {
            myCharacterAnimator.CurrentMode = mood;
        }

        protected virtual void SelectCharacter(CharacterAnimator.Characters character)
        {
            myCharacterAnimator.CurrentCharacter = character;
        }
    }
}
