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

    private GameObject _playerPrefab;
    private string _mapName = null;


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
    private bool _isReadytoInstanciate = false;
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

    /*********************************************************************\
    |   Start : Initialisation								       		   |
    \*********************************************************************/

    void Start()
    {
        RefreshHostList();

        //Boutons pour Choisir son joueur
        _buttonWarriorG = GameObject.Find("ButtonWarrior");
        Button buttonWarrior = _buttonWarriorG.GetComponent<Button>();
        buttonWarrior.onClick.AddListener(() => { setUpCharacter(1); });

        _buttonPriestG = GameObject.Find("ButtonPriest");
        Button buttonPriest = _buttonPriestG.GetComponent<Button>();
        buttonPriest.onClick.AddListener(() => { setUpCharacter(2); });

        _buttonArcherG = GameObject.Find("ButtonArcher");
        Button buttonArcher = _buttonArcherG.GetComponent<Button>();
        buttonArcher.onClick.AddListener(() => { setUpCharacter(3); });



        
        //Boutons pour Choisir une Map
        _buttonMap1G = GameObject.Find("ButtonMap1");
        Button buttonMap1 = _buttonMap1G.GetComponent<Button>();
        buttonMap1.onClick.AddListener(() => { setUpMap(1); });

        /*
        _buttonMap2G = GameObject.Find("ButtonMap2");
        Button buttonMap2 = _buttonMap2G.GetComponent<Button>();
        buttonMap2.onClick.AddListener(() => { setUpMap(2); });

        _buttonMap3G = GameObject.Find("ButtonMap3");
        Button buttonMap3 = _buttonMap3G.GetComponent<Button>();
        buttonMap3.onClick.AddListener(() => { setUpMap(3); });
        */

        //Boutton pour chercher une partie
        _buttonValG = GameObject.Find("ButtonValider");
        Button buttonVal = _buttonValG.GetComponent<Button>();
        buttonVal.onClick.AddListener(() => { StartServer(); });


        //Boutton pour Rechercher une Partie
        _buttonSearchG = GameObject.Find("ButtonSearch");
        Button buttonSearch = _buttonSearchG.GetComponent<Button>();
        buttonSearch.onClick.AddListener(() => { RefreshHostList(); });

        _lobbyName = GameObject.Find("LobbyName");
        _player1name = GameObject.Find("Player1Lobby");
        _player1name.SetActive(false);
        _player2name = GameObject.Find("Player2Lobby");
        _player2name.SetActive(false);
        _player3name = GameObject.Find("Player3Lobby");
        _player3name.SetActive(false);
        _buttonValLobby = GameObject.Find("ButtonValLobby");
        Button buttonValLobby = _buttonValLobby.GetComponent<Button>();
        buttonValLobby.onClick.AddListener(() => { StartGame(); });
        _buttonValLobby.SetActive(false);
        _canvasLobby.SetActive(false);

    }

    /*********************************************************************\
    |   Update : Rafraichis le tableau des parties disponibles			   |
    \*********************************************************************/
    void Update()
    {
        if (_isRefreshingHostList && MasterServer.PollHostList().Length > 0)
        {
            _isRefreshingHostList = false;
            _hostList = MasterServer.PollHostList();
        }

        //Affichage du lancement de partie si les choix du joueur sont terminés
        if (_mapChoose && _playerChoose)
        {
            Vector3 buttonV_position = new Vector3(_buttonValG.transform.position.x, 50.0f, _buttonValG.transform.position.z);
            _buttonValG.transform.position = buttonV_position;
        }

        //Si on est le serveur et le nombre de joueurs est de 3
        if (Network.isServer && _listPlayers.Count == 3)
        {
            _buttonValLobby.SetActive(true);
        }

        if (Network.isClient && _isReadytoLaunch)
        {
            spawnPlayer();
            _isReadytoLaunch = false;
        }

        //boucle test listPlayer
        foreach (string name in _listPlayers)
        {
            Debug.Log(name);
        }

        if (_connected && Network.isServer)
        {
            string stringlistPlayer = string.Join(",", _listPlayers.ToArray());
            networkView.RPC("server_PlayerJoin", RPCMode.Others, stringlistPlayer);
            _connected = false;
        }

        if (_listPlayers.Count >= 1 && _lobbyToinit)
        {
            _canvasLobby.SetActive(true);
            if (_listPlayers.Count == 1)
            {
                _canvasLobby.SetActive(true);
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

                Vector3 buttonS_position = new Vector3(_buttonSearchG.transform.position.x, 50.0f, _buttonSearchG.transform.position.z);
                _buttonSearchG.transform.position = buttonS_position;

                for (int i = 0; i < _hostList.Length; i++)
                {   
                    if (GUI.Button(new Rect(600, 100 + (110 * i), 300, 50), _mapName + " nb players : " + (Network.connections.Length + 1)))
                    {
                        JoinServer(_hostList[i]);
                    }
                }
            }
        }
    }


    /*********************************************************************\
    |   StartServer : Initialise un serveur avec 3 joueurs maximum		  |
    \*********************************************************************/
    private void StartServer()
    {
        _canvasPlayers.SetActive(false);

        Network.InitializeServer(3, 25000, !Network.HavePublicAddress());

        //Local MasterServer
        /*
        MasterServer.ipAddress = "127.0.0.1";
        MasterServer.port = 23466;
        Network.natFacilitatorIP = "127.0.0.1";
        Network.natFacilitatorPort = 5005;
        Network.InitializeServer(3, 5005,false);
        */
        MasterServer.RegisterHost(_gameName, "4A Unity Project");
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
        _listPlayers.Add(_playerPrefab.name);
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
        networkView.RPC("server_PlayerJoinRequest", RPCMode.Server, _playerPrefab.name);
    }

    /*********************************************************************\
    |server_PlayerJoinRequest : RPC d'envoie du nom du joueur au serveur  |
    \*********************************************************************/
    [RPC]
    void server_PlayerJoinRequest(string player)
    {
        _connected = true;
        _listPlayers.Add(player);
    }


    /*********************************************************************\
    |server_PlayerJoin : RPC d'envoie du tableau de joueurs aux clients    |
    \*********************************************************************/
    [RPC]
    void server_PlayerJoin(string player)
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
    |   spawnPlayer : Crée le gameObject du joueur a partir d'un prefab	   |
    \*********************************************************************/
    private void spawnPlayer()
    {
        Network.Instantiate(_playerPrefab, Vector3.up * 5, Quaternion.identity, 0);

    }

}