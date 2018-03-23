using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Prototype.NetworkLobby
{
    //Player entry in the lobby. Handle selecting color/setting name & getting ready for the game
    //Any LobbyHook can then grab it and pass those value to the game player prefab (see the Pong Example in the Samples Scenes)
    public class LobbyPlayer : NetworkLobbyPlayer
    {
        static Color[] Colors = new Color[] {Color.red,  Color.blue};
        static string[] Robots = new string[] {  "OBSERVER" ,"HERO","ENGINEER","INFANTRY1", "INFANTRY2", "INFANTRY3" };
        static string[] Spawns = new string[] { "SIX","ONE", "TWO", "THREE", "FOUR", "FIVE"  };
        //used on server to avoid assigning the same color to two player
        static List<int> _colorInUse = new List<int>();
        private bool colorchoose = true;
        static private int redplayernum = 0;
        static private int blueplayernum = 0;
        static public int maxplayernum = 5;
        static List<int> _redrobotInUse = new List<int>();
        static List<int> _bluerobotInUse = new List<int>();
        static List<int> _redspawnInUse = new List<int>();
        static List<int> _bluespawnInUse = new List<int>();
        public Button colorButton;
        public Button robotButton;
        public Button spawnButton;
        public Text Robotname;
        public Text Spawnname;

        public InputField nameInput;
        public Button readyButton;
        public Button waitingPlayerButton;
        public Button removePlayerButton;

        public GameObject localIcone;
        public GameObject remoteIcone;

        //OnMyName function will be invoked on clients when server change the value of playerName
        [SyncVar(hook = "OnMyName")]
        public string playerName = "";

        [SyncVar(hook = "OnMyColor")]
        public Color playerColor = Color.red;
        [SyncVar(hook = "OnMyRobot")]
        public string playerRobot = "ROBOT";

        [SyncVar(hook = "OnMySpawn")]
        public string playerSpawn = "SPAWN";

        public Color OddRowColor = new Color(250.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f, 1.0f);
        public Color EvenRowColor = new Color(180.0f / 255.0f, 180.0f / 255.0f, 180.0f / 255.0f, 1.0f);

        static Color JoinColor = new Color(255.0f/255.0f, 0.0f, 101.0f/255.0f,1.0f);
        static Color NotReadyColor = new Color(34.0f / 255.0f, 44 / 255.0f, 55.0f / 255.0f, 1.0f);
        static Color ReadyColor = new Color(0.0f, 204.0f / 255.0f, 204.0f / 255.0f, 1.0f);
        static Color TransparentColor = new Color(0, 0, 0, 0);

        //static Color OddRowColor = new Color(250.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f, 1.0f);
        //static Color EvenRowColor = new Color(180.0f / 255.0f, 180.0f / 255.0f, 180.0f / 255.0f, 1.0f);


        public override void OnClientEnterLobby()
        {
            base.OnClientEnterLobby();

            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(1);

            LobbyPlayerList._instance.AddPlayer(this);
            LobbyPlayerList._instance.DisplayDirectServerWarning(isServer && LobbyManager.s_Singleton.matchMaker == null);

            if (isLocalPlayer)
            {
                SetupLocalPlayer();
            }
            else
            {
                SetupOtherPlayer();
            }

            //setup the player data on UI. The value are SyncVar so the player
            //will be created with the right value currently on server
            OnMyName(playerName);
            OnMyColor(playerColor);
            OnMyRobot(playerRobot);
            OnMySpawn(playerSpawn);
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();

            //if we return from a game, color of text can still be the one for "Ready"
            readyButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;

           SetupLocalPlayer();
        }

        void ChangeReadyButtonColor(Color c)
        {
            ColorBlock b = readyButton.colors;
            b.normalColor = c;
            b.pressedColor = c;
            b.highlightedColor = c;
            b.disabledColor = c;
            readyButton.colors = b;
        }

        void SetupOtherPlayer()
        {
            nameInput.interactable = false;
            removePlayerButton.interactable = NetworkServer.active;

            ChangeReadyButtonColor(NotReadyColor);

            readyButton.transform.GetChild(0).GetComponent<Text>().text = "...";
            readyButton.interactable = false;

            OnClientReady(false);
        }

        void SetupLocalPlayer()
        {
            nameInput.interactable = true;
            remoteIcone.gameObject.SetActive(false);
            localIcone.gameObject.SetActive(true);
            CheckRemoveButton();
            CmdColorChangeInit();
            CmdRobotChange();
            CmdSpawnChange();

            ChangeReadyButtonColor(JoinColor);

            readyButton.transform.GetChild(0).GetComponent<Text>().text = "SIGN IN";
            readyButton.interactable = true;

            //have to use child count of player prefab already setup as "this.slot" is not set yet
            if (playerName == "")
                CmdNameChanged("Player" + (LobbyPlayerList._instance.playerListContentTransform.childCount-1));

            //we switch from simple name display to name input
            colorButton.interactable = true;
            robotButton.interactable = true;
            spawnButton.interactable = true;

            nameInput.interactable = true;
            nameInput.onEndEdit.RemoveAllListeners();
            nameInput.onEndEdit.AddListener(OnNameChanged);

            colorButton.onClick.RemoveAllListeners();
            colorButton.onClick.AddListener(OnColorClicked);
            robotButton.onClick.RemoveAllListeners();
            robotButton.onClick.AddListener(OnRobotClicked);
            spawnButton.onClick.RemoveAllListeners();
            spawnButton.onClick.AddListener(OnSpawnClicked);

            readyButton.onClick.RemoveAllListeners();
            readyButton.onClick.AddListener(OnReadyClicked);


            //when OnClientEnterLobby is called, the loval PlayerController is not yet created, so we need to redo that here to disable
            //the add button if we reach maxLocalPlayer. We pass 0, as it was already counted on OnClientEnterLobby
            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(0);
        }



        //This enable/disable the remove button depending on if that is the only local player or not
        public void CheckRemoveButton()
        {
            if (!isLocalPlayer)
                return;

            int localPlayerCount = 0;
            foreach (PlayerController p in ClientScene.localPlayers)
                localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;

            removePlayerButton.interactable = localPlayerCount > 1;
        }

        public override void OnClientReady(bool readyState)
        {
            if (readyState)
            {
                ChangeReadyButtonColor(TransparentColor);

                Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
                textComponent.text = "LOADING";
                //textComponent.color = ReadyColor;
                readyButton.interactable = false;
                colorButton.interactable = false;
                robotButton.interactable = false;
                spawnButton.interactable = false;
                nameInput.interactable = false;
            }
            else
            {
                ChangeReadyButtonColor(isLocalPlayer ? JoinColor : NotReadyColor);

                Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
                textComponent.text = isLocalPlayer ? "SIGN IN" : "...";
                textComponent.color = Color.white;
                readyButton.interactable = isLocalPlayer;
                colorButton.interactable = isLocalPlayer;
                robotButton.interactable = isLocalPlayer;
                spawnButton.interactable = isLocalPlayer;
                nameInput.interactable = isLocalPlayer;
            }
        }

        public void OnPlayerListChanged(int idx)
        { 
            GetComponent<Image>().color = (idx % 2 == 0) ? EvenRowColor : OddRowColor;
        }

        ///===== callback from sync var

        public void OnMyName(string newName)
        {
            playerName = newName;
            nameInput.text = playerName;
        }



        public void OnMyColor(Color newColor)
        {
            playerColor = newColor;
            colorButton.GetComponent<Image>().color = newColor;
        }

        public void OnMyRobot(string newRobot)
        {
            playerRobot = newRobot;
            Robotname.text = newRobot;
        }

        public void OnMySpawn(string newSpawn)
        {
            playerSpawn = newSpawn;
            Spawnname.text = newSpawn;
        }
        //===== UI Handler

        //Note that those handler use Command function, as we need to change the value on the server not locally
        //so that all client get the new value throught syncvar
        public void OnColorClicked()
        {
            CmdColorChange();
        }
        public void OnRobotClicked()
        {
            CmdRobotChange();
        }
        public void OnSpawnClicked()
        {
            CmdSpawnChange();
        }

        public void OnReadyClicked()
        {
            SendReadyToBeginMessage();
        }

        public void OnNameChanged(string str)
        {
            CmdNameChanged(str);
        }

        public void OnRemovePlayerClick()
        {
            if (isLocalPlayer)
            {
                if (playerColor == Color.red)
                {
                    redplayernum--;
                }
                if (playerColor == Color.blue)
                {
                    blueplayernum--;
                }
                RemovePlayer();

            }
            else if (isServer)
                LobbyManager.s_Singleton.KickPlayer(connectionToClient);
                
        }

        public void ToggleJoinButton(bool enabled)
        {
            readyButton.gameObject.SetActive(enabled);
            waitingPlayerButton.gameObject.SetActive(!enabled);
        }

        [ClientRpc]
        public void RpcUpdateCountdown(int countdown)
        {
            LobbyManager.s_Singleton.countdownPanel.UIText.text = "" + countdown;
            LobbyManager.s_Singleton.countdownPanel.gameObject.SetActive(countdown != 0);
        }

        [ClientRpc]
        public void RpcUpdateRemoveButton()
        {
            CheckRemoveButton();
        }

        //====== Server Command
        [Command]
        public void CmdColorChangeInit()
        {
            if (redplayernum >= blueplayernum)
            {
                playerColor = Color.red;
                redplayernum++;
            }
            else
            {
                playerColor = Color.blue;
                blueplayernum++;
            }
            if (redplayernum > maxplayernum)
            {
                redplayernum = maxplayernum;
                playerColor = Color.blue;
                blueplayernum++;
            }
            if (blueplayernum > maxplayernum)
            {
                blueplayernum = maxplayernum;
                playerColor = Color.red;
                redplayernum++;
            }
            if (playerColor == Color.blue)
            {
                int robotidx = System.Array.IndexOf(Robots, playerRobot);
                _redrobotInUse.Remove(robotidx);
                if (_bluerobotInUse.IndexOf(robotidx) > 0)
                {
                    CmdRobotChange();
                }
                int spawnidx = System.Array.IndexOf(Spawns, playerSpawn);
                _redspawnInUse.Remove(spawnidx);
                if (_bluespawnInUse.IndexOf(spawnidx) > 0)
                {
                    CmdSpawnChange();
                }

            }
            if (playerColor == Color.red)
            {
                int robotidx = System.Array.IndexOf(Robots, playerRobot);
                _bluerobotInUse.Remove(robotidx);
                if (_redrobotInUse.IndexOf(robotidx) > 0)
                {
                    CmdRobotChange();
                }
                int spawnidx = System.Array.IndexOf(Spawns, playerSpawn);
                _bluespawnInUse.Remove(spawnidx);
                if (_redspawnInUse.IndexOf(spawnidx) > 0)
                {
                    CmdSpawnChange();
                }
            }
        }


        [Command]
        public void CmdColorChange()
        {
            if (redplayernum == maxplayernum && playerColor == Color.blue)
            {
                return;
            }
            if (blueplayernum == maxplayernum && playerColor == Color.red)
            {
                return;
            }
            colorchoose = !colorchoose;
                if (colorchoose)
                {
                    playerColor = Color.red;
                    redplayernum++;
                    blueplayernum--;
                }
                else
                {
                    playerColor = Color.blue;
                    blueplayernum++;
                    redplayernum--;
                }
                if (playerColor == Color.blue)
                {
                    int robotidx = System.Array.IndexOf(Robots, playerRobot);
                    _redrobotInUse.Remove(robotidx);
                    if (_bluerobotInUse.IndexOf(robotidx) > 0)
                    {
                        CmdRobotChange();
                    }
                    int spawnidx = System.Array.IndexOf(Spawns, playerSpawn);
                    _redspawnInUse.Remove(spawnidx);
                    if (_bluespawnInUse.IndexOf(spawnidx) > 0)
                    {
                        CmdSpawnChange();
                    }

                }
                if (playerColor == Color.red)
                {
                    int robotidx = System.Array.IndexOf(Robots, playerRobot);
                    _bluerobotInUse.Remove(robotidx);
                    if (_redrobotInUse.IndexOf(robotidx) > 0)
                    {
                        CmdRobotChange();
                    }
                    int spawnidx = System.Array.IndexOf(Spawns, playerSpawn);
                    _bluespawnInUse.Remove(spawnidx);
                    if (_redspawnInUse.IndexOf(spawnidx) > 0)
                    {
                        CmdSpawnChange();
                    }
                }      
        }

        [Command]
        public void CmdRobotChange()
        {
      
            if (playerColor == Color.red)
            {
                int idx = System.Array.IndexOf(Robots, playerRobot);
                int inUseIdx = _redrobotInUse.IndexOf(idx);
                if (idx < 0) idx = 0;

                idx = (idx + 1) % Robots.Length;

                bool alreadyInUse = false;

                do
                {
                    alreadyInUse = false;
                    for (int i = 0; i < _redrobotInUse.Count; ++i)
                    {
                        if (_redrobotInUse[i] == idx)
                        {//that color is already in use
                            alreadyInUse = true;
                            idx = (idx + 1) % Robots.Length;
                        }
                    }
                }
                while (alreadyInUse);

                if (inUseIdx >= 0)
                {//if we already add an entry in the colorTabs, we change it
                    _redrobotInUse[inUseIdx] = idx;
                }
                else
                {//else we add it
                    _redrobotInUse.Add(idx);
                }
                playerRobot = Robots[idx];

                if (playerRobot == "OBSERVER")
                {
                    int spawnidx = System.Array.IndexOf(Spawns, playerSpawn);
                    _redspawnInUse.Remove(spawnidx);
                    RpcObserverButtonOff();
                }

            }

            if (playerColor == Color.blue)
            {
                int idx = System.Array.IndexOf(Robots, playerRobot);
                int inUseIdx = _bluerobotInUse.IndexOf(idx);
                if (idx < 0) idx = 0;

                idx = (idx + 1) % Robots.Length;

                bool alreadyInUse = false;

                do
                {
                    alreadyInUse = false;
                    for (int i = 0; i < _bluerobotInUse.Count; ++i)
                    {
                        if (_bluerobotInUse[i] == idx)
                        {//that color is already in use
                            alreadyInUse = true;
                            idx = (idx + 1) % Robots.Length;
                        }
                    }
                }
                while (alreadyInUse);

                if (inUseIdx >= 0)
                {//if we already add an entry in the colorTabs, we change it
                    _bluerobotInUse[inUseIdx] = idx;
                }
                else
                {//else we add it
                    _bluerobotInUse.Add(idx);
                }
                playerRobot = Robots[idx];
                if (playerRobot == "OBSERVER")
                {
                    int spawnidx = System.Array.IndexOf(Spawns, playerSpawn);
                    _bluespawnInUse.Remove(spawnidx);
                    RpcObserverButtonOff();
                }
            }
            if (playerRobot != "OBSERVER" && playerSpawn == "OBSERVER")
            {
                RpcObserverButtonOn();
            }
        }

        [ClientRpc]
        void RpcObserverButtonOff()
        {
            playerSpawn = "OBSERVER";
            Spawnname.text = "OBSERVER";
            spawnButton.interactable = false;
        }

        [ClientRpc]
        void RpcObserverButtonOn()
        {
            spawnButton.interactable = true;
            CmdSpawnChange();
        }
        [Command]
        public void CmdSpawnChange()
        {

            if (playerColor == Color.red)
            {
                int idx = System.Array.IndexOf(Spawns, playerSpawn);

                int inUseIdx = _redspawnInUse.IndexOf(idx);

                if (idx < 0) idx = 0;

                idx = (idx + 1) % Spawns.Length;

                bool alreadyInUse = false;

                do
                {
                    alreadyInUse = false;
                    for (int i = 0; i < _redspawnInUse.Count; ++i)
                    {
                        if (_redspawnInUse[i] == idx)
                        {//that color is already in use
                            alreadyInUse = true;
                            idx = (idx + 1) % Spawns.Length;
                        }
                    }
                }
                while (alreadyInUse);

                if (inUseIdx >= 0)
                {//if we already add an entry in the colorTabs, we change it
                    _redspawnInUse[inUseIdx] = idx;
                }
                else
                {//else we add it
                    _redspawnInUse.Add(idx);
                }

                playerSpawn = Spawns[idx];
            }

            if (playerColor == Color.blue)
            {
                int idx = System.Array.IndexOf(Spawns, playerSpawn);

                int inUseIdx = _bluespawnInUse.IndexOf(idx);

                if (idx < 0) idx = 0;

                idx = (idx + 1) % Spawns.Length;

                bool alreadyInUse = false;

                do
                {
                    alreadyInUse = false;
                    for (int i = 0; i < _bluespawnInUse.Count; ++i)
                    {
                        if (_bluespawnInUse[i] == idx)
                        {//that color is already in use
                            alreadyInUse = true;
                            idx = (idx + 1) % Spawns.Length;
                        }
                    }
                }
                while (alreadyInUse);

                if (inUseIdx >= 0)
                {//if we already add an entry in the colorTabs, we change it
                    _bluespawnInUse[inUseIdx] = idx;
                }
                else
                {//else we add it
                    _bluespawnInUse.Add(idx);
                }

                playerSpawn = Spawns[idx];
            }
  
        }



        [Command]
        public void CmdNameChanged(string name)
        {
            playerName = name;
        }

        //Cleanup thing when get destroy (which happen when client kick or disconnect)
        public void OnDestroy()
        {
            LobbyPlayerList._instance.RemovePlayer(this);
            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(-1);

            int idx = System.Array.IndexOf(Colors, playerColor);

            if (idx < 0)
                return;

            for (int i = 0; i < _colorInUse.Count; ++i)
            {
                if (_colorInUse[i] == idx)
                {//that color is already in use
                    _colorInUse.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
