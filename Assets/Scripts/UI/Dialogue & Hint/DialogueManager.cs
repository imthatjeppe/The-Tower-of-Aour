using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UnityCore {
    namespace Audio {
        public class DialogueManager : MonoBehaviour {
            [SerializeField] GameObject dialogueBox, nextDialogueArrow;
            [SerializeField] Text nameText, dialogueText;
            AudioController audioController;
            DialogueHolder getDialogue;

            public float textSpeed = 0.025f;

            string[] holdDialogue;
            bool dialogueActive, writingOut;
            int numInDialogue;

            void Start() {
                audioController = GameObject.Find("AudioController").GetComponent<AudioController>();
            }

            void Update() {
                if (!dialogueActive)
                    return;

                if (writingOut && Input.GetKeyDown(KeyCode.E)) {
                    SkipSentence();
                    return;
                }

                if (Input.GetKeyDown(KeyCode.E)) {
                    NextSentence();
                    audioController.PlayAudio(AudioType.SFX_ClickInTextBox);
                }
            }

            void SkipSentence() {
                StopAllCoroutines();
                dialogueText.text = holdDialogue[numInDialogue];
                nextDialogueArrow.SetActive(true);
                writingOut = false;
                numInDialogue++;
            }

            void NextSentence() {
                if (numInDialogue == getDialogue.dialogueText.Length) {
                    nextDialogueArrow.SetActive(false);
                    getDialogue.dialogueDone = true;
                    dialogueActive = false;
                    dialogueBox.SetActive(false);
                    numInDialogue = 0;
                    return;
                }

                nextDialogueArrow.SetActive(false);
                StartCoroutine(TypeSentence(holdDialogue[numInDialogue]));
            }

            void OnTriggerEnter(Collider other) {
                if (dialogueActive)
                    return;

                if (other.CompareTag("Dialogue")) {
                    getDialogue = other.GetComponent<DialogueHolder>();
                    if (getDialogue.dialogueDone)
                        return;

                    dialogueActive = true;
                    dialogueBox.SetActive(true);
                    nameText.text = getDialogue.speakerName + ":";
                    holdDialogue = new string[getDialogue.dialogueText.Length];
                    for (int i = 0; i < holdDialogue.Length; i++) {
                        holdDialogue[i] = getDialogue.dialogueText[i];
                    }

                    StartCoroutine(TypeSentence(holdDialogue[numInDialogue]));
                }
            }

            IEnumerator TypeSentence(string sentence) {
                dialogueText.text = "";
                writingOut = true;
                foreach (char letter in sentence.ToCharArray()) {
                    dialogueText.text += letter;
                    yield return new WaitForSeconds(textSpeed);
                }
                writingOut = false;
                nextDialogueArrow.SetActive(true);
                StopCoroutine(TypeSentence(holdDialogue[numInDialogue]));
                numInDialogue++;
            }
        }
    }
}