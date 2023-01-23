using UnityEngine;
using TMPro;

using Necropanda.Interfaces;
using Necropanda.Utils.Console.Commands;

/// <summary>
/// Authored & Written by @mrobertscgd
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.Utils.Console
{
    /// <summary>
    /// This is the Developer Console Behaviour, it allows the console to interact with the UI and handles any/all processing of commands
    /// that come in using the TMP Input field.
    /// 
    /// The script will make sure that its the only one in the scene, if not, it will fix that by destroying itself or making itself the declared instance.
    /// </summary>
    public class DeveloperConsoleBehaviour : MonoBehaviour
    {
        [SerializeField] private string prefix = string.Empty;  // The prefix we will send commands with.
        public ConsoleCommand[] commands = new ConsoleCommand[0]; // The list of commands we can use.

        // UI Stuff, self explanatory.
        [Header("UI")]
        [SerializeField] private GameObject uiCanvas = null;
        [SerializeField] private TMP_InputField inputField = null;
        [SerializeField] private TMP_Text consoleText = null;

        [Header("Game UI Scripts")]
        [SerializeField] private HUDInterface hudInterface;
        [SerializeField] private JournalMainCode journalMainCode;
        [SerializeField] private InventoryManager inventoryManager;


        private float pausedTimeScale;  // Float for holding the paused time scale.

        private static DeveloperConsoleBehaviour instance;  // The instance of the console. There can only be one.

        private static string _outputMessage;
        public static string OutputMessage
        {
            get { return _outputMessage; }
            set
            {
                // Check the message len
                if (value.Length > 50)
                {
                    // Warn that the status message might be too long
                    Debug.LogWarning("Status message was too long! It might not fit in the window!");
                    // Set the status variable
                    _outputMessage = value;
                }
                else
                {
                    // Set the status variable
                    _outputMessage = value;
                }
            }
        }

        private DeveloperConsole developerConsole;  // Reference the the Console
        private DeveloperConsole DeveloperConsole   // Getter for the console
        {
            get
            {
                // if its NOT null, it already exists, so go and get us it.
                if (developerConsole != null) { return developerConsole; }
                // if it IS null, make us a new one!
                return developerConsole = new DeveloperConsole(prefix, commands);
            }
        }

        /// <summary>
        /// Called when the object is loaded, hands the singleton check.
        /// </summary>
        private void Awake()
        {
            // Singleton check.
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Update is called every frame. We are only using it to check for a key press.
        /// </summary>
        private void Update()
        {
            // check for our key press. This then toggles the console window.
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                Toggle();
            }
        }

        /// <summary>
        /// Toggles the console UI and handles the timescale.
        /// </summary>
        public void Toggle()
        {

            inputField.text = string.Empty;
            hudInterface = GameObject.FindObjectOfType<HUDInterface>();
            inventoryManager = GameObject.FindObjectOfType<InventoryManager>();
            journalMainCode = GameObject.FindObjectOfType<JournalMainCode>();

            if (uiCanvas.activeSelf)
            {
                Time.timeScale = pausedTimeScale;
                uiCanvas.SetActive(false);
            }
            else
            {
                if (hudInterface != null)
                {
                    hudInterface.enabled = false;
                }
                else if (inventoryManager != null)
                {
                    inventoryManager.enabled = false;
                }
                else if (journalMainCode != null)
                {
                    journalMainCode.enabled = false;
                }
                pausedTimeScale = Time.timeScale;
                Time.timeScale = 0;
                uiCanvas.SetActive(true);
                inputField.ActivateInputField();
            }
        }

        /// <summary>
        /// This processes the command given in. Calls upon the DeveloperConsole classes ProcessCommand function.
        /// </summary>
        /// <param name="inputValue">The command that is being called.</param>
        public void ProcessCommand(string inputValue)
        {
            DeveloperConsole.ProcessCommand(inputValue);    // Process the command.

            inputField.text = string.Empty; // Reset the input field text.
        }

        public void UpdateOutputMessage()
        {
            consoleText.text += "\n";
            consoleText.text += OutputMessage;
        }
    }
}

