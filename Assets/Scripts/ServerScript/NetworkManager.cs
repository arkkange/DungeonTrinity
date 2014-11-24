using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour
{
    private const string _gameName = "Dungeon Trinity";
    private bool _isRefreshingHostList = false;
    private HostData[] _hostList;
    private int _numberOfPlayer = 0;
    private string _mapName = null;

    private GameObject _playerPrefab;
    [SerializeField]
    private GameObject _warriorPrefab;
    [SerializeField]
    private GameObject _priestPrefab;
    [SerializeField]
    private GameObject _archerPrefab;
    [SerializeField]
    private GameObject _canvasPlayers;
    [SerializeField]
    private GameObject _canvasLobby;

    private bool _playerChoose = false;
    private bool _mapChoose = false;
    private bool _isReadytoLaunch = false;
    private bool _connected = false;
    private bool _lobbyToinit = true;

    private GameObject _buttonWarriorG;
    private GameObject _buttonPriestG;
    private GameObject _buttonArcherG;
    private GameObject _buttonMap1G;
    private GameObject _buttonMap2G;
    private GameObject _buttonMap3G;
    private GameObject _buttonValG;
    private GameObject _buttonServerG;
    private GameObject _buttonSearchG;

    private GameObject _lobbyName;
    private GameObject _player1name;
    private GameObject _player2name;
    private GameObject _player3name;
    private GameObject _buttonValLobby;

    List<string> _listPlayers = new List<string>();

    /*
    private Component _lobbyNameComp;
    private Component _player1nameComp;
    private Component _player2nameComp;
    private Component _player3nameComp; 
    */

    /*********************************************************************\
    |   Start : Initialisation des variables pour les boutons et UIText	   |
    \*********************************************************************/

    void Start()
    { 
        RefreshHostList();
         
        //Boutons pour Choisir son joueur
        _buttonWarriorG = _canvasPlayers.transform.GetChild(0).gameObject;
        Button buttonWarrior = _buttonWarriorG.GetComponent<Button>();
        buttonWarrior.onClick.AddListener(() => { setUpCharacter(1); });

        _buttonPriestG = _canvasPlayers.transform.GetChild(1).gameObject;
        Button buttonPriest = _buttonPriestG.GetComponent<Button>();
        buttonPriest.onClick.AddListener(() => { setUpCharacter(2); });

        _buttonArcherG = _canvasPlayers.transform.GetChild(2).gameObject;
        Button buttonArcher = _buttonArcherG.GetComponent<Button>();
        buttonArcher.onClick.AddListener(() => { setUpCharacter(3); });
        
        //Boutons pour Choisir une Map
        _buttonMap1G = _canvasPlayers.transform.GetChild(3).gameObject;
        Button buttonMap1 = _buttonMap1G.GetComponent<Button>();
        buttonMap1.onClick.AddListener(() => { setUpMap(1); });

        _buttonMap2G = _canvasPlayers.transform.GetChild(4).gameObject;
        Button buttonMap2 = _buttonMap2G.GetComponent<Button>();
        buttonMap2.onClick.AddListener(() => { setUpMap(2); });

        _buttonMap3G = _canvasPlayers.transform.GetChild(5).gameObject;
        Button buttonMap3 = _buttonMap3G.GetComponent<Button>();
        buttonMap3.onClick.AddListener(() => { setUpMap(3); });

        //Bouton pour chercher une partie
        _buttonValG = _canvasPlayers.transform.GetChild(6).gameObject;
        Button buttonVal = _buttonValG.GetComponent<Button>();
        buttonVal.onClick.AddListener(() => { StartServer(); });
        _buttonValG.SetActive(false);


        //Bouton pour Rechercher une Partie
        _buttonSearchG = _canvasPlayers.transform.GetChild(7).gameObject;
        Button buttonSearch = _buttonSearchG.GetComponent<Button>();
        buttonSearch.onClick.AddListener(() => { RefreshHostList(); });
        _buttonSearchG.SetActive(false);

        //Textes d'affichage des joueurs dans le lobby
        _lobbyName = _canvasLobby.transform.GetChild(0).gameObject;
        _player1name = _canvasLobby.transform.GetChild(1).gameObject;
        _player1name.SetActive(false);
        _player2name = _canvasLobby.transform.GetChild(2).gameObject;
        _player2name.SetActive(false);
        _player3name = _canvasLobby.transform.GetChild(3).gameObject;
        _player3name.SetActive(false);
        _buttonValLobby = _canvasLobby.transform.GetChild(4).gameObject;
        Button buttonValLobby = _buttonValLobby.GetComponent<Button>();
        buttonValLobby.onClick.AddListener(() => { StartGame(); });
        _buttonValLobby.SetActive(false);
        _canvasLobby.SetActive(false);

        /* 
        _lobbyNameComp = _lobbyName.GetComponent<Text>();
        _player1nameComp = _player1name.GetComponent<Text>();
        _player2nameComp = _player2name.GetComponent<Text>();
        _player3nameComp = _player3name.GetComponent<Text>();
        */
    }

    /*********************************************************************\
    |   Update : Rafraichis le tableau des parties disponibles			   |
    \*********************************************************************/
    void Update()
    {
        //rafaichis la liste des parties disponibles
        if (_isRefreshingHostList && MasterServer.PollHostList().Length > 0)
        {
            _isRefreshingHostList = false;
            _hostList = MasterServer.PollHostList();
        }

        //Affichage du lancement de partie si les choix du joueur sont terminés
        if (_mapChoose && _playerChoose)
        {
            _buttonValG.SetActive(true);
        }

        //Si on est le serveur et le nombre de joueurs est de 3
        if (Network.isServer && _listPlayers.Count == 3)
        {
            _buttonValLobby.SetActive(true);
        }

        //Si on est le client et que les autres joueurs sont dans la partie
        if (Network.isClient && _isReadytoLaunch)
        {
            spawnPlayer();
            _isReadytoLaunch = false;          
        }
        
        //Envoi de RPC de synchro si on est serveur et qu'un client se connecte
        if (_connected && Network.isServer)
        {
            networkView.RPC("addPlayerCount", RPCMode.All, _numberOfPlayer);
            string stringlistPlayer = string.Join(",", _listPlayers.ToArray());
            networkView.RPC("server_listPlayersSync", RPCMode.Others, stringlistPlayer);
            _connected = false;
        } 
        
        //Initialisation du textes sur le canvas de lobby quand des joueurs rejoignent la partie
        if (_listPlayers.Count >= 1 && _lobbyToinit)
        {
            _canvasLobby.SetActive(true);
            if (_listPlayers.Count == 1)
            { 
                _lobbyName.GetComponent<Text>().text = _mapName;
                _player1name.SetActive(true);
                _player1name.GetComponent<Text>().text = GetName(_listPlayers[0]);
            }else if (_listPlayers.Count == 2)
            {
                _lobbyName.GetComponent<Text>().text = _mapName;
                _player1name.SetActive(true);
                _player1name.GetComponent<Text>().text = GetName(_listPlayers[0]);
                _player2name.SetActive(true);
                _player2name.GetComponent<Text>().text = GetName(_listPlayers[1]);
            }else if (_listPlayers.Count == 3)
            {
                _lobbyName.GetComponent<Text>().text = _mapName;
                _player1name.SetActive(true);
                _player1name.GetComponent<Text>().text = GetName(_listPlayers[0]);
                _player2name.SetActive(true);
                _player2name.GetComponent<Text>().text = GetName(_listPlayers[1]);
                _player3name.SetActive(true);
                _player3name.GetComponent<Text>().text = GetName(_listPlayers[2]);
            }
        }
    }

    /*********************************************************************\
    | setUpCharacter : instancie la variable correspondant au nom du joueur|
    \*********************************************************************/
    public void setUpCharacter(int i)
    {
        switch (i)
        {
            case 1:
                _playerPrefab = _warriorPrefab;
                break;
            case 2:
                _playerPrefab = _priestPrefab;
                break;
            case 3:
                _playerPrefab = _archerPrefab;
                break;
        }
        _playerChoose = true;
    }

    /*********************************************************************\
    |   setUpMap : instancie la variable correspondant au nom de la map    |
    \*********************************************************************/
    public void setUpMap(int i)
    {
        switch (i)
        {
            case 1:
                _mapName = "Map no1";
                break;
            case 2:
                _mapName = "Map no2";
                break;
            case 3:
                _mapName = "Map no3";
                break;
        }
        _mapChoose = true;
    }

    /**********************************************************************************\
    |   GetName : Renvoie le nom du perso choisi                                        |
    \**********************************************************************************/
    private string GetName(string namep)
    {
        string name = null;
        switch (namep)
        {
            case "Player_warrior":
                name = "Warrior";
                break;
            case "Player_archer":
                name = "Archer";
                break;
            case "Player_priest":
                name = "Priest";
                break;
        }
        return name;
    }

    /*********************************************************************\
    |   OnGUI Interface de création et de démarage d'une partie			  |
    \*********************************************************************/
    void OnGUI()
    {
        if (_playerChoose && _mapChoose)
        {
            if (MasterServer.PollHostList().Length > 0)
            {
                _hostList = MasterServer.PollHostList();
                _buttonSearchG.SetActive(true);

                for (int i = 0; i < _hostList.Length; i++)
                {   
                    if (GUI.Button(new Rect(600, 100 + (110 * i), 300, 50), _hostList[i].gameName + " nb players : " + _numberOfPlayer))
                    {
                        JoinServer(_hostList[i]); 
                    }
                }
            }
        }
    }


    /*********************************************************************\
    |   StartServer : Initialise un serveur avec 3 joueurs maximum		  |         
    |                                                                     |  
    |    Local MasterServer Si celui d'unity est down :                   |
    |                                                                     |
    |    MasterServer.ipAddress = "127.0.0.1";                            |
    |    MasterServer.port = 23466;                                       |      
    |    Network.natFacilitatorIP = "127.0.0.1";                          |      
    |    Network.natFacilitatorPort = 5005;                               |  
    |    Network.InitializeServer(3, 5005,false);                         |  
    |                                                                     |
    \*********************************************************************/
    private void StartServer()
    {
        _canvasPlayers.SetActive(false);
        Network.InitializeServer(3, 25000, !Network.HavePublicAddress());
        MasterServer.RegisterHost(_gameName, _mapName);
    }

    /*********************************************************************\
    |   StartServer : Envoie la demande de Spawn des joueurs		       |
    \*********************************************************************/
    private void StartGame()
    {
        spawnPlayer();
        networkView.RPC("ready_launch_game", RPCMode.All);
    }


    /**********************************************************************************\
    |   OnServerInitialized : Création du Gameobject player and le joueur se connecte  |
    \**********************************************************************************/
    void OnServerInitialized()
    {
        _numberOfPlayer += 1;
        _listPlayers.Add(_playerPrefab.name);
    }


    /*********************************************************************\
    |   RefreshHosList : Rafraichis le tableau des parties disponibles	   |
    \*********************************************************************/
    private void RefreshHostList()
    {
        if (!_isRefreshingHostList)
        {
            _isRefreshingHostList = true;
            MasterServer.RequestHostList(_gameName);
        }
    }

    /*********************************************************************\
    |   JoinServer : Connecte le client au serveur						   |
    \*********************************************************************/
    private void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }

    /*********************************************************************\
    |   OnConnectedToServer : envoie le nom du joueur connecté au Serveur  |
    \*********************************************************************/
    void OnConnectedToServer()
    {
        _canvasPlayers.SetActive(false);
        networkView.RPC("listPlayersAdd", RPCMode.Server, _playerPrefab.name);
    }

    /*********************************************************************\
    |server_PlayerJoinRequest : RPC d'envoie du nom du joueur au serveur  |
    \*********************************************************************/
    [RPC]
    void listPlayersAdd(string player)
    {
        _connected = true;
        _listPlayers.Add(player);
    }


    /************************************************************************\
    |server_listPlayersSync : RPC d'envoie du tableau de joueurs aux clients  |
    \************************************************************************/
    [RPC]
    void server_listPlayersSync(string player)
    {
        List<string> newlist = new List<string>();
        foreach (string pl in player.Split(','))
        {
            newlist.Add(pl);
        }
        _listPlayers = newlist;
    }

    /*********************************************************************\
    |ready_launch_game : RPC qui indique que tous les clienst sont prêts   |
    \*********************************************************************/
    [RPC]
    void ready_launch_game()
    {
        _isReadytoLaunch = true;
    }

    /*********************************************************************\
    |addPlayerCount : RPC qui ajoute +1 aux nombres de joueurs             |
    \*********************************************************************/
    [RPC]
    void addPlayerCount(int nb)
    {
        _numberOfPlayer = nb + 1;
    }

    /*********************************************************************\
    |   spawnPlayer : Crée le gameObject du joueur a partir d'un prefab	   |
    \*********************************************************************/
    private void spawnPlayer()
    {   
        Network.Instantiate(_playerPrefab, Vector3.up * 5, Quaternion.identity, 0);
    }

}