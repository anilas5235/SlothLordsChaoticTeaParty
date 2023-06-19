using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.DialogScripts
{
    [RequireComponent(typeof(Image))]
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private bool dialogCharacter;
        
        [SerializeField] private CharacterMoods currentMode;
        [SerializeField] private Characters currentCharacter;

        private Sprite[] characterIcons;
        private Image characterImage;
        
        #region Enums
        public enum Characters
        {
            None,
            Elenor,
            Gonzo,
            Norbert,
            Sheldon,
            Lazy
        }
        
        public enum CharacterMoods
        {
            Neutral,
            Happy,
            Offended,
            Party,
            Serious,
            Surprised,
        }
      #endregion

      #region Properties

      public CharacterMoods CurrentMode
      {
          get => currentMode;
          set
          {
              if (currentMode == value) return;
              currentMode = value;
              ModeChanged();
          }
      }
      
      public Characters CurrentCharacter
      {
          get => currentCharacter;
          set
          {
              if (currentCharacter == value) return;
              currentCharacter = value;
              LoadCharacter();
              ModeChanged();
          }
      }

      #endregion

      #region Events

      public Action<CharacterMoods> OnModeChange;

      public Action<Characters> OnCharacterChange;
      
      #endregion

      private void Awake()
      {
          characterImage = GetComponent<Image>();
      }

      private void Start()
      {
          LoadCharacter();
          ModeChanged();
      }

      private void ModeChanged()
      {
          switch (currentCharacter)
          {
              case Characters.Elenor: break;
              case Characters.Gonzo: break;
              case Characters.Norbert: break;
              case Characters.Sheldon: break;
              default: return;
          }
          characterImage.sprite = characterIcons[(int)currentMode];
          OnModeChange?.Invoke(currentMode);
      }

      private void LoadCharacter()
      {
          string characterName;

          switch (currentCharacter)
          {
              case Characters.Elenor:
                  characterName = "eleanor";
                  break;
              case Characters.Gonzo:
                  characterName = "gonzo";
                  break;
              case Characters.Norbert:
                  characterName = "norbert";
                  break;
              case Characters.Sheldon:
                  characterName = "sheldon";
                  break;
              default: return;
          }

          if (dialogCharacter)
          {
              characterIcons = new[]
              {
                  Resources.Load<Sprite>($"ArtWork/UI/CharacterEmotions/{characterName}/{characterName}_neutral"),
                  Resources.Load<Sprite>($"ArtWork/UI/CharacterEmotions/{characterName}/{characterName}_happy"),
                  Resources.Load<Sprite>($"ArtWork/UI/CharacterEmotions/{characterName}/{characterName}_offended"),
                  Resources.Load<Sprite>($"ArtWork/UI/CharacterEmotions/{characterName}/{characterName}_party"),
                  Resources.Load<Sprite>($"ArtWork/UI/CharacterEmotions/{characterName}/{characterName}_serious"),
                  Resources.Load<Sprite>($"ArtWork/UI/CharacterEmotions/{characterName}/{characterName}_surprised"),
              };
          }
          else
          {
              characterIcons = new[]
              {
                  Resources.Load<Sprite>($"ArtWork/UI/CharacterIcons/{characterName}/{characterName}_neutral"),
                  Resources.Load<Sprite>($"ArtWork/UI/CharacterIcons/{characterName}/{characterName}_happy"),
                  Resources.Load<Sprite>($"ArtWork/UI/CharacterIcons/{characterName}/{characterName}_offended"),
                  Resources.Load<Sprite>($"ArtWork/UI/CharacterIcons/{characterName}/{characterName}_party"),
              };
          }

          OnCharacterChange?.Invoke(currentCharacter);
      }
    }
}
