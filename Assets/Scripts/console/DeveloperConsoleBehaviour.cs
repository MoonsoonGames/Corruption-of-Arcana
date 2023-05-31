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
        [SerializeField] private GameObject devUICanvas = null;
        [SerializeField] private TMP_InputField inputField = null;
        [SerializeField] private TMP_Text consoleText = null;

        [Header("Game UI Scripts")]
        [SerializeField] private HUDInterface hudInterface;
        [SerializeField] private JournalMainCode journalMainCode;
        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private MapSelector mapSelector;


        private float pausedTimeScale = 0f;  // Float for holding the paused time scale.
        private float unpausedTimeScale;  // Float for holding the unpaused time scale.

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
            inputField.ActivateInputField();
            // hudInterface = GameObject.FindObjectOfType<HUDInterface>();
            // inventoryManager = GameObject.FindObjectOfType<InventoryManager>();
            // journalMainCode = GameObject.FindObjectOfType<JournalMainCode>();


            devUICanvas.SetActive(!devUICanvas.activeInHierarchy);

            // Check to see if the Developer UI Canvas is active in the scene
            if (devUICanvas.activeInHierarchy)
            {
                // If the Console is enabled, disable UI scripts
                if (hudInterface)
                {
                    hudInterface.enabled = false;
                }
                if (inventoryManager)
                {
                    inventoryManager.enabled = false;
                }
                if (journalMainCode)
                {
                    journalMainCode.enabled = false;
                }
                if (mapSelector)
                {
                    mapSelector.enabled = false;
                }
                if (TEMP_OpenDeckbuilding.instance)
                {
                    TEMP_OpenDeckbuilding.instance.enabled = false;
                }

                unpausedTimeScale = Time.timeScale; // store the unpaused timescale before changing it
                Time.timeScale = 0;
            }
            else
            {
                // If the Console is disabled, enable UI scripts
                if (hudInterface)
                {
                    hudInterface.enabled = true;
                }
                if (inventoryManager)
                {
                    inventoryManager.enabled = true;
                }
                if (journalMainCode)
                {
                    journalMainCode.enabled = true;
                }
                if (mapSelector)
                {
                    mapSelector.enabled = true;
                }
                if (TEMP_OpenDeckbuilding.instance)
                {
                    TEMP_OpenDeckbuilding.instance.enabled = true;
                }

                Time.timeScale = 1;
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

