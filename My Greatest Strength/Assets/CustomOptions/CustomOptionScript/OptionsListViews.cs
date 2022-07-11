using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

    public class OptionsListViews : DialogueViewBase
    {
        [SerializeField] CanvasGroup canvasGroup;

        [SerializeField] OptionViews optionViewPrefab;

        [SerializeField] TextMeshProUGUI lastLineText;

        [SerializeField] float fadeTime = 0.1f;

        [SerializeField] bool showUnavailableOptions = false;

        //0 - top lemonade juice / 1 - left lemonade juice / 2 - right lemonade juice
        [SerializeField] GameObject[] lemonadeJuice;

        //little change. This should be changed if there's more than 2 choices to choose
        [SerializeField] private Transform[] CupsChoicePosition;

        // A cached pool of OptionView objects so that we can reuse them
        List<OptionViews> optionViews = new List<OptionViews>();

        // The method we should call when an option has been selected.
        Action<int> OnOptionSelected;

        // The line we saw most recently.
        LocalizedLine lastSeenLine;

        private GameObject eventSystem;

        public void Start()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            
            eventSystem = GameObject.FindWithTag("EventSystem");
        }

        public void Reset()
        {
            canvasGroup = GetComponentInParent<CanvasGroup>();
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            // Don't do anything with this line except note it and
            // immediately indicate that we're finished with it. RunOptions
            // will use it to display the text of the previous line.
            lastSeenLine = dialogueLine;
            onDialogueLineFinished();
        }

        public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
        { 
            lemonadeJuice[0].GetComponent<Animator>().Play("lem_top_anim",  -1, 0f);


            
            
            eventSystem.GetComponent<YarnCommands>().canBePlayed = true;
            // Hide all existing option views
            foreach (var optionView in optionViews)
            {
                optionView.gameObject.SetActive(false);
            }

            int cupToChoose = 0;

            // If we don't already have enough option views, create more
            while (dialogueOptions.Length > optionViews.Count)
            {
                if (dialogueOptions.Length > 2)
                {
                    Debug.LogWarning("Please use only 2 choices in Yarn Spinner!");
                    break;
                }

                if (CupsChoicePosition[cupToChoose] == null)
                {
                    Debug.Log("Forgot to attach choices to cup position");
                    break;
                }
                var optionView = CreateNewOptionView(CupsChoicePosition[cupToChoose]);
                cupToChoose += 1;
                optionView.gameObject.SetActive(false);
            }

            // Set up all of the option views
            int optionViewsCreated = 0;

            for (int i = 0; i < dialogueOptions.Length; i++)
            {
                var optionView = optionViews[i];
                var option = dialogueOptions[i];

                if (option.IsAvailable == false && showUnavailableOptions == false)
                {
                    // Don't show this option.
                    continue;
                }

                optionView.gameObject.SetActive(true);

                optionView.Option = option;

                // The first available option is selected by default
                if (optionViewsCreated == 0)
                {
                    optionView.Select();
                }

                optionViewsCreated += 1;
            }

            // Update the last line, if one is configured
            if (lastLineText != null)
            {
                if (lastSeenLine != null) {
                    lastLineText.gameObject.SetActive(true);
                    lastLineText.text = lastSeenLine.Text.Text;
                } else {
                    lastLineText.gameObject.SetActive(false);
                }
            }

            // Note the delegate to call when an option is selected
            OnOptionSelected = onOptionSelected;

            // Fade it all in
            StartCoroutine(Effects.FadeAlpha(canvasGroup, 0, 1, fadeTime));

            // <summary>
            // Creates and configures a new <see cref="OptionView"/>, and adds
            // it to <see cref="optionViews"/>.
            // </summary>
            // new change, accepts transform position*
            OptionViews CreateNewOptionView(Transform position)
            {
                var optionView = Instantiate(optionViewPrefab);
                optionView.transform.SetParent(position.transform, false);
                optionView.transform.SetAsLastSibling();

                optionView.OnOptionSelected = OptionViewWasSelected;
                optionViews.Add(optionView);
                

                return optionView;
            }

            /// <summary>
            /// Called by <see cref="OptionView"/> objects.
            /// </summary>
            void OptionViewWasSelected(DialogueOption option)
            {
                if (!optionViews[0].hasSubmittedOptionSelection && !optionViews[1].hasSubmittedOptionSelection)
                {
                    
                    StartCoroutine(OptionViewWasSelectedInternal(option));
                    
                    IEnumerator OptionViewWasSelectedInternal(DialogueOption selectedOption)
                    {
                        if (eventSystem != null && eventSystem.GetComponent<YarnCommands>().canBePlayed)
                        {
                            eventSystem.GetComponent<YarnCommands>().canBePlayed = false;
                            if (option.DialogueOptionID == 0)
                            {
                                lemonadeJuice[1].GetComponent<Animator>().Play("lem_bot_left_anim",  -1, 0f);
                            }
                            else
                            {
                                lemonadeJuice[2].GetComponent<Animator>().Play("lem_bot_left_anim",  -1, 0f);
                            }
                            yield return StartCoroutine(eventSystem.GetComponent<YarnCommands>().CupChoices(option.DialogueOptionID));
                        }
                        yield return StartCoroutine(Effects.FadeAlpha(canvasGroup, 1, 0, fadeTime));
                        OnOptionSelected(selectedOption.DialogueOptionID);
                    }
                }
                else
                {
                    Debug.LogWarning("Option Already Selected");
                }
            }
        }
    }

